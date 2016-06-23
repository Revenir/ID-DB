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
using System.IO;
using System.Data.SQLite;

namespace ID_Management_Utility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void CreateTable()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "create table Users (firstName varchar(20),lastName varchar(20), IDNumber int,photo varchar(50), beginningBalance varchar(20), currentBalance varchar(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            string sqlCommand = "create table Balances (userIDNumber varchar(20), transactionNumber varchar(20), transactionAmount varchar(20), transactionDate varachar(20))";
            SQLiteCommand command2 = new SQLiteCommand(sqlCommand, m_dbConnection);
            command2.ExecuteNonQuery();
        }

        public bool CheckFirstTime()
        {
            if (Properties.Settings.Default.FirstUse)
            {
                Properties.Settings.Default.FirstUse = false;
                Properties.Settings.Default.Save();

                string pathString = @"C:\IDDBShared";
                if (!(Directory.Exists(pathString)))
                {
                    System.IO.Directory.CreateDirectory(pathString);
                    SQLiteConnection.CreateFile("C:\\IDDBShared\\IDDatabase.sqlite");
                }

                SQLiteConnection.CreateFile("C:\\IDDBShared\\IDDatabase.sqlite");
                CreateTable();
                return true;
            }
            else
            {
                return false;
            }
        //    string data = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        //    string path = System.IO.Path.Combine(data, name);

        //    if (Directory.Exists(path))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        // create the directory on first run
        //        DirectoryInfo di = Directory.CreateDirectory(path);
        //        SQLiteConnection.CreateFile("IDDatabase.sqlite");
        //        CreateTable();
        //        return true;
        //    }
        }

        public MainWindow()
        {
            InitializeComponent();
            bool isFirstTime = CheckFirstTime();

        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            UserCreation createUser = new UserCreation();
            createUser.Show();
        }

        private void ModifyUser_Click(object sender, RoutedEventArgs e)
        {
            ModifyUser modifyUser = new ModifyUser();
            modifyUser.Show();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            UserDeletion deleteUser = new UserDeletion();
            deleteUser.Show();
        }

        private void ViewUser_Click(object sender, RoutedEventArgs e)
        {
            UserViewing viewUser = new UserViewing();
            viewUser.Show();
        }

        private void PrintID_Click(object sender, RoutedEventArgs e)
        {
            PrintID printID = new PrintID();
            printID.Show();
        }

        private void ViewDatabase_Click(object sender, RoutedEventArgs e)
        {
            ViewDB viewDB = new ViewDB();
            viewDB.Show();
        }
    }
}
