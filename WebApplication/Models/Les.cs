using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Les
    {
        public virtual int les_no { set; get; }
        public virtual Sportaanbod Sportaanbod { set; get; }
        public virtual Gebruiker Sportdocent { set; get; }
        public virtual DateTime dag { set; get; }
        public virtual DateTime begintijd { set; get; }
        public virtual int aantal_deelnemers { set; get; }
        public virtual DateTime eindtijd { set; get; }
        public virtual int volgnr { set; get; }
        public virtual int max_aantal_deelnemers { set; get; }
        public virtual int losse_verkoop { set; get; }
        public virtual int vervallen { set; get; }
        public virtual int niet_tonen { set; get; }
        public virtual string dagnaam { set; get; }
        public virtual ISet<Reservering> Reserveringen { get; set; }
        public virtual LesStatus Lesstatus
        {
            get
            {
                if (this.eindtijd < DateTime.Now)
                    return LesStatus.Voorbij;
                else if (vervallen == 1)
                    return LesStatus.Vervallen;
                else if (this.Reserveringen.Count >= max_aantal_deelnemers)
                    return LesStatus.Uitverkocht;
                else if (niet_tonen == 1)
                    return LesStatus.NietTonen;
                else
                    return LesStatus.Beschikbaar;
            }
        }

        public Les()
        {

        }
    }
    public enum LesStatus {Voorbij, Uitverkocht, Vervallen, NietTonen, Beschikbaar };
}
