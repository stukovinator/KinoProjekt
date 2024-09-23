using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoProjekt
{
    public class Screening
    {
        public int Id { get; set; }
        public int Sala { get; set; }
        public int FilmID { get; set; } // Upewnij się, że to jest klucz obcy
        public string Data { get; set; }

        public virtual Movie Movie { get; set; } // Relacja do filmu
        public ICollection<Reservation> Reservations { get; set; }  // Lista rezerwacji dla tego seansu
    }
}
