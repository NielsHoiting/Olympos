using NHibernate;
using NHibernate.Criterion;
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

            if (l.Reserveringen.Contains(r) || l.niet_tonen == 1 || l.Lesstatus == LesStatus.Uitverkocht || l.Lesstatus == LesStatus.Voorbij)
            {
                return false;
            }

            ISession session = OpenSession();
            session.Save(r);
            return true;
        }

        public void AnnuleerReservering(Gebruiker g, int lesid)
        {
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Reservering));
            criteria.CreateAlias("Les", "Les");
            criteria.Add(Restrictions.Eq("Les.les_no", lesid));
            criteria.Add(Restrictions.Eq("Deelnemer", g));
            Reservering r = criteria.List<Reservering>().FirstOrDefault();
            if (r != null) { 
                session.BeginTransaction();
                session.Delete(r);
                session.Transaction.Commit();
            }
        }
    }
}