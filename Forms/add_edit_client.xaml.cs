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
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;

namespace AnsWPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для add_edit_client.xaml
    /// </summary>
    public partial class add_edit_client : Window
    {
        public add_edit_client()
        {
            InitializeComponent();
        }

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && !Char.IsControl(e.Text, 0) && e.Text != "(" && e.Text != ")" && e.Text != "+" && e.Text != "-")
            {
                e.Handled = true;//Тут мы хотим оствить служебные символы ()1-0+
            }
        }
        private void Add_but_Click(object sender, RoutedEventArgs e)
        {
            if (
                LastName.Text.Trim() != ""
                && FirstName.Text.Trim() != ""
                && Patronymic.Text.Trim() != ""
                && Birthday.SelectedDate != null
                && Email.Text.IndexOf("@", 0) > 3
                && Email.Text.Trim().Length > 5
                && Phone.Text.Trim().Length > 8
                )
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) // Обращение к БД
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($@"INSERT INTO [dbo].[Client]
           ([FirstName]
           ,[LastName]
           ,[Patronymic]
           ,[Birthday]
           ,[RegistrationDate]
           ,[Email]
           ,[Phone]
           ,[GenderCode]
           ,[PhotoPath])
     VALUES
           ('{FirstName.Text}'
           ,'{LastName.Text}'
           ,'{Patronymic.Text}'
           ,'{Birthday.SelectedDate}'
           ,'{DateTime.Now}'
           ,'{Email.Text}'
           ,'{Phone.Text}'
           ,'{(Gender.SelectedIndex == 0 ? "М" : "Ж")}'
           ,'Клиенты\{photo_client}')", connection);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Всё ок.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка! Проверьте данные!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Проверьте все данные!");
            }
        }
        public string photo_client { get; set; } = "default.png";

        public string ID_client { get; set; }

        private void Photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog().Value)
            {
                File.Copy(openFileDialog.FileName, AppDomain.CurrentDomain.BaseDirectory + "Клиенты\\" + openFileDialog.SafeFileName, true);
                Photo.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                photo_client = openFileDialog.SafeFileName;
            }
        }

        private void Save_client_Click(object sender, RoutedEventArgs e)
        {
            if (
    LastName.Text.Trim() != ""
    && FirstName.Text.Trim() != ""
    && Patronymic.Text.Trim() != ""
    && Birthday.SelectedDate != null
    && Email.Text.IndexOf("@", 0) > 3
    && Email.Text.Trim().Length > 5
    && Phone.Text.Trim().Length > 8
    )
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) // Обращение к БД
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($@"UPDATE [dbo].[Client]
                                                       SET [FirstName] = '{FirstName.Text}'
                                                      ,[LastName] = '{LastName.Text}'
                                                      ,[Patronymic] = '{Patronymic.Text}'
                                                      ,[Birthday] = '{Birthday.SelectedDate}'
                                                      ,[Email] = '{Email.Text}'
                                                      ,[Phone] = '{Phone.Text}'
                                                      ,[GenderCode] = '{(Gender.SelectedIndex == 0 ? "М" : "Ж")}'
                                                      
                                                 WHERE ID = {ID_client}", connection);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Всё ок.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка! Проверьте данные!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Проверьте все днанные!");
            }
        }

        private void LastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.ToCharArray()[0] >= 'A' && e.Text.ToCharArray()[0] <= 'z'
                || e.Text == "-"
                || e.Text.ToCharArray()[0] >= 'А' && e.Text.ToCharArray()[0] <= 'я')
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LastName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            /* if ((int)e.Key > 42 && (int)e.Key < 70 || (int)e.Key == 143 || (int)e.Key == 2 || (int)e.Key == 18)
             {

             }
             else
             {
                 e.Handled = true;
             }*/
        }

        public void load_tag()

        {
            Tags_panel.Children.Clear();
            using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) // Обращение к БД
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"SELECT        Tag.Color, Tag.Title
                                                    FROM            Tag INNER JOIN
                                                    TagOfClient ON Tag.ID = TagOfClient.TagID where TagOfClient.ClientID = {ID_client}", connection);
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
                            case "Green":
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
                        label.MouseLeftButtonDown += Label_MouseLeftButtonDown;
                        Tags_panel.Children.Add(label);
                    }
                }
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) // Обращение к БД
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"DELETE FROM [dbo].[TagOfClient]
                                                     WHERE ClientID={ID_client} 
                                                     and TagID=(Select ID from  Tag where Title = '{(sender as Label).Content}')", connection);
                command.ExecuteNonQuery();
                load_tag();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Add_tag_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source = K1-204-TEACHER,49172; 
                                            Initial Catalog = Sessia_ANC;
                                            Integrated Security = true;")) // Обращение к БД
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"INSERT INTO [dbo].[TagOfClient]
                                                   ([ClientID]
                                                   ,[TagID])
                                             VALUES
                                                   ({ID_client}
                                                   ,{Tag.SelectedIndex+1})", connection);
                try
                {
                    command.ExecuteNonQuery();
                    load_tag();
                    
                }
                catch
                {
                    MessageBox.Show("Данный текст уже есть!");
                }
            }
        }

        private void Add_canel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LastName_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Patronymic_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
