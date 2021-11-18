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
        /// <summary>
        /// adding a customer in the list of the datalayer
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(Customer newCustomer)
        {
            try
            {
                if (newCustomer.Id == "\n")
                    throw new InvalidInputException("invalid Id input");
                if (newCustomer.Loc.Latitude <= -90 || newCustomer.Loc.Latitude >= 90)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
                if (newCustomer.Loc.Longitude <= -180 || newCustomer.Loc.Longitude >= 180)// out of range of latitude
                    throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
                if (newCustomer.PhoneNumber == "\n")
                    throw new InvalidInputException("invalid phonenumber");
                IDAL.DO.Customer customer = new();
                newCustomer.CopyPropertiestoIDAL(customer);
                idal1.AddCustomer(customer);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new DuplicateIdException("The Customer already exists.\n,", ex);
            }
        }


        /// <summary>
        /// updates the name and phone of the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(int customerId, string name, string phone)
        {
            try
            {
                UpdateCustomer(customerId, name, phone);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new MissingIdException("Invalid ID.\n,", ex);
            }
        }
    }
}
