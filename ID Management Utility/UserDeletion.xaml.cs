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
using System.Data.SQLite;
using Microsoft.Win32;

namespace ID_Management_Utility
{
    /// <summary>
    /// Interaction logic for UserDeletion.xaml
    /// </summary>
    public partial class UserDeletion : Window
    {
        public UserDeletion()
        {
            InitializeComponent();
        }

        private void SearchID_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(IDToFind.Text)))
            {
                string userIDToFind = IDToFind.Text;

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "select * from Users";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                bool userDNE = true;
                while (reader.Read())
                {
                    string currentID = Convert.ToString(reader["IDNumber"]);
                    if (currentID == userIDToFind)
                    {
                        LblUsername.Content = reader["firstName"] + " " + reader["lastName"];
                        LblUserID.Content = currentID;
                        string photoLocation = Convert.ToString(reader["photo"]);
                        IMGUserPhoto.Source = new BitmapImage(new Uri(photoLocation));
                        userDNE = false;
                    }
                }

                if (userDNE == true)
                {
                    MessageBox.Show("User does not exist.");
                }
                else
                {
                    LblAskToDelete.Visibility = Visibility.Visible;
                    LblUserID.Visibility = Visibility.Visible;
                    LblUsername.Visibility = Visibility.Visible;
                    LblDash.Visibility = Visibility.Visible;
                    IMGUserPhoto.Visibility = Visibility.Visible;
                    BtnCancel.Visibility = Visibility.Visible;
                    BtnDeleteUser.Visibility = Visibility.Visible;
                    LblHasBeenDeleted.Visibility = Visibility.Hidden;
                    BtnCancel.IsEnabled = true;
                    BtnDeleteUser.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please input a valid user ID.");
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "DELETE FROM USERS WHERE IDNumber = " + IDToFind.Text;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            LblHasBeenDeleted.Visibility = Visibility.Visible;
            BtnDeleteUser.IsEnabled = false;
            BtnCancel.IsEnabled = false;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            LblAskToDelete.Visibility = Visibility.Hidden;
            LblUserID.Visibility = Visibility.Hidden;
            LblUsername.Visibility = Visibility.Hidden;
            LblDash.Visibility = Visibility.Hidden;
            IMGUserPhoto.Visibility = Visibility.Hidden;
            BtnCancel.Visibility = Visibility.Hidden;
            BtnDeleteUser.Visibility = Visibility.Hidden;
            LblHasBeenDeleted.Visibility = Visibility.Hidden;

            MessageBox.Show("Deletion has been cancelled.");
        }
    }
}
