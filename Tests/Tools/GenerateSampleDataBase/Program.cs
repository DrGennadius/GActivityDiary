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
            db.BeginTransaction();

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
                    db.Commit();
                    db.ResetSession();
                    db.BeginTransaction();
                }
            }

            db.Commit();
            db.Dispose();

            Console.WriteLine("Generated!");
        }
    }
}
