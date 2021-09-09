using GActivityDiary.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary
{
    public class DB
    {
        private static readonly DbContext _instance = new();

        public static DbContext Instance => _instance;

        private DB() { }
    }
}
