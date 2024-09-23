using KinoProjekt.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoProjekt
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<UpcomingMovie> UpcomingMovies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public string DbPath { get; }

        public SqliteDbContext()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            DbPath = System.IO.Path.Combine(folder, "KinoProjekt.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MAPOWANIE TABELI FILMY
            modelBuilder.Entity<Movie>().ToTable("Filmy");
            modelBuilder.Entity<Movie>()
                .HasIndex(f => f.Id)
                .IsUnique();

            // MAPOWANIE TABELI NADCHODZACE_FILMY
            modelBuilder.Entity<UpcomingMovie>().ToTable("Nadchodzace_filmy");

            // MAPOWANIE TABELI UZYTKOWNICY
            modelBuilder.Entity<User>().ToTable("Uzytkownicy");
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();

            // MAPOWANIE TABELI SEANSE
            modelBuilder.Entity<Screening>().ToTable("Seanse");
            modelBuilder.Entity<Screening>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Screenings)
                .HasForeignKey(s => s.FilmID);


            // MAPOWANIE TABELI REZERWACJE
            modelBuilder.Entity<Reservation>().ToTable("Rezerwacje");
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UzytkownikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Screening)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.SeansId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => new { r.UzytkownikId, r.SeansId, r.NrSiedzenia })
                .IsUnique();
        }
    }
}
