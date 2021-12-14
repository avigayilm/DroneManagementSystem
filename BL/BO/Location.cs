using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace BO
    {
        public class Location
        {
            public double Longitude { get; set; }//{ get { return Math.Round(Longitude, 2); } set => Longitude = value; }//we can't chage location, set it only when we initilize

         

            public string _Longitude {
                get => " " + $"{Bonus.DecimalToSexagesimal(Longitude, 'n')}";
              
                set { }
            }
     


            //public double Longitude// the Name property
            //{
            //    get { return Math.Round(Longitude, 2); }
            //    set => Longitude = value;
            //}
            public double Latitude { get; set; }
            public string _Latitude
            {
                get => " " + $", {Bonus.DecimalToSexagesimal(Latitude, 't')}\n";

                set { }
            }

            public override string ToString()
            {
                return _Longitude + _Latitude;
               
            }
        }
    }

