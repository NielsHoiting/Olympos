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

                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (Les l in criteria.List<Les>().ToList<Les>())
                    {
                        l.begintijd = l.begintijd.AddDays(3);
                        l.eindtijd = l.eindtijd.AddDays(3);
                        l.dag = l.dag.AddDays(3);
                        session.SaveOrUpdate(l);
                    }

                    foreach (Sportaanbod s in sportaanbod)
                    {
                        s.EinddatumVerkoop = s.EinddatumVerkoop.AddDays(3);
                        s.Startdatum = s.Startdatum.AddDays(3);
                        s.StartdatumVerkoop = s.StartdatumVerkoop.AddDays(3);
                        s.TonenWebTot = s.TonenWebTot.AddDays(3);
                        s.TonenWebVan = s.TonenWebVan.AddDays(3);
                        session.SaveOrUpdate(s);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
