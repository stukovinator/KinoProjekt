using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoProjekt
{
    public class Reservation
    {
        public int Id {  get; set; }
        public int UzytkownikId { get; set; }
        public int SeansId { get; set; }
        public int NrSiedzenia {  get; set; }

        public User User {  get; set; }
        public Screening Screening { get; set; }
    }
}
