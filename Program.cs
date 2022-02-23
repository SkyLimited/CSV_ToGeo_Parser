using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToGeoParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CSV to Geo Parser v 0.1\nUSAGE: csv_to_geo_parser.exe --type=[Poly|Line]  file1.csv file2.csv file3.csv ...");

            //parse args
            if(!args.Any(_=>_.StartsWith("--type")) ||                
                args.Count() <2 
              )
            {
                Console.WriteLine("All Arguments are mandatory.");
                return;                
            }

            var mode = args.FirstOrDefault(_ => _.StartsWith("--type=")).Split('=')[1];
            if(mode!="Poly" && mode != "Line")
            {
                Console.WriteLine($"mode {mode} is not supported.");
                return;
            }
        /*    var outx = args.FirstOrDefault(_ => _.StartsWith("--out=")).Split('=')[1];
            if(outx!="WKT" && outx!="GeoJSON")
            {
                Console.WriteLine($"out {outx} is not supported.");
                return;
            }*/

            var fileList = args.Where(_ => !_.StartsWith("--")).ToList();
            var gj = new GeoJSON();
            foreach (var file in fileList)
            {
                var strings = File.ReadAllLines(file);



                Feature feature = null;
                var lasts = "";
                foreach (var s in strings)
                {
                    if ((s != ";" && lasts == ";") || lasts == "")
                    {
                        feature = new Feature();
                        gj.features.Add(feature);
                    }
                    if (s != ";")
                    {
                        var crds = s.Replace(".", ",").Split(';');
                        feature.geometry.AddCoords(Double.Parse(crds[0]), Double.Parse(crds[1]));
                    }
                    lasts = s;
                }
            }
                string res = "";
                if (mode == "Poly")
                {
                    res = JsonConvert.SerializeObject(gj);

                }
                if (mode == "Line")
                {
                    res = "MULTILINESTRING (";
                    for (int i = 0; i < gj.features.Count; i++)
                    {
                        var f = gj.features[i];
                        if (i != 0)
                            res += ", ";
                        res += f.geometry.SerializeAsWKT();
                    }
                    res += ")";
                }
            File.WriteAllText("out.txt", res);
            
        }
    }
}
