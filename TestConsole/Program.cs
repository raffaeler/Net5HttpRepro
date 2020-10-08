using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public partial class Program
    {
        private HttpClient _client;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            await new Program().Start();

            Console.WriteLine("End");
            Console.ReadKey();
        }

        private async Task Start()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            await SendReuse($"Hello, world");

            //foreach (var item in Enumerable.Range(1, 100))
            //{
            //    await SendReuse($"Hello, world {item}");
            //}


            // =========================================================
            // if I dispose the client, the problem does not show up
            //_client.Dispose();
            // =========================================================
        }

        private async Task SendReuse(string textMessage)
        {
            try
            {
                var message = $"\"{textMessage}\"";

                using (var content = new StringContent(message, Encoding.UTF8, "application/json"))
                {
                    using (var response = await _client.PostAsync("http://localhost:5000/Logger", content))
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (Exception err)
            {
                var errorText = err.ToString();
                Debug.WriteLine(errorText);

                await _client.PostAsync("Logger",
                    new StringContent(errorText, Encoding.UTF8, "application/json"));
            }
        }

        // This does not cause the exceptions
        private async Task SendNew(string textMessage)
        {
            try
            {
                var message = $"\"{textMessage}\"";

                using (var freshClient = new HttpClient())
                {
                    using (var content = new StringContent(message, Encoding.UTF8, "application/json"))
                    {
                        using (var response = await freshClient.PostAsync("http://localhost:5000/Logger", content))
                        {
                            response.EnsureSuccessStatusCode();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                var errorText = err.ToString();
                Debug.WriteLine(errorText);
            }
        }


    }

}
