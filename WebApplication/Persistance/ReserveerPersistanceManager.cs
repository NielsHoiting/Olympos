using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Persistance
{
    public class ReserveerPersistanceManager : PersistenceManager
    {
        public ReserveerPersistanceManager()
        {

        }

        public bool ReserveerLes(Gebruiker g, Les l)
        {
            Reservering r = new Reservering();
            r.datum_reservering = DateTime.Now;
            r.Deelnemer = g;
            r.is_geweest = false;
            r.Les = l;

            if (l.Reserveringen.Contains(r))
            {
                return false;
            }

            ISession session = OpenSession();
            session.Save(r);
            return true;
        }
    }
}