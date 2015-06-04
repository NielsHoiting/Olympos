using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    class Sportaanbod
    {
        public int sportaanbod_no { private set; get; }
        public int sportcode { private set; get; }
        public DateTime startdatum { private set; get; }
        public DateTime startdatum_verkoop { private set; get; }
        public DateTime einddatum_verkoop { private set; get; }
        public DateTime tonen_web_van { private set; get; }
        public DateTime tonen_web_tot { private set; get; }
        public int max_aantal_deelnemers { private set; get; }
        public int aantal_ingeschreven_deeln { private set; get; }
        public string sportniveau { private set; get; }
    }
}
