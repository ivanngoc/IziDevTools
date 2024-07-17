using System;
using System.Data.Common;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace IziHardGames.Libs.ForHttp
{
    public static class ExtensionForHttpResponseMessage
    {
        public static async ValueTask<bool> ValidateStatusOkAsync(this HttpResponseMessage response)
        {
            var json = JsonObject.Parse(await response.Content.ReadAsStringAsync());
            return json?["status"]?.GetValue<string>().Equals("OK", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }

        public static async Task DebugLogAsync(this HttpResponseMessage response)
        {
#if DEBUG
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.ffff")} {response.RequestMessage!.RequestUri}. Status:{(int)response.StatusCode}-{response.StatusCode}; Content:{await response.Content.ReadAsStringAsync()}");
#else
            await Task.CompletedTask;
#endif
        }
    }
}