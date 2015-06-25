using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using WebApplication.Models;

namespace WebApplication.Persistance
{
    public class AccountPersistenceManager : PersistenceManager
    {
        public Gebruiker Login(string username, string password)
        {
            Gebruiker returnGebruiker = null;
            using (ISession session = OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Gebruiker));
                criteria.Add(Expression.Eq("sco_nummer", Int32.Parse(username)));
                criteria.Add(Expression.Eq("wachtwoord", password));
                IList<Gebruiker> matchingObjects = criteria.List<Gebruiker>();
                if (matchingObjects.Count() > 0)
                {
                    returnGebruiker = matchingObjects.First();
                }
            }
            return returnGebruiker;
        }

        public Gebruiker ZoekGebruiker(string achternaam, DateTime geboortedatum)
        {
            using (ISession session = OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Gebruiker));
                criteria.Add(Restrictions.Eq("achternaam", achternaam));
                criteria.Add(Restrictions.Eq("geboortedatum", geboortedatum));
                Gebruiker g = criteria.List<Gebruiker>().FirstOrDefault();
                return g;
            }
        }
    }
}
