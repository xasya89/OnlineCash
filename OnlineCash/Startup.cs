using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hangfire;
using OnlineCash.Services;
using DatabaseBuyer;
using Hangfire.MemoryStorage;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using OnlineCash.HostedServices;

namespace OnlineCash
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage());
            services.AddHangfireServer();

            services.AddSignalR();

            services.AddSession(options =>
            {
                options.Cookie.Name = "App.OnlineCash.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });
            services.AddDbContext<shopContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySQL"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.30-mysql"));
            });
            services.AddDbContext<shopbuyerContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySQLBuyer"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.30-mysql"));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/AccessDenied");
                });
            services.AddScoped<RabbitService>();
            services.AddScoped<Filters.ControlCountGoodsFilter>();
            services.AddScoped<IGoodBalanceService, GoodBalanceService>();
            services.AddScoped<ICashBoxService, CashBoxService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<IWriteofService, WriteofService>();
            services.AddScoped<ArrivalService>();
            services.AddScoped<IStockTackingService, StockTackingService>();
            services.AddScoped<ISmsService, SmsStreamTelecom>();
            services.AddTransient<CashMoneyService>();
            services.AddTransient<MoneyBalanceService>();
            services.AddScoped<NotificationOfEventInSystemService>();
            services.AddTransient<GoodCountAnalyseService>();
            services.AddTransient<RevaluationService>();
            services.AddTransient<GoodCountBalanceService>();
            services.AddTransient<GoodService>();
            //services.AddScoped<IEvotorService, EvotorService>();
            services.AddControllersWithViews(options=> {
                //options.Filters.Add(typeof(Filters.ControlDocSynchFilter));
            });
            services.AddScoped<HostedServices.BuyerObserverHostedService>();
            services.AddHostedService<UserRegisterInTelegramHostedService>();
            services.AddHostedService<BuyerBackgroundService>();
            services.AddHostedService<GoodBalanceBackgroundService>();
            services.AddHostedService<ControlDuplicateGoodBackgroundService>();
            services.AddHostedService<TelegramNotifyBackgroundService>();
        }

        public static string ShopName = "OnlineCash";
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IConfiguration configuration,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            shopContext db, shopbuyerContext dbBuyer)
        {
            int shopIdDefault = Convert.ToInt32(configuration.GetSection("ShopIdDefault").Value);

            loggerFactory.AddFile(Path.Combine("logs","log-{Date}.txt"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/robots.txt"))
                {
                    var robotsTxtPath = Path.Combine(env.ContentRootPath, "robots.txt");
                    string output = "User-agent: *  \nDisallow: /";
                    if (File.Exists(robotsTxtPath))
                    {
                        output = await File.ReadAllTextAsync(robotsTxtPath);
                    }
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(output);
                }
                else await next();
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            /*
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<Controllers.DiscountAndBuyerHub>(Controllers.DiscountAndBuyerHub.HabUrl);
            });
            */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<Controllers.DiscountAndBuyerHub>(Controllers.DiscountAndBuyerHub.HabUrl);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard();
            /*
            backgroundJobClient.Enqueue(() => Console.WriteLine("Hello Hanfire job!"));
            recurringJobManager.AddOrUpdate(
                "Run calc good balance",
                () => serviceProvider.GetService<IGoodBalanceService>().CalcAsync(shopIdDefault, DateTime.Now),
                configuration.GetSection("Jobs").GetSection("GoodBalanceCalc").Value
                );
            */

            recurringJobManager.AddOrUpdate(
                "Run calc money balance",
                () => serviceProvider.GetService<MoneyBalanceService>().Calculate(shopIdDefault, DateTime.Now),
                configuration.GetSection("Jobs").GetSection("GoodBalanceCalc").Value
                );
            recurringJobManager.RemoveIfExists(nameof(BuyerObserverHostedService));
            recurringJobManager.AddOrUpdate<BuyerObserverHostedService>(nameof(BuyerObserverHostedService),
                job => job.Run(JobCancellationToken.Null),
                Cron.MinuteInterval(1), TimeZoneInfo.Local);
            
            db.Database.Migrate();
            dbBuyer.Database.Migrate();
            ShopName = db.Shops.FirstOrDefault()?.Name ?? "OnlineCash";
        }
    }
}
