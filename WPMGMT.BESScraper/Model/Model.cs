using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPMGMT.BESScraper.Model
{
    public abstract class Model
    {
        public string UniqueCondition { get; set; }     // Defines the WHERE clause that makes this record unique
    }
}
