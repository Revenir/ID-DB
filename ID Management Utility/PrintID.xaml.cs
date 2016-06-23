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
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;


namespace ID_Management_Utility
{
    /// <summary>
    /// Interaction logic for PrintID.xaml
    /// </summary>
    public partial class PrintID : Window
    {
        public PrintID()
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
                        UserName.Content = reader["firstName"] + " " + reader["lastName"];
                        IDNumber.Content = currentID;
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
                    IMGUserPhoto.Visibility = Visibility.Visible;
                    UserName.Visibility = Visibility.Visible;
                    IDNumber.Visibility = Visibility.Visible;
                    IDPrintBox.Visibility = Visibility.Visible;
                    Print.Visibility = Visibility.Visible;
                    Print.IsEnabled = true;
                    Cancel.Visibility = Visibility.Visible;
                    Cancel.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please Input a User ID.");
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.PrintVisual(IDPrintBox, "ID Printing.");
        }

        private void Cancel_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Printing has been canceled.");
            IMGUserPhoto.Visibility = Visibility.Hidden;
            UserName.Visibility = Visibility.Hidden;
            IDNumber.Visibility = Visibility.Hidden;
            IDPrintBox.Visibility = Visibility.Hidden;
            Print.Visibility = Visibility.Hidden;
            Print.IsEnabled = false;
            Cancel.Visibility = Visibility.Hidden;
            Cancel.IsEnabled = false;
        }

    }
}
