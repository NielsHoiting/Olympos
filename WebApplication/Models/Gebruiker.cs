using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Gebruiker
    {
        public int sco_nummer { private set; get; }
        public DateTime geldig_tot { private set; get; }
        public string rol { private set; get; }
        public string wachtwoord { private set; get; }
        public string voornaam { private set; get; }
        public string tussenvoegsel { private set; get; }
        public string achternaam { private set; get; }
        public DateTime geboortedatum { private set; get; }
        public string email_adres { private set; get; }
    }
}
