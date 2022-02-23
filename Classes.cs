using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToGeoParser
{
     public class GeoJSON
    {
        public string type { get; set; }
        public List<Feature> features { get; private set; }
        public CRS crs { get; private set; }

        public GeoJSON()
        {
            type = "FeatureCollection";
            features = new List<Feature>();
            crs = new CRS();
        }

    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; private set; }

        public Feature()
        {
            type = "Feature";
            geometry = new Geometry();
        }
    }

    public class CRS
    {
        public string type { get; set; }
        public Properties properties { get; private set; }

        public CRS()
        {
            type = "name";
            properties = new Properties();
        }
    }

    public class Properties
    {
        public string name = "EPSG:3857";

    }

    public class Geometry
    {
        public string type { get; set; }
        public List<List<double[]>> coordinates { get; private set; }

        public void AddCoords(double x, double y)
        {
            var t = new double[2];
            t[0] = x;
            t[1] = y;
            realCords.Add(t);
            
        }


        public string SerializeAsWKT()
        {
            var res = "(";
            for(int i=0; i< realCords.Count; i++)
            {
                if (i != 0)
                    res += ", ";
                res += realCords[i][0].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture) + " " + realCords[i][1].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                
            }
            res += ")";
            return res;

        }

        private List<double[]> realCords;


        public Geometry()
        {
            type = "Polygon";
            coordinates = new List<List<double[]>>();
            coordinates.Add(new List<double[]>());
            realCords = coordinates.First();
        }

    }

    
}
