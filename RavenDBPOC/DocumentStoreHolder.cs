using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using System;
using System.Threading;

namespace RavenDBPOC
{
    public class DocumentStoreHolder
    {
        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store => store.Value;

        private static IDocumentStore CreateStore()
        {
            var store = new DocumentStore()
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "sample"
            }.Initialize();

            IndexCreation.CreateIndexes(typeof(DocumentStoreHolder).Assembly, store);

            return store;
        }

        public static T Run<T>(Func<IDocumentSession, T> func)
        {
            using (IDocumentSession session = Store.OpenSession())
            {
                return func(session);
            }
        }

        public static T RunAndSave<T>(Func<IDocumentSession, T> func, bool waitForIndexCompletion = true)
        {
            using (IDocumentSession session = Store.OpenSession())
            {
                var result = func(session);
                //session.Advanced.WaitForIndexesAfterSaveChanges();
                session.SaveChanges();
                if (waitForIndexCompletion)
                {
                    WaitForIndexCompletion();
                }
                return result;
            }
        }

        public static void RunAndSave(Action<IDocumentSession> action, bool waitForIndexCompletion = true)
        {
            RunAndSave(session =>
            {
                action(session);
                return true;
            }, waitForIndexCompletion);
        }

        public static void WaitForIndexCompletion()
        {
            var stats = Store.Maintenance.Send(new GetStatisticsOperation());

            while (stats.StaleIndexes.Length > 0)
            {
                Thread.Sleep(10);
                stats = Store.Maintenance.Send(new GetStatisticsOperation());
            }
        }
    }
}
