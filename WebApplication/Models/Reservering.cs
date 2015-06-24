using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Reservering : IEquatable<Reservering>
    {
        public virtual int reservering_no { set; get; }
        public virtual Les Les { set; get; }
        public virtual Gebruiker Deelnemer { set; get; }
        public virtual DateTime datum_reservering { set; get; }
        public virtual bool is_geweest { set; get; }
        
        public virtual bool Equals(Reservering r)
        {
            if (r == null)
            {
                return false;
            }
            return r.Les.les_no == this.Les.les_no && r.Deelnemer.sco_nummer == this.Deelnemer.sco_nummer;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;
            Reservering r = (Reservering)obj;
            return r.Les.les_no == this.Les.les_no && r.Deelnemer.sco_nummer == this.Deelnemer.sco_nummer;
        }

        public override int GetHashCode()
        {
            return Les.les_no.GetHashCode() ^ Deelnemer.sco_nummer.GetHashCode();
        }
    }

}
