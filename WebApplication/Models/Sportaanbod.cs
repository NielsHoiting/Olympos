using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    class Sportaanbod
    {
        public virtual int SportaanbodNo { set; get; }
        public virtual string Sportcode { set; get; }
        public virtual DateTime Startdatum { set; get; }
        public virtual DateTime StartdatumVerkoop { set; get; }
        public virtual DateTime EinddatumVerkoop { set; get; }
        public virtual DateTime TonenWebVan { set; get; }
        public virtual DateTime TonenWebTot { set; get; }
        public virtual int MaxAantalDeelnemers { set; get; }
        public virtual int AantalIngeschrevenDeelnemers { set; get; }
        public virtual string SportNiveau { set; get; }
        public virtual ISet<Les> Lessen { get; set; }
    }
}
