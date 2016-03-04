using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using System.Text.RegularExpressions;
using NHibernate.Mapping.Attributes;
using System.IO;
using System.Threading;

namespace hkkf.Common
{

    public sealed class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;
        private static Configuration configuration;

        private static List<Assembly> assemblies = new List<Assembly>();


        static NHibernateHelper()
        {
            //try
            //{

            //sessionFactory = Configuration.BuildSessionFactory();
            //}
            //catch(Exception ex)
            //{
            //    MemoryStream stream = HbmSerializer.Default.Serialize(assembly);
            //    TextReader reader = new StreamReader(stream);
            //    string xml = reader.ReadToEnd();
            //    int row = 1;
            //    xml = Regex.Replace(xml, "^.*$", m =>  string.Format("{0:d3}\t{1}", row++, m.Value) , RegexOptions.Multiline);
            //    throw new Exception(ex.Message + "\r\n" + xml);
            //}
        }

        /// <summary>
        /// 添加程序集，重新生成 SessionFactory
        /// </summary>
        /// <param name="assembly"></param>
        /// <remarks>多加次添加同一程序集不会报错，只有第一次有效</remarks>
        public static void AddAssemblyAndBuilderSessionFactory(Assembly assembly)
        {
            if (assemblies.Contains(assembly)) return;
            assemblies.Add(assembly);
            
            try
            {
                var configuration = new Configuration()
                    .Configure()
                    .AddAssembly(assembly)
                    .AddInputStream(HbmSerializer.Default.Serialize(assembly))
                    .Configure(GetConfigurationFilePath("hibernate.cache.xml"));
                NHibernateHelper.configuration = configuration;
                sessionFactory = configuration.BuildSessionFactory();
            }
            catch(Exception ex)
            {
                MemoryStream s = HbmSerializer.Default.Serialize(assembly);
                TextReader reader = new StreamReader(s);
                string xml = reader.ReadToEnd();
                int row = 1;
                xml = Regex.Replace(xml, "^.*$", m => string.Format("{0:d3}\t{1}", row++, m.Value), RegexOptions.Multiline);
               throw new Exception(ex.Message + "\r\n" + xml);
            }

        }

        /// <summary>
        /// Obtains the current session.
        /// </summary>
        /// <returns>The current session.</returns>
        /// <remarks>
        /// The definition of what exactly "current" means is controlled by the NHibernate.Context.ICurrentSessionContext implementation configured for use.
        /// </remarks>
        /// <exception cref="NHibernate.HibernateException">Indicates an issue locating a suitable current session.</exception>
        public static ISession GetCurrentSession()
        {
            while (sessionFactory == null) Thread.Sleep(50);
            ISession session = sessionFactory.GetCurrentSession();
            Console.WriteLine("---" + session.GetHashCode());
            return session;
        }

        /// <summary>
        /// Create a database connection and open a ISession on it
        /// </summary>
        /// <returns></returns>
        public static ISession OpenSession()
        {
            while (sessionFactory == null) Thread.Sleep(50);
            return sessionFactory.OpenSession();
        }

        public static void ToDB()
        {
            SchemaExport export = new SchemaExport(configuration);
            export.Execute(true, true, false);
        }

        public static PersistentClass GetPersistentClass(Type type)
        {
            Configuration cfg = NHibernateHelper.configuration;
            return cfg.ClassMappings.FirstOrDefault(c => c.MappedClass == type);
        }

        public static void WaitUntilSessionFactoryOK()
        {
            while(sessionFactory == null) Thread.Sleep(100);
        }

        static string GetConfigurationFilePath(string fileName) {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string text = AppDomain.CurrentDomain.RelativeSearchPath ?? string.Empty;
            string path = text.Split(new char[] { ';' }).First<string>();
            string path2 = Path.Combine(baseDirectory, path);
            return Path.Combine(path2, fileName);
        }
    }
}
