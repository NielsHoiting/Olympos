using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    class Reservering
    {
        public virtual int reservering_no { set; get; }
        public virtual Les Les { set; get; }
        public virtual Gebruiker Deelnemer { set; get; }
        public virtual DateTime datum_reservering { set; get; }
        public virtual bool is_geweest { set; get; }
    }
}
