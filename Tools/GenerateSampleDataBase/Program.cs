using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using NHibernate;
using System;
using System.Threading.Tasks;

namespace GenerateSampleDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContext db = new();
            var transaction = db.BeginTransaction();

            DateTime dateTime = DateTime.Now.AddMonths(-1);

            for (int i = 1; i <= 1000000; i++)
            {
                db.Activities.Save(new Activity()
                {
                    Name = $"Test Activity {i}",
                    CreatedAt = dateTime
                });
                dateTime = dateTime.AddMinutes(1);
                if (i % 100000 == 0)
                {
                    Commit(transaction).Wait();
                    db.ResetSession();
                    transaction = db.BeginTransaction();
                }
            }

            transaction.Commit();
            db.Dispose();

            Console.WriteLine("Generated!");
        }

        static async Task Commit(ITransaction transaction)
        {
            await transaction.CommitAsync();
        }
    }
}
