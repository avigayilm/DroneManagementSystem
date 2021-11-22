using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Receiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneInParcel? Dr { get; set; }
            public DateTime? Created { get; set; }
            public DateTime? Assigned { get; set; }
            public DateTime? PickedUp { get; set; }
            public DateTime? Delivered { get; set; }




            public override string ToString()
            {
                return " "
                + $"ID is {Id}, \n"
                + $"Priority is {Priority}, \n"
                + $"Parcel sender {Sender}, \n"
                + $"Parcel receiver {Receiver}, \n"
                + $"Parcel weight {Weight}, \n"
                + $"The parcel was requested at {Created},\n"
                + $"Drone carrying is {Dr}, \n"
                + $" The Parcel is scheduled for {Assigned},\n"
                + $"The Parcel was picked up at{PickedUp},\n"
                + $"The parcel was delivered at { Delivered},\n" ;
            }
        }
    }

}
