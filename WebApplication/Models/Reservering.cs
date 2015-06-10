using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    class Reservering
    {
        public int reservering_no { private set; get; }
        public int les_per_sportaanbod_les_no { private set; get; }
        public int deelnemer_sco_nummer { private set; get; }
        public DateTime datum_reservering { private set; get; }
        public bool is_geweest { private set; get; }
    }
}
