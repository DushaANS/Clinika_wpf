using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Data.SqlClient;

namespace AnsWPF.Controls
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : UserControl
    {
        public Client()
        {
            InitializeComponent();
        }
        public MainWindow MainWindow;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) // Обращение к БД
            {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"DELETE FROM [dbo].[Client] WHERE Id = {ID.Content}", connection);
                    command.ExecuteNonQuery();
                    MainWindow.Load_date("'%%'", "");

            }
        }

        private void Edit_but_Click(object sender, RoutedEventArgs e)
        {
            Forms.add_edit_client add_Edit_Client = new Forms.add_edit_client();
            add_Edit_Client.LastName.Text = LastName.Content.ToString();
            add_Edit_Client.FirstName.Text = FirstName.Content.ToString();
            add_Edit_Client.Patronymic.Text = Patronymic.Content.ToString();
            add_Edit_Client.Birthday.SelectedDate = Convert.ToDateTime(Birthday.Content.ToString());
            add_Edit_Client.Email.Text = Email.Content.ToString();
            add_Edit_Client.Photo.Source = Photo.Source;
            add_Edit_Client.Phone.Text = Phone.Content.ToString();
            add_Edit_Client.photo_client = Photo.Source.ToString().Remove(0, Photo.Source.ToString().LastIndexOf('/') + 1);
            add_Edit_Client.ID_client = ID.Content.ToString();
            add_Edit_Client.Add_but.Visibility = Visibility.Hidden;
            add_Edit_Client.Save_client.Visibility = Visibility.Visible;
            add_Edit_Client.Tags_panel.Visibility = Visibility.Visible;
            add_Edit_Client.Tag.Visibility = Visibility.Visible;
            add_Edit_Client.Add_tag.Visibility = Visibility.Visible;
            add_Edit_Client.load_tag();
            add_Edit_Client.Gender.SelectedIndex = Gender.Content.ToString() == "1" ? 0 : 1;
            add_Edit_Client.ShowDialog();
            MainWindow.Load_date("'%%'", "");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) // Обращение к БД
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"SELECT        Tag.Color, Tag.Title
                                                    FROM            Tag INNER JOIN
                                                    TagOfClient ON Tag.ID = TagOfClient.TagID where TagOfClient.ClientID = {ID.Content}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Label label = null;
                        switch (reader[0].ToString().Trim())
                        {
                            case "Red":
                                label = new Label
                                {
                                    Content = reader[1],
                                    Background = Brushes.Red
                                };
                                break;
                            case "Gren":
                                label = new Label
                                {
                                    Content = reader[1],
                                    Background = Brushes.Green
                                };
                                break;
                            case "Blue":
                                label = new Label
                                {
                                    Content = reader[1],
                                    Background = Brushes.Blue
                                };
                                break;
                            default:
                                break;
                        }
                        
                    }
                }
            }
        }
    }
}
