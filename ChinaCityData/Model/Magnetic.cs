using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaCityData
{
    public class Magnetic
    {
        public string type { get; set; }
        public string name { get; set; }

        public Feature[] features { get; set; }
    }

    public class Feature { 
        public string type { get; set; }
        public Propertie properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Propertie { 
        public string NAME { get; set; }
        public string QUHUADAIMA { get; set; }
        public int Location { get; set; }
        public int Alignment { get; set; }

    }

    public class Geometry {
        public string type { get; set; }
        public double[] coordinates { get; set; }
    }

}
