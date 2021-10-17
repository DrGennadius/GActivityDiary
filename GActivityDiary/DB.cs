using GActivityDiary.Core.DataBase;

namespace GActivityDiary
{
    public class DB
    {
        // TODO: May be use DI?
        // 1. https://www.reactiveui.net/docs/handbook/dependency-inversion/#dependency-injection
        // 2. https://github.com/reactiveui/splat

        private static readonly DbContext _instance = new();

        public static DbContext Instance => _instance;

        private DB() { }
    }
}
