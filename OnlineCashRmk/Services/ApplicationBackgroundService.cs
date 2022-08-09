using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCashRmk.Services
{
    internal class ApplicationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        public ApplicationBackgroundService(IServiceProvider services)
        {
            this._services = services;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = _services.GetRequiredService<Form1>();
            using var db = _services.GetRequiredService<IDbContextFactory<DataContext>>().CreateDbContext();
            db.Database.Migrate();
            Application.Run(mainForm);

            return Task.CompletedTask;
        }
    }
}
