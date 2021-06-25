using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using DataBase;

namespace RMK.Infrastructure
{
    public class SendingShifts
    {
        string urlServer;
        public SendingShifts(IConfiguration configuration)
        {
            urlServer = configuration.GetSection("UrlServer").Value;
        }
        public void Send()
        {
            
        }
    }
}
