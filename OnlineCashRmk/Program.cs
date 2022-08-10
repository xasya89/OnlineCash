using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Microsoft.Extensions.Hosting;
using OnlineCashRmk.Services;

namespace OnlineCashRmk
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            if (InstanceCheck())
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .WriteTo.File(Path.Combine("logs", "log.log"), rollingInterval: RollingInterval.Day).CreateLogger();

                var host = Host.CreateDefaultBuilder()
             .ConfigureAppConfiguration((context, builder) =>
             {
                 // Add other configuration files...
                 builder.AddJsonFile("appsettings.json", optional: true);
             })
             .ConfigureServices((context, services) =>
             {
                 services.AddHttpClient()
                //.AddDbContextFactory<DataContext>(opt=>opt.UseSqlite("Data Source=CustomerDB.db;"))
                .AddDbContextFactory<DataContext>(opt => opt.UseMySql(
                    context.Configuration.GetConnectionString("MySQL"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.30-mysql")
                    ))
                .AddLogging(configure => { configure.AddSerilog(); configure.SetMinimumLevel(LogLevel.Error | LogLevel.Warning); })

                .AddSingleton<BarCodeScanner>()
                .AddSingleton<ICashRegisterService, AtolService>()
                .AddScoped<ISynchService, SynchService>()
                .AddTransient<IConfiguration>(_ => context.Configuration)
                .AddScoped<SynchWithServerService>()
                .AddScoped<Form1>()
                .AddScoped<FormMenu>()
                //.AddTransient<PayForm>()
                .AddTransient<FormPaymentCombine>()
                .AddTransient<FormWriteOf>()
                .AddTransient<FormArrival>()
                .AddTransient<FormStocktaking>()
                .AddTransient<FormCashMoney>()
                .AddTransient<FormNewGood>()
                .AddTransient<FormFindGood>()
                .AddTransient<FormHistory>()
                //.AddHostedService<RabbitBackgroundService>()
                .AddHostedService<SynchBackgroundService>()
                .AddHostedService<ApplicationBackgroundService>();
             })
             .ConfigureLogging(logging =>
             {
                 // Add other loggers...
             })
             .UseSerilog()
             .UseConsoleLifetime()
             .Build();
                CurrentHost = host;
                await host.RunAsync();
                /*

                var services = new ServiceCollection();

                ConfigureServices.ConfigureService(services);
                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    using (var db = provider.GetRequiredService<IDbContextFactory<DataContext>>().CreateDbContext())
                        db.Database.Migrate();
                    var form1 = provider.GetRequiredService<Form1>();
                    provider.GetRequiredService<Services.ISynchBackgroundService>();
                    Application.Run(form1);
                }
                */
            }
        }
        // держим в переменной, чтобы сохранить владение им до конца пробега программы
        static Mutex InstanceCheckMutex;
        static bool InstanceCheck()
        {
            bool isNew;
            InstanceCheckMutex = new Mutex(true, "OnlineCashRmk", out isNew);
            return isNew;
        }

        private static IHost CurrentHost;
        public static async Task StopedHost() => await CurrentHost.StopAsync();
    }
}
