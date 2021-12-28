using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace BO
    {
        public class ParcelInTransfer
        {
            public int Id { get; set; }
            public Priorities Priority { get; set; }
            public bool Status { get; set; }// 1=not picked up or on it's way
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Receiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Location PickedUpFrom { get; set; }
            public Location DeliverdTo { get; set; }
            public double Distance { get; set; }

            public override string ToString()
            {
                return ""
                + $"ID is: {Id}\n"
                + $"Priority is: {Priority}\n"
                + $"Parcel weight: {Weight}\n"
                + $"stat is: {Status}\n"
                + $"Collecting location: {PickedUpFrom}\n"
                + $"Delivering location: {DeliverdTo}\n"
                + $"Delivery distance: {Distance}\n"
                + $"sender: {Sender}\n"
                + $"receiver: {Receiver}\n";
            }

        }
    }

