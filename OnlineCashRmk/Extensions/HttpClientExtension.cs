using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCashRmk.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddDefaultHeaders(this HttpClient httpClient, Guid uuid)
        {
            httpClient.DefaultRequestHeaders.Add("doc-uuid", uuid.ToString());
        }
    }
}
