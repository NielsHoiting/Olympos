using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Reservering
    {
        public virtual int reservering_no { set; get; }
        public virtual Les Les { set; get; }
        public virtual Gebruiker Deelnemer { set; get; }
        public virtual DateTime datum_reservering { set; get; }
        public virtual bool is_geweest { set; get; }
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;
            Reservering r = (Reservering) obj;
            System.Diagnostics.Debug.WriteLine("equals");
            return r.Les == this.Les && r.Deelnemer == this.Deelnemer;
        }
    }

}
