using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Gebruiker
    {
        public virtual int sco_nummer { set; get; }
        public virtual DateTime geldig_tot { set; get; }
        public virtual string rol { set; get; }
        public virtual string wachtwoord { set; get; }
        public virtual string voornaam { set; get; }
        public virtual string tussenvoegsel { set; get; }
        public virtual string achternaam { set; get; }
        public virtual DateTime geboortedatum { set; get; }
        public virtual string email_adres { set; get; }
        public Gebruiker()
        {

        }
    }
}
