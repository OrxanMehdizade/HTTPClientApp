using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HTTPWFPApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new();
        private List<wpfuser> Users = new();
        public ObservableCollection<string> UsersString { get; set; } = new();

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
            Users.Clear();
            var users = JsonSerializer.Deserialize<List<wpfuser>>(text)!;
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    users.ForEach(u => UsersString.Add(u.ToString()));
                    users.ForEach(u => Users.Add(u));
                });
            });
        }

        private async void PostButtonClick(object sender, RoutedEventArgs e)
        {
            var msg = new HttpRequestMessage
            {
                RequestUri = new Uri(@"http://localhost:27001/"),
                Method = HttpMethod.Post
            };
            AddHTTPWPF window = new();
            window.ShowDialog();
            var userName = window.username;
            var Surname = window.surname;
            wpfuser user = new() { Name = userName, Surname = Surname };
            msg.Content = new StringContent(JsonSerializer.Serialize(user));
            var response = await client.SendAsync(msg);
            var text = await response.Content.ReadAsStringAsync();
            Users.Clear();
            var users = JsonSerializer.Deserialize<List<wpfuser>>(text)!;
            users.ForEach(u => Users.Add(u));
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

                var Id = Users[UserList.SelectedIndex].Id;

                AddHTTPWPF window = new();
                window.ShowDialog();
                var userName = window.username;
                var surname = window.surname;
                wpfuser user = new() { Id = Id, Name = userName, Surname = surname };
                msg.Content = new StringContent(JsonSerializer.Serialize(user));
                var response = await client.SendAsync(msg);
            }
            else MessageBox.Show("First Select User", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);


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
                var Id = Users[UserList.SelectedIndex].Id;
                msg.Content = new StringContent(Id.ToString());
                var response = await client.SendAsync(msg);
            }
            else MessageBox.Show("First Select User", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
