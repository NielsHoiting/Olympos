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
    public class GebruikersPersistenceManager : PersistenceManager
    {
        public Boolean Login(string username, string password)
        {
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Gebruiker));
            criteria.Add(Expression.Eq("sco_nummer", username));
            criteria.Add(Expression.Eq("wachtwoord", password));
            IList<Gebruiker> matchingObjects = criteria.List<Gebruiker>();
            return matchingObjects.Count() > 0;
        }
    }
}