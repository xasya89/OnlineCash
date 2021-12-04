using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;

namespace OnlineCash.Services
{
    public class SmsStreamTelecom : ISmsService
    {
        string login;
        string password;
        string sourceAdress;
        static string sessionId;
        public SmsStreamTelecom(IConfiguration configuration)
        {
            login = configuration.GetSection("StreamTelecom").GetSection("Login").Value;
            password = configuration.GetSection("StreamTelecom").GetSection("Password").Value;
            sourceAdress = configuration.GetSection("StreamTelecom").GetSection("SourceAddress").Value;
            sessionId = GetSessionId().Result;
        }

        public async Task<bool> Send(string phone, string message)
        {
            var resp = await "https://gateway.api.sc/rest/Send/SendSms/".PostMultipartAsync(p =>
              p.AddString("sessionId", sessionId)
              .AddString("destinationAddress", phone.Trim().Replace("+", ""))
              .AddString("data", message)
              .AddString("sourceAddress", sourceAdress)
            ).ReceiveJson<string>();
            return true;
        }

        private async Task<string> GetSessionId()
        {
            var str=await "https://gateway.api.sc/rest/Session/".PostMultipartAsync(p => p.AddString("login", login).AddString("password", password))
                .ReceiveString();
            return str.Replace("\"", "");
        }
    }
}
