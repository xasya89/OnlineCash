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
using Hangfire.MemoryStorage;
using System.IO;

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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
            services.AddScoped<Filters.ControlCountGoodsFilter>();
            services.AddScoped<IGoodBalanceService, GoodBalanceService>();
            services.AddScoped<ICashBoxService, CashBoxService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<IWriteofService, WriteofService>();
            services.AddScoped<ArrivalService>();
            services.AddScoped<IStockTackingService, StockTackingService>();
            services.AddTransient<BuyerRegistration>();
            services.AddScoped<ISmsService, SmsStreamTelecom>();
            services.AddTransient<CashMoneyService>();
            services.AddTransient<MoneyBalanceService>();
            services.AddSingleton<NotificationOfEventInSystemService>();
            services.AddTransient<GoodCountAnalyseService>();
            services.AddTransient<RevaluationService>();
            //services.AddScoped<IEvotorService, EvotorService>();
            services.AddControllersWithViews(options=> {
                options.Filters.Add(typeof(Filters.ControlCountGoodsFilter));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IConfiguration configuration,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            shopContext db)
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
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard();
            backgroundJobClient.Enqueue(() => Console.WriteLine("Hello Hanfire job!"));
            recurringJobManager.AddOrUpdate(
                "Run calc good balance",
                () => serviceProvider.GetService<IGoodBalanceService>().CalcAsync(shopIdDefault, DateTime.Now),
                configuration.GetSection("Jobs").GetSection("GoodBalanceCalc").Value
                );

            recurringJobManager.AddOrUpdate(
                "Run calc money balance",
                () => serviceProvider.GetService<MoneyBalanceService>().Calculate(shopIdDefault, DateTime.Now),
                configuration.GetSection("Jobs").GetSection("GoodBalanceCalc").Value
                );
            db.Database.Migrate();
        }
    }
}
