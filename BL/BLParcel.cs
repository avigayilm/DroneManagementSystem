using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DAL;
using IDAL;


namespace BL
{
    public partial class BL
    {

        public void AddParcel(Parcel newParcel)// do I have to check customer and receiver
        {
            try
            {
                if (newParcel.Id < 0)
                    throw new InvalidInputException("invalid Id input \n");
                if (newParcel.Priority != Priorities.Emergency && newParcel.Priority != Priorities.Fast && newParcel.Priority != Priorities.Normal)
                    throw new InvalidInputException("Invalid weightCategory \n");
                if (newParcel.Weight != WeightCategories.Heavy && newParcel.Weight != WeightCategories.Light && newParcel.Weight != WeightCategories.Medium)
                    throw new InvalidInputException("Invalid weightCategory \n");
                newParcel.Delivered = null;
                newParcel.Assigned = null;
                newParcel.PickedUp = null;
                newParcel.Dr = null;
                IDAL.DO.Parcel parcelTemp = new();
                newParcel.CopyPropertiestoIDAL(parcelTemp);
                idal1.AddParcel(parcelTemp);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new DuplicateIdException("The Parcel already exists.\n,", ex);
            }
        }
    }
}
