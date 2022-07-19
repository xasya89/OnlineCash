using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data;
using Dapper;
using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace OnlineCash.HostedServices
{
    public class ControlDuplicateGoodBackgroundService : BackgroundService
    {
        ILogger<ControlDuplicateGoodBackgroundService> _logger;
        IConfiguration _configuration;
        public ControlDuplicateGoodBackgroundService(IConfiguration configuration, ILogger<ControlDuplicateGoodBackgroundService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using MySqlConnection con = new MySqlConnection(_configuration.GetConnectionString("MySQL"));
                    con.Open();
                    using MySqlTransaction tr = con.BeginTransaction();

                    var duplicates = await con.QueryAsync<GoodCountHistory>("SELECT *, count(*) as CountDuplicate FROM goodcountdochistories GROUP BY goodid, docid having CountDuplicate>1");
                    foreach (GoodCountHistory duplicate in duplicates)
                    {
                        await con.ExecuteAsync($"UPDATE goodcountbalancecurrents SET Count=Count - {duplicate.Count.ToStringWithDotted()} WHERE GoodId={duplicate.GoodId}");
                        await con.ExecuteAsync($"DELETE  FROM goodcountdochistories WHERE id={duplicate.Id}");
                    }
                    tr.Commit();
                    con.Close();
                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
            }
            catch(MySqlException ex)
            {
                _logger.LogError($"Duplicate background service - {ex.Message}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Duplicate background service - {ex.Message}");
            }
        }

        private class GoodCountHistory
        {
            public int Id { get; set; }
            public int GoodId { get; set; }
            public decimal Count { get; set; }
            public int CountDuplicate { get; set; }
        }
    }
}
