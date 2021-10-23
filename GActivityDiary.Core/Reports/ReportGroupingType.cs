using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Reports
{
    /// <summary>
    /// Report grouping type.
    /// </summary>
    public enum ReportGroupingType
    {
        [Description("Nothing")]
        Nothing = 0,

        [Description("Day")]
        Day = 2
    }
}
