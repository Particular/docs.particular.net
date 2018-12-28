using System;

namespace Client
{
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main()
        {
            Console.Title = "Client";

            const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
            var random = new Random();
            var client = new HttpClient();

            Console.WriteLine("Press enter to call the order create HTTP API");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }
                var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                Console.WriteLine($"Placing order {orderId}");

                var content = new StringContent($"\"{orderId}\"");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync("http://localhost:5000/", content, CancellationToken.None);
                Console.WriteLine(response.StatusCode);
            }
        }
    }
}
