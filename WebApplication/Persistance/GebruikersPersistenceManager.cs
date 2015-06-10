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
        public Gebruiker Login(string username, string password)
        {
            Gebruiker returnGebruiker = null;
            using (ISession session = OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Gebruiker));
                criteria.Add(Expression.Eq("sco_nummer", username));
                criteria.Add(Expression.Eq("wachtwoord", password));
                System.Diagnostics.Debug.WriteLine("iemand logt in met sco_nummer" + username + " en wachtwoord " + password);
                IList<Gebruiker> matchingObjects = criteria.List<Gebruiker>();
                if (matchingObjects.Count() > 0)
                {
                    returnGebruiker = matchingObjects.First();
                }
            }
            return returnGebruiker;
        }
    }
}