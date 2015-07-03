using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Persistance;
using WebApplication.Models;


namespace DatabaseControl
{
    class Program
    {
        static void Main(string[] args)
        {
            updateLessen();
            
                
        }
        public static void updateLessen()
        {
            using (ISession session = PersistenceManager.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Les));
                IList<Les> lessen = criteria.List<Les>();


                ICriteria criteria2 = session.CreateCriteria(typeof(Sportaanbod));
                IList<Sportaanbod> sportaanbod = criteria2.List<Sportaanbod>();
                int dagen = 7;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (Les l in criteria.List<Les>().ToList<Les>())
                    {
                        l.begintijd = l.begintijd.AddDays(dagen);
                        l.eindtijd = l.eindtijd.AddDays(dagen);
                        l.dag = l.dag.AddDays(dagen);
                        session.SaveOrUpdate(l);
                    }

                    foreach (Sportaanbod s in sportaanbod)
                    {
                        s.EinddatumVerkoop = s.EinddatumVerkoop.AddDays(dagen);
                        s.Startdatum = s.Startdatum.AddDays(dagen);
                        s.StartdatumVerkoop = s.StartdatumVerkoop.AddDays(dagen);
                        s.TonenWebTot = s.TonenWebTot.AddDays(dagen);
                        s.TonenWebVan = s.TonenWebVan.AddDays(dagen);
                        session.SaveOrUpdate(s);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
