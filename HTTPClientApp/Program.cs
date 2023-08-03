using System.Text.Json;

namespace HTTPClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new();

            Console.WriteLine("HTTP Get Method -> 1" +
                "\nHTTP Post Method -> 2" +
                "\nHTTP Put Method -> 3" +
                "\nHTTP Delete Method -> 4\n");

            int choice = Convert.ToInt32(Console.ReadLine());

            var msg = new HttpRequestMessage();
            msg.RequestUri = new Uri(@"http://localhost:27001/");

            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.NumPad1)
            {
                if (choice == 1)
                {
                    msg.Method = HttpMethod.Get;
                }
            }
            else if (key == ConsoleKey.NumPad2)
            {
                if (choice == 2)
                {
                    msg.Method = HttpMethod.Post;
                    var userFirstName = Console.ReadLine();
                    var userLastName = Console.ReadLine();
                    var userAge = Console.ReadLine();
                    UserClient user = new() { FirstName = userFirstName, LastName = userLastName, Age = userAge };
                    msg.Content = new StringContent(JsonSerializer.Serialize(user));
                }
            }
            else if (key == ConsoleKey.NumPad3)
            {
                if (choice == 3)
                {
                    msg.Method = HttpMethod.Put;
                    var Id = Convert.ToInt32(Console.ReadLine());
                    var userFirstName = Console.ReadLine();
                    var userLastName = Console.ReadLine();
                    var userAge = Console.ReadLine();
                    UserClient user = new() { Id = Id, FirstName = userFirstName, LastName = userLastName, Age = userAge };
                    msg.Content = new StringContent(JsonSerializer.Serialize(user));
                }
            }
            else if (key == ConsoleKey.NumPad4)
            {
                if (choice == 4)
                {
                    msg.Method = HttpMethod.Delete;
                    var Id = Console.ReadLine();
                    msg.Content = new StringContent(Id!);
                }
            }

            var response = await client.SendAsync(msg);
            var text = await response.Content.ReadAsStringAsync();
            Console.WriteLine(text);
        }
    }
}