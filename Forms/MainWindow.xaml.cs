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

namespace AnsWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Load_date(string stroka, string top)
        {
            Clients.Children.Clear();
            using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) 
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT {top}* FROM [dbo].[Client] where GenderCode like "+stroka, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Controls.Client client = new Controls.Client();
                        client.ID.Content = reader[0];
                        client.FirstName.Content = reader[1];
                        client.LastName.Content = reader[2];
                        client.Patronymic.Content = reader[3];
                        client.Birthday.Content = reader[4];
                        client.RegistrationDate.Content = reader[5];
                        client.Email.Content = reader[6];
                        client.Phone.Content = reader[7];
                        client.Gender.Content = reader[8];
                        try
                        {
                            client.Photo.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + reader[9].ToString()));
                        }
                        catch
                        {

                        }
                        client.MainWindow = this;

                        Clients.Children.Add(client); //Добавляем клиента
                    }
                }
            }
        }

        private void Gender_opr(string a)
        {
            {
                switch (Gender.SelectedIndex)
                {
                    case 0:
                        Load_date("'%%' "+ a, "");
                        break;
                    case 1:
                        Load_date("'м' " + a, "");
                        break;
                    case 2:
                        Load_date("'ж' " + a, "");
                        break;
                }
            }
        }
        private void Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gender_opr("");
            
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Gender_opr($"and (LastName like'%{Search_fio.Text}%' or FirstName like '%{Search_fio.Text}%' or Patronymic like '%{Search_fio.Text}%')");
        }

        private void Search_email_TextChanged(object sender, TextChangedEventArgs e)
        {
            Gender_opr($"and Email like '%{Search_email.Text}%'");
        }

        private void Search_number_TextChanged(object sender, TextChangedEventArgs e)
        {
            Gender_opr($"and Phone like '%{Search_number.Text}%'");
        }

        private void Check_berthday_Checked(object sender, RoutedEventArgs e)
        {
            Gender_opr($@" and Birthday like '%-{(DateTime.Now.Month.ToString().Length < 2 ? ("0" + DateTime.Now.Month.ToString())
                                                                                            : DateTime.Now.Month.ToString())}-%'");
        }

        private void Check_berthday_Unchecked(object sender, RoutedEventArgs e)
        {
            Gender_opr("");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Forms.add_edit_client add_Edit_Client = new Forms.add_edit_client();
            add_Edit_Client.ShowDialog();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            Scroll_clients.PageDown();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Scroll_clients.PageUp();
        }

        private void Scroll_clients_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void Klient_count_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Klient_count.SelectedIndex)
            {
                case 0:
                    Load_date("'%%'", "");
                    break;
                case 1:
                    Load_date("'%%'" , "TOP(10)");
                    break;
                case 2:
                    Load_date("'%%' ", "TOP (50)");
                    break;
                case 3:
                    Load_date("'%%' ", "TOP (200)");
                    break;
                default:
                    break;
            }
        }
    }
}
