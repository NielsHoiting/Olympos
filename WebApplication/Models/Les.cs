using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    class Les
    {
        public int les_no { private set; get; }
        public int sportdocent_sco_no { private set; get; }
        public int sportaanbod_no { private set; get; }
        public DateTime dag { private set; get; }
        public DateTime begintijd { private set; get; }
        public DateTime eindtijd { private set; get; }
        public int aantal_deelnemers { private set; get; }
        public int max_aantal_deelnemers { private set; get; }
        public int losse_verkoop { private set; get; }
        public int vervallen { private set; get; }

    }
}
