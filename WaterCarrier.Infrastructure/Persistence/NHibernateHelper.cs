using NHibernate;
using NHibernate.Cfg;
using System;
using System.Reflection;

namespace WaterCarrier.Infrastructure.Persistence
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    InitializeSessionFactory();
                }
                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
            try
            {
                var configuration = new Configuration();
                configuration.Configure(); // Ищет hibernate.cfg.xml по умолчанию
                
                // Находим и добавляем все наши файлы маппинга .hbm.xml
                configuration.AddAssembly(Assembly.GetExecutingAssembly());

                _sessionFactory = configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                // Здесь в реальном приложении должно быть логирование ошибки
                throw new Exception("Не удалось инициализировать NHibernate.", ex);
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
} 