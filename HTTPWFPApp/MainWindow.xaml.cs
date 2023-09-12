using HTTPClientApp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HTTPWFPApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new();
        public ObservableCollection<string> UsersString { get; set; } = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void GetButtonClick(object sender, RoutedEventArgs e)
        {
            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(@"http://localhost:27001/"),
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync(msg);
            var text = await response.Content.ReadAsStringAsync();
            UsersString.Clear();
            var users = JsonSerializer.Deserialize<List<UserClient>>(text);
            foreach (var user in users)
            {
                UsersString.Add(user.ToString());
            }
        }

        private async void PostButtonClick(object sender, RoutedEventArgs e)
        {
            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(@"http://localhost:27001/"),
                Method = HttpMethod.Post
            };
            AddHTTPWPF window = new AddHTTPWPF();
            window.ShowDialog();
            var userName = window.username;
            var Surname = window.surname;
            UserClient user = new UserClient { FirstName = userName, LastName = Surname};
            msg.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(msg);
            var text = await response.Content.ReadAsStringAsync();
            UsersString.Clear();
            var updatedUsers = JsonSerializer.Deserialize<List<UserClient>>(text);
            foreach (var updatedUser in updatedUsers)
            {
                UsersString.Add(updatedUser.ToString());
            }
        }

        private async void PutButtonClick(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem is not null)
            {
                var msg = new HttpRequestMessage
                {
                    RequestUri = new Uri(@"http://localhost:27001/"),
                    Method = HttpMethod.Put
                };

                var Id = UsersString[UserList.SelectedIndex].Split('-')[0].Trim();

                AddHTTPWPF window = new AddHTTPWPF();
                window.ShowDialog();
                var userName = window.username;
                var surname = window.surname;
                UserClient user = new UserClient { Id = int.Parse(Id), FirstName = userName, LastName = surname};
                msg.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(msg);
                var text = await response.Content.ReadAsStringAsync();
                UsersString.Clear();
                var updatedUsers = JsonSerializer.Deserialize<List<UserClient>>(text);
                foreach (var updatedUser in updatedUsers)
                {
                    UsersString.Add(updatedUser.ToString());
                }
            }
            else
            {
                MessageBox.Show("First Select User", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem is not null)
            {
                var msg = new HttpRequestMessage
                {
                    RequestUri = new Uri(@"http://localhost:27001/"),
                    Method = HttpMethod.Delete
                };
                var Id = UsersString[UserList.SelectedIndex].Split('-')[0].Trim();
                msg.Content = new StringContent(Id, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(msg);
                var text = await response.Content.ReadAsStringAsync();
                UsersString.Clear();
                var updatedUsers = JsonSerializer.Deserialize<List<UserClient>>(text);
                foreach (var updatedUser in updatedUsers)
                {
                    UsersString.Add(updatedUser.ToString());
                }
            }
            else
            {
                MessageBox.Show("First Select User", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
