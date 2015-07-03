using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Persistance
{
    public class LesPersistanceManager : PersistenceManager
    {
        static Dictionary<string, string> itemDictionary = new Dictionary<string, string>();

        // aantal wordt meegegeven in ReserverenController
        public List<Les> getKomendeLessen(Gebruiker gebruiker, int aantal)
        {   
            //Haalt lessen op die beginnen na beginTijd
            DateTime beginTijd = DateTime.Now;
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Les));
            criteria.Add(Restrictions.Gt("begintijd", beginTijd));
            criteria.Add(Restrictions.Eq("niet_tonen", 0));
            criteria.Add(Restrictions.Eq("vervallen", 0));
            IList<Les> lesList = criteria.List<Les>();
            // Verwijderd lessen waar de gebruiker al in zit
            var lessen = from les in lesList
                    where (from r in les.Reserveringen
                           where r.Deelnemer.sco_nummer == gebruiker.sco_nummer
                           select r).FirstOrDefault() == null
                           && les.Reserveringen.Count < les.max_aantal_deelnemers
                    select les;
            List<Les> tempList = lessen.ToList<Les>();
            // Sorteert lessen op begintijd
            tempList.Sort((x, y) =>
            {
                if (x.begintijd > y.begintijd) return 1;
                else if (x.begintijd == y.begintijd) return 0;
                else return -1;
            });


            List<Les> returnList = new List<Les>();
            int i = 0;
            foreach (Les l in tempList)
            {
                
                if(i >= aantal)
                    break;
                returnList.Add(l);
                i++;
            }
            return returnList;
        }
        public List<Les> getMijnInteresseLessen(Gebruiker gebruiker, int aantalLessen, int aantalDagen, bool isGeweest)
        {
            //berekenen datum
            DateTime datumTerug = DateTime.Today;
            datumTerug.AddDays(-aantalDagen);

            ISession session = OpenSession();
            
            //criteria voor sportcodes ophalen toevoegen
            ICriteria criteriaReservering = session.CreateCriteria(typeof(Reservering));
            criteriaReservering.Add(Expression.Eq("Deelnemer", gebruiker));
            if (isGeweest)
                criteriaReservering.Add(Expression.Eq("is_geweest", true));
            criteriaReservering.Add(Expression.Gt("datum_reservering", datumTerug));
            IList<Reservering> vorigeReserveringenList = criteriaReservering.List<Reservering>();
            List<Reservering> vorigeReservering = vorigeReserveringenList.ToList<Reservering>();
            List<string> sportCodes = new List<string>();
            //sportcodes in list zetten
            foreach (Reservering r in vorigeReservering)
                if (!sportCodes.Contains(r.Les.Sportaanbod.Sportcode)) { 
                    sportCodes.Add(r.Les.Sportaanbod.Sportcode);
                }
           
            
            string[] sportCodesArray = sportCodes.ToArray();
            ICriteria criteria = session.CreateCriteria(typeof(Les));
            criteria.Add(Restrictions.Eq("niet_tonen", 0));
            criteria.CreateAlias("Sportaanbod", "Sportaanbod");
            criteria.Add(Restrictions.In("Sportaanbod.Sportcode", sportCodesArray));
            IList<Les> lesList = criteria.List<Les>();
            var lessen = from les in lesList
                         where (from r in les.Reserveringen
                                where r.Deelnemer.sco_nummer == gebruiker.sco_nummer
                                select r).FirstOrDefault() == null
                         select les;

            //op datum sorteren
            List<Les> returnList = lessen.ToList<Les>();
            returnList.Sort((x, y) =>
            {
                if (x.begintijd > y.begintijd) return 1;
                else if (x.begintijd == y.begintijd) return 0;
                else return -1;
            });
            return returnList;

        }
        public List<Les> getAgenda(DateTime start, DateTime eind, Gebruiker gebruiker)
        {
            // haalt lessen op waar de gebruiker in zit
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Reservering));
            criteria.Add(Restrictions.Eq("Deelnemer", gebruiker));
            IList<Reservering> results = criteria.List<Reservering>();
            // Haalt lessen weg die niet in de agenda zitten
            var lessen = from s in results
                         let les = s.Les
                         where les.begintijd > start && les.begintijd < eind
                         select les;
            List<Les> returnList = lessen.ToList<Les>();
            // Sorteert lessen
            returnList.Sort((x,y) => {
                if (x.begintijd > y.begintijd) return 1;
                else if (x.begintijd == y.begintijd) return 0;
                else return -1;
            });
            return returnList;
        }
        public List<Les> GetWeekOverzicht(DateTime start, DateTime eind, String[] sportcodes)
        {
            // Haalt alle lessen op tussen start en eind met filter op sportcodes
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Les));
            criteria.Add(Restrictions.Gt("begintijd", start));
            criteria.Add(Restrictions.Lt("eindtijd", eind));
            criteria.Add(Restrictions.Eq("niet_tonen", 0));
            if (sportcodes != null) { 
                criteria.CreateAlias("Sportaanbod", "Sportaanbod");
                criteria.Add(Restrictions.In("Sportaanbod.Sportcode", sportcodes));
            }
            IList<Les> lesList = criteria.List<Les>();
            List<Les> returnList = lesList.ToList<Les>();
            returnList.Sort((x, y) =>
            {
                if (x.begintijd > y.begintijd) return 1;
                else if (x.begintijd == y.begintijd) return 0;
                else return -1; 
            });
            return returnList;
        }
        public Les getLes(int lesId)
        {
            //haalt info van de les op met les_no lesId
            ISession session = OpenSession();
                Les returnLes = null;
                ICriteria criteria = session.CreateCriteria(typeof(Les));
                criteria.Add(Restrictions.Eq("les_no", lesId));
                criteria.Add(Restrictions.Eq("niet_tonen", 0));
                IList<Les> matchingObjects = criteria.List<Les>();
                if (matchingObjects.Count() > 0)
                {
                    returnLes = matchingObjects.First();
                }
                return returnLes;
        }
        public List<Les> getRegistreerbareLessen(Gebruiker gebruiker)
        {
            // Haalt alle komende lessen en lessen van de huidige dat op van een sportdocent
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Les));
            criteria.Add(Restrictions.Eq("Sportdocent", gebruiker));
            criteria.Add(Restrictions.Gt("begintijd", DateTime.Now.Date));
            criteria.Add(Restrictions.Eq("niet_tonen", 0));
            List<Les> results = criteria.List<Les>().ToList<Les>();


            results.Sort((x, y) =>
            {
                if (x.begintijd > y.begintijd) return 1;
                else if (x.begintijd == y.begintijd) return 0;
                else return -1;
            });
            return results;
        }

        public bool ToggleAanwezigheid(int sco_nummer, int lesid)
        {
            //Veranderd aanwezigheid van true naar false of van false naar true
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Reservering));
            criteria.CreateAlias("Deelnemer", "Gebruiker");
            criteria.CreateAlias("Les", "Les");
            criteria.Add(Restrictions.Eq("Deelnemer.sco_nummer", sco_nummer));
            criteria.Add(Restrictions.Eq("Les.les_no", lesid));
            Reservering r = criteria.List<Reservering>().ToList<Reservering>().FirstOrDefault();
            System.Diagnostics.Debug.WriteLine(r.is_geweest);
            session.BeginTransaction();
            r.is_geweest = !r.is_geweest;
            session.SaveOrUpdate(r);
            session.Transaction.Commit();
            return r.is_geweest;
        }

        public void checkAanwezigheid(int id)
        {
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Reservering));
            criteria.CreateAlias("Les", "Les");
            criteria.Add(Restrictions.Eq("Les.les_no", id));
            List<Reservering> reserveringen = criteria.List<Reservering>().ToList<Reservering>();
            var eventjson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/events");
            JObject events = JObject.Parse(eventjson);
            JArray results = (JArray)events["results"];
            string nextUrl = events["next_url"].ToString();
            bool done = false;
            //Variabele voor begin controleren events
            DateTime detectlimit = DateTime.Now.AddHours(-1);
            while (nextUrl != "" && !done)
            {
                var tempJSON = new WebClient().DownloadString(nextUrl);
                JObject tempObject = JObject.Parse(tempJSON);
                nextUrl = tempObject["next_url"].ToString();
                foreach (JToken j in (JArray)tempObject["results"])
                {
                    DateTime detectTijd = DateTime.Parse(j["time_created"].ToString());

                    if (detectTijd < detectlimit)
                    {
                        done = true;
                        break;
                    }
                    results.Add(j);
                }
            }
            session.BeginTransaction();
            //todo: controle op locatie toevoegen
            foreach (Reservering r in reserveringen)
            {
                string hexCode = r.Deelnemer.sco_nummer.ToString("D24");
                foreach (JToken j in results)
                {
                    string sporterId = "invalid";
                    if (itemDictionary.ContainsKey(j["topic"]["arguments"]["item"].ToString()))
                    {
                        sporterId = itemDictionary[j["topic"]["arguments"]["item"].ToString()];
                    }
                    else
                    {
                        var itemJson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/items/" + j["topic"]["arguments"]["item"].ToString());
                        JObject items = JObject.Parse(itemJson);
                        sporterId = items["code_hex"].ToString();
                        itemDictionary.Add(j["topic"]["arguments"]["item"].ToString(), sporterId);
                    }
                    if (sporterId == hexCode)
                    {
                        r.is_geweest = true;
                        session.SaveOrUpdate(r);
                        break;
                    }
                }
            }

            foreach (JToken j in results)
            {
                string sporterId = "invalid";
                if (itemDictionary.ContainsKey(j["topic"]["arguments"]["item"].ToString()))
                {
                    sporterId = itemDictionary[j["topic"]["arguments"]["item"].ToString()];
                }
                else
                {
                    var itemJson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/items/" + j["topic"]["arguments"]["item"].ToString());
                    JObject items = JObject.Parse(itemJson);
                    sporterId = items["code_hex"].ToString();
                    itemDictionary.Add(j["topic"]["arguments"]["item"].ToString(), sporterId);
                }
                int sco_nummer;
                if (Int32.TryParse(sporterId, out sco_nummer))
                {
                    Reservering r = new Reservering();
                    r.datum_reservering = DateTime.Now;
                    AccountPersistenceManager accountManager = new AccountPersistenceManager();
                    Gebruiker g = accountManager.getGebruiker(sco_nummer);
                    // deze code moet weg gehaalt worden als de echte database wordt gebruikt
                    if(g == null){
                        g = accountManager.ZoekGebruiker("Oelen", DateTime.Parse("1986-10-08"));
                    }
                    r.Deelnemer = g;
                    r.is_geweest = true;
                    r.Les = getLes(id);
                    session.Save(r);
                }
            }


            session.Transaction.Commit();
        }

        public void Inschrijven(int sco_nummer, int lesid)
        {
            
            ISession session = OpenSession();
            ICriteria criteria1 = session.CreateCriteria(typeof(Gebruiker));
            criteria1.Add(Restrictions.Eq("sco_nummer", sco_nummer));
            Gebruiker g = criteria1.List<Gebruiker>().FirstOrDefault();

            ICriteria criteria2 = session.CreateCriteria(typeof(Les));
            criteria2.Add(Restrictions.Eq("les_no", lesid));
            Les l = criteria2.List<Les>().FirstOrDefault();

            Reservering r = new Reservering();
            r.datum_reservering = DateTime.Now;
            r.Deelnemer = g;
            r.is_geweest = true;
            r.Les = l;
            if (l.Reserveringen.Contains(r))
            {
                System.Diagnostics.Debug.WriteLine("les al gereserveerd");
            }
            else
            {
                session.BeginTransaction();
                session.Save(r);
                session.Transaction.Commit();
            }
            

        }

        public Reservering GetReservering(Gebruiker g, int id)
        {
            ISession session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Reservering));
            criteria.CreateAlias("Les", "Les");
            criteria.Add(Restrictions.Eq("Les.les_no", id));
            criteria.Add(Restrictions.Eq("Deelnemer", g));
            Reservering r = criteria.List<Reservering>().FirstOrDefault();
            return r;
        }
    }
}