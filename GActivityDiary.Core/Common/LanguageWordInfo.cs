using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Common
{
    public class LanguageWordInfo
    {
        /// <summary>
        /// Ex.: 1 hour, 1 час.
        /// </summary>
        public string Singular { get; set; }

        /// <summary>
        /// Ex.: 2 hours, 2 часа.
        /// </summary>
        public string Plural { get; set; }

        /// <summary>
        /// Ex.: 1 h, 1 ч.
        /// </summary>
        public string Short { get; set; }

        /// <summary>
        /// Get Singular or Plural by numeric value.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        internal string GetValue(int number)
        {
            return number == 1
                ? Singular
                : Plural;
        }
    }
}
