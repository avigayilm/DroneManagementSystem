using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    // public record rec(string hudis); 

    public class Location
    {
        public double Longitude { get; set; }//{ get { return Math.Round(Longitude, 2); } set => Longitude = value; }//we can't chage location, set it only when we initilize



        public string _Longitude
        {
            get => " " + $"{Bonus.DecimalToSexagesimal(Longitude, 'n')}";

            set
            {
                double check;
                bool b = double.TryParse(value, out check);
                { }
                if (b)
                    Longitude = check;
            }

        }



        //public double Longitude// the Name property
        //{
        //    get { return Math.Round(Longitude, 2); }
        //    set => Longitude = value;
        //}
        public double Latitude { get; set; }
        public string _Latitude
        {
            get => " " + $", {Bonus.DecimalToSexagesimal(Latitude, 't')}";

            set 
            {
                double check;
                bool b = double.TryParse(value, out check);
                { }
                if (b)
                    Latitude = check;
            }
        }

        public override string ToString()
        {
            return _Longitude + _Latitude;

        }
    }
}

