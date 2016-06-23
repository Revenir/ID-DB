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
using Microsoft.Win32;
using System.Data.SQLite;

namespace ID_Management_Utility
{
    /// <summary>
    /// Interaction logic for ModifyUser.xaml
    /// </summary>
    public partial class ModifyUser : Window
    {
        public ModifyUser()
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
                    FirstName.Text = Convert.ToString(reader["firstName"]);
                    LastName.Text = Convert.ToString(reader["lastName"]);
                    IDNumber.Text = currentID;
                    BeginningBalance.Text = Convert.ToString(reader["beginningBalance"]);
                    CurrentBalance.Text = Convert.ToString(reader["currentBalance"]);
                    PhotoLocation.Text = Convert.ToString(reader["photo"]);
                    userDNE = false;
                    initialBalance.Content = CurrentBalance.Text;
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
                    FirstName.Text = Convert.ToString(reader["firstName"]);
                    LastName.Text = Convert.ToString(reader["lastName"]);
                    IDNumber.Text = Convert.ToString(reader["IDNumber"]);
                    BeginningBalance.Text = Convert.ToString(reader["beginningBalance"]);
                    CurrentBalance.Text = Convert.ToString(reader["currentBalance"]);
                    PhotoLocation.Text = Convert.ToString(reader["photo"]);
                    initialBalance.Content = CurrentBalance.Text;
                    userDNE = false;
                }
            }

            return userDNE;
        }

        private void SearchID_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(IDToFind.Text)))
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
                    LblBeginningBalance.Visibility = Visibility.Visible;
                    LblCurrentBalance.Visibility = Visibility.Visible;
                    LblFirstName.Visibility = Visibility.Visible;
                    LblIDNumber.Visibility = Visibility.Visible;
                    LblLastName.Visibility = Visibility.Visible;
                    LblPhoto.Visibility = Visibility.Visible;
                    FirstName.Visibility = Visibility.Visible;
                    LastName.Visibility = Visibility.Visible;
                    BeginningBalance.Visibility = Visibility.Visible;
                    LastName.Visibility = Visibility.Visible;
                    IDNumber.Visibility = Visibility.Visible;
                    PhotoLocation.Visibility = Visibility.Visible;
                    UploadPhoto.Visibility = Visibility.Visible;
                    CurrentBalance.Visibility = Visibility.Visible;
                    ModifyUser1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("Please Input a User ID.");
            }
        }
        private void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                PhotoLocation.Text = op.FileName;
            }
        }

        private void getHighestTransactionNumber()
        {
            // For some reason, if this method returns anything, the program freezes...
            string ID;

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select Max(transactionNumber) from Balances";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                    ID = (string)reader.GetValue(0);
                    transID.Content = ID;
                }
                else
                {
                    transID.Content = "1";
                }
            }
        }

        private void ModifyUser1_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "UPDATE USERS SET firstName = '" + FirstName.Text + "', lastName = '" + LastName.Text + "',"
                + "beginningBalance = '" + BeginningBalance.Text + "', currentBalance = '" + CurrentBalance.Text + "'," +
                "photo = '" + PhotoLocation.Text + "' WHERE IDNumber = " + IDNumber.Text;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            double initialBal = Convert.ToDouble(initialBalance.Content);
            double currentBal = Convert.ToDouble(CurrentBalance.Text);
            if (initialBal != currentBal)
            {
                getHighestTransactionNumber();
                int transactionIDNumber = Convert.ToInt16(transID.Content) + 1;
                double balanceDifference = initialBal - currentBal;
                string transactionAmount = Convert.ToString(balanceDifference);
                string transactionID = Convert.ToString(transactionIDNumber);
                DateTime thisDay = DateTime.Today;
                string date = thisDay.ToString("d");

                string sqlCommand = "insert into Balances (userIDNumber, transactionNumber, transactionAmount, transactionDate) values ('" + IDNumber.Text + "','" + transactionID + "'," + transactionAmount + ",'" + date + "')";
                SQLiteCommand commandSql = new SQLiteCommand(sqlCommand, m_dbConnection);
                commandSql.ExecuteNonQuery();
            }

            LblBeginningBalance.Visibility = Visibility.Hidden;
            LblCurrentBalance.Visibility = Visibility.Hidden;
            LblFirstName.Visibility = Visibility.Hidden;
            LblIDNumber.Visibility = Visibility.Hidden;
            LblLastName.Visibility = Visibility.Hidden;
            LblPhoto.Visibility = Visibility.Hidden;
            FirstName.Visibility = Visibility.Hidden;
            LastName.Visibility = Visibility.Hidden;
            BeginningBalance.Visibility = Visibility.Hidden;
            LastName.Visibility = Visibility.Hidden;
            IDNumber.Visibility = Visibility.Hidden;
            PhotoLocation.Visibility = Visibility.Hidden;
            UploadPhoto.Visibility = Visibility.Hidden;
            CurrentBalance.Visibility = Visibility.Hidden;
            ModifyUser1.Visibility = Visibility.Hidden;

            MessageBox.Show("User has been successfully modified.");
        }

        private void UploadPhoto_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                PhotoLocation.Text = op.FileName;
            }
        }
    }
}
