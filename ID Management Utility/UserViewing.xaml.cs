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
    /// Interaction logic for UserViewing.xaml
    /// </summary>
    public partial class UserViewing : Window
    {
        public string CurrentUserID()
        {
            return IDToFind.Text;
        }
        public UserViewing()
        {
            InitializeComponent();
        }

        public bool FindByIDNumber(string userIDToFind)
        {
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
                    StartingBalance.Content = "$" + Convert.ToString(reader["beginningBalance"]);
                    Current_Balance.Content = "$" + Convert.ToString(reader["currentBalance"]);
                    string photoLocation = Convert.ToString(reader["photo"]);
                    IMGUserPhoto.Source = new BitmapImage(new Uri(photoLocation));
                    userDNE = false;
                }
            }

            return userDNE;
        }

        public bool FindIDByName(string FirstNameToFind, string LastNameToFind)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from Users";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            bool userDNE = true;
            while (reader.Read())
            {
                string currentFirstName = Convert.ToString(reader["firstName"]);
                string currentLastName = Convert.ToString(reader["lastName"]);
                if (currentFirstName == FirstNameToFind && currentLastName == LastNameToFind)
                {
                    UserName.Content = reader["firstName"] + " " + reader["lastName"];
                    IDNumber.Content = Convert.ToString(reader["IDNumber"]);
                    StartingBalance.Content = "$" + Convert.ToString(reader["beginningBalance"]);
                    Current_Balance.Content = "$" + Convert.ToString(reader["currentBalance"]);
                    string photoLocation = Convert.ToString(reader["photo"]);
                    IMGUserPhoto.Source = new BitmapImage(new Uri(photoLocation));
                    userDNE = false;
                }
            }

            return userDNE;            
        }

        private void SearchID_Click(object sender, RoutedEventArgs e)
        {
            if(!(string.IsNullOrEmpty(IDToFind.Text)))
            {
                string userIDToFind = IDToFind.Text;
                double number;
                bool isNumeric = double.TryParse(userIDToFind, out number);
                bool userDNE = true;

                if (isNumeric)
                {
                    userDNE = FindByIDNumber(userIDToFind);
                }
                else
                {
                    string[] name = userIDToFind.Split(' ');
                    if (name.Length == 2)
                    {
                        userDNE = FindIDByName(name[0], name[1]);
                    }
                    else
                    {
                        userDNE = true;
                    }
                }

                if (userDNE == true)
                {
                    MessageBox.Show("User does not exist.");
                }
                else
                {
                    LblCurrentBalance.Visibility = Visibility.Visible;
                    LblStartingBalance.Visibility = Visibility.Visible;
                    LblUserID.Visibility = Visibility.Visible;
                    LblUserName.Visibility = Visibility.Visible;
                    IMGUserPhoto.Visibility = Visibility.Visible;
                    UserName.Visibility = Visibility.Visible;
                    IDNumber.Visibility = Visibility.Visible;
                    StartingBalance.Visibility = Visibility.Visible;
                    Current_Balance.Visibility = Visibility.Visible;
                    btnViewReport.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("Please Input a User ID.");
            }
        }

        private void btnViewReport_Click(object sender, RoutedEventArgs e)
        {
            ViewReport viewRp = new ViewReport(IDToFind.Text);
            viewRp.Show();
        }
    }
}
