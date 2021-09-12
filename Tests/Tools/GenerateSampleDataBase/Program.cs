using GActiveDiary.Tests.Common.DataBaseUtils;
using System;

namespace GenerateSampleDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = DataBaseGenerator.Generate();
            dbContext.Dispose();

            Console.WriteLine("Generated!");
        }
    }
}
