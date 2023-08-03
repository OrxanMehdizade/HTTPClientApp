using HTTPServerApp;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HTTPServerApp
{

    class Program
    {
        static void Main(string[] args)
        {
            List<UserServer> users = new()
            {
                new(){ FirstName = "Orxan", LastName = "Mehdizade", Age = "22" },
                new(){ FirstName = "Abbas", LastName = "Bagirov", Age = "42" },
                new(){ FirstName = "Nadir", LastName = "Nadirli", Age = "35" },
            };

            HttpListener listener = new();
            listener.Prefixes.Add(@"http://localhost:27001/");
            listener.Start();

            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                StreamWriter streamWriter = new(response.OutputStream);
                StreamReader reader = new(request.InputStream);

                if (request.HttpMethod == "GET")
                {
                    var text = JsonSerializer.Serialize(users);
                    streamWriter.WriteLine(text);
                    streamWriter.Close();
                }
                else if (request.HttpMethod == "POST")
                {
                    var id = UserServer.staticID;
                    var newUser = JsonSerializer.Deserialize<UserServer>(reader.ReadToEnd());
                    newUser!.Id = id;
                    users.Add(newUser);
                    var text = JsonSerializer.Serialize(users);
                    streamWriter.WriteLine(text);
                    streamWriter.Close();
                }
                else if (request.HttpMethod == "PUT")
                {
                    var updateUser = JsonSerializer.Deserialize<UserServer>(reader.ReadToEnd());
                    UserServer.staticID--;
                    users.FirstOrDefault(u => u.Id == updateUser!.Id)!.FirstName = updateUser!.FirstName;
                    users.FirstOrDefault(u => u.Id == updateUser!.Id)!.LastName = updateUser!.LastName;
                    users.FirstOrDefault(u => u.Id == updateUser!.Id)!.Age = updateUser!.Age;
                    var text = JsonSerializer.Serialize(users);
                    streamWriter.WriteLine(text);
                    streamWriter.Close();
                }
                else if (request.HttpMethod == "DELETE")
                {
                    var deleteId = Convert.ToInt32(reader.ReadToEnd());
                    users.Remove(users.FirstOrDefault(u => u.Id == deleteId)!);
                    var text = JsonSerializer.Serialize(users);
                    streamWriter.WriteLine(text);
                    streamWriter.Close();
                }
            }
        }
    }
}
