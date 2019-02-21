using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick1
{
    public class Translation
    {
        public string text { get; set; }
        public string to { get; set; }
    }

    public class RootObject
    {
        public List<Translation> translations { get; set; }
    }
}
