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
using System.Data;

namespace ID_Management_Utility
{
    /// <summary>
    /// Interaction logic for ViewReport.xaml
    /// </summary>
    public partial class ViewReport : Window
    {
        public ViewReport(string UserID)
        {
            InitializeComponent();
            userID.Content = UserID;
            FillData();
        }

        void FillData()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            string finder = "select * from Balances WHERE userIDNumber = " + userID.Content; 

            using (SQLiteDataAdapter myAdapter = new SQLiteDataAdapter(finder, m_dbConnection))
            {
                DataTable myData = new DataTable();
                myAdapter.Fill(myData);
                YouthTransactions.ItemsSource = myData.AsDataView();
            }
        }

        private void PrintID_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.PrintVisual(YouthTransactions, "Youth Transactions");
        }
    }
}
