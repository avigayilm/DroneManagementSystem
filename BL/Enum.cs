using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public enum DroneStatuses
        {
            Available = 0, Maintenance, Delivery
        }
        public enum WeightCategories
        {
            Light = 0, Medium, Heavy
        }

        public enum Priorities
        {
            Normal = 0, Fast, Emergency
        }
        public enum ParcelStatuses
        {
            Created, Assigned, PickedUp, Delivered
        }

    }
}
