﻿using System;
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
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace KinoProjekt.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Page4.xaml
    /// </summary>
    /// 

    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();
            readFilmsFromDb();
        }

        public void readFilmsFromDb()
        {
            using (var db = new SqliteDbContext())
            {
                try
                {
                    var upcomingList = db.UpcomingMovies.ToList();
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
