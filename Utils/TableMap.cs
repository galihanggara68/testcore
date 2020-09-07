using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Extractor_API.Utils
{
    class TableMap
    {
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public TableMap()
        {
            this.Headers = new List<string>();
            this.Rows = new List<List<string>>();
        }
    }
}
