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
        public List<Les> getLastMinuteLessen(Gebruiker gebruiker)
        {
            ISession session = OpenSession();
            DateTime volgendeDag = DateTime.Now.Date.AddDays(1);
            ICriteria criteria = session.CreateCriteria(typeof(Les));
            criteria.Add(Restrictions.Gt("begintijd", DateTime.Now));
            criteria.Add(Restrictions.Lt("begintijd", volgendeDag));
            IList<Les> lesList = criteria.List<Les>();
            var x = from les in lesList
                    where (from r in les.Reserveringen
                           where r.Deelnemer.sco_nummer == gebruiker.sco_nummer
                           select r).FirstOrDefault() == null
                    select les;
            return x.ToList<Les>();
        }
        public List<Les> getAgenda(int skip, Gebruiker gebruiker)
        {
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Reservering));
            criteria.Add(Restrictions.Eq("Deelnemer", gebruiker));
            IList<Reservering> results = criteria.List<Reservering>();
            
            DateTime start = DateTime.Now.Date.AddDays(skip*5);
            DateTime eind = DateTime.Now.Date.AddDays((skip+1)*5);
            var lessen = from s in results
                         let les = s.Les
                         where les.begintijd > start && les.begintijd < eind
                         select les;
            List<Les> returnList = lessen.ToList<Les>();
            returnList.Sort((x,y) => {
                if (x.begintijd > y.begintijd) return 1;
                else if (x.begintijd == y.begintijd) return 0;
                else return -1;
            }
                );
            return returnList;
        }
        public Les getLes(int lesId)
        {
            ISession session = OpenSession();
                Les returnLes = null;
                ICriteria criteria = session.CreateCriteria(typeof(Les));
                criteria.Add(Restrictions.Eq("les_no", lesId));
                IList<Les> matchingObjects = criteria.List<Les>();
                if (matchingObjects.Count() > 0)
                {
                    returnLes = matchingObjects.First();
                }
                return returnLes;
        }
    }
}