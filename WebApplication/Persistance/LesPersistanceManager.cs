using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Persistance
{
    public class LesPersistanceManager : PersistenceManager
    {
        public List<Les> getAgenda(int skip, Gebruiker gebruiker)
        {
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Reservering));
            criteria.Add(Restrictions.Eq("Deelnemer", gebruiker));
            IList<Reservering> results = criteria.List<Reservering>();
            
            DateTime start = DateTime.Now.AddDays(skip*5);
            DateTime eind = DateTime.Now.AddDays((skip+1)*5);
            var lessen = from s in results
                         let les = s.Les
                         where les.begintijd > start && les.begintijd < eind
                         select les;
            return lessen.ToList<Les>();
        }
    }
}