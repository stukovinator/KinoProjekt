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
using System.Data.SQLite;
using System.Data;

namespace KinoProjekt.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Page4.xaml
    /// </summary>
    /// 

    public class UpcomingMovie
    {
        public string Tytul { get; set; }
        public string DataPremiery { get; set; }
    }

    public partial class Page4 : Page
    {
        private string dbPath = "DataSource=C:/Users/denic/source/repos/KinoProjekt/KinoProjekt/KinoProjekt.db;Version=3;";
        public Page4()
        {
            InitializeComponent();
            readFilmsFromDb();
        }

        public void readFilmsFromDb()
        {
            List<UpcomingMovie> upcomingList = new List<UpcomingMovie>();

            using(SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Tytul, DataPremiery FROM Nadchodzace_filmy;";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        upcomingList.Add(new UpcomingMovie { Tytul = reader["Tytul"].ToString(), DataPremiery = reader["DataPremiery"].ToString() });
                    }
                    
                    upcomingMoviesList.ItemsSource = upcomingList;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Błąd wczytywania danych: " + ex.Message);
                }
            }
        }
    }
}
