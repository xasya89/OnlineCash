using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCash
{
    public static class ClientServer
    {
        // HttpClient создается 1 раз на все время работы приложения
        private static readonly HttpClient client = new HttpClient();

        // POST
        private static async Task<string> PostRequestAsync(string url, string json)
        {
            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        // GET
        private static async Task<string> GetRequestAsync(string url)
        {
            using HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
