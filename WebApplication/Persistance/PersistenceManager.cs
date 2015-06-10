﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using WebApplication.Models;

namespace WebApplication.Persistance
{
    public class PersistenceManager
    {

        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddAssembly("WebApplication"); 
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public void Save<T>(T item)
        {
            using (ISession session = OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(item);
                    session.Transaction.Commit();
                }
            }
        }

    }
}