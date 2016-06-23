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
using System.IO;
using System.Data.SQLite;
using Microsoft.Win32;

namespace ID_Management_Utility
{
    /// <summary>
    /// Interaction logic for UserCreation.xaml
    /// </summary>
    public partial class UserCreation : Window
    {
        public UserCreation()
        {
            InitializeComponent();
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

        private void getHighestIDNumber()
        {
            // For some reason, if this method returns anything, the program freezes...
            string ID;

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select Max(IDNumber) from Users";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetValue(0) != DBNull.Value)
                {
                  //  ID = (string)reader.GetValue(0);
                    ID = Convert.ToString(reader.GetValue(0));
                    double IDNum = Convert.ToDouble(ID);
                    IDNum += 1;
                    ID = Convert.ToString(IDNum);
                    IDNumber.Text = ID;
                }
                else
                {
                    IDNumber.Text = "1";
                }
            }
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            getHighestIDNumber();
            string firstName = FirstName.Text;
            string lastName = LastName.Text;
            string IDnum = IDNumber.Text;
            string photoAddress = PhotoLocation.Text;
            string beginningBalance = BeginningBalance.Text;
            string currentBalance = BeginningBalance.Text;

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            string sql = "insert into Users (firstName,lastName, IDNumber, photo, beginningBalance, currentBalance) values ('" +firstName + "','" + lastName + "'," + IDnum + ",'" + photoAddress + "','" + beginningBalance + "','" + currentBalance + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            MessageBox.Show("User has been created. User ID is " + IDnum);
            FirstName.Clear();
            LastName.Clear();
            IDNumber.Clear();
            PhotoLocation.Clear();
            BeginningBalance.Clear();
            
        }
    }
}
