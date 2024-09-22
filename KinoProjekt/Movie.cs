using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoProjekt
{
    public class Movie
    {
        public int Id { get; set; }
        public string Tytul { get; set; }
        public double Ocena { get; set; }
        public int IloscGlosow { get; set; }
        public string Opis { get; set; }
        public byte[] Plakat { get; set; }

        // Opcjonalnie: dodaj relację, jeśli chcesz mieć dostęp do seansów
        public virtual ICollection<Screening> Screenings { get; set; }
    }
}
