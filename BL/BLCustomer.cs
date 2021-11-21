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
                throw new AddingException("Couldn't Add the Customer.\n,", ex);
            }
        }


        /// <summary>
        /// updates the name and phone of the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(string customerId, string name, string phone)
        {
            try
            {
                idal1.UpdateCustomer(customerId, name, phone);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Couldn't Update the Drone.\n,", ex);
            }
        }

        public Customer GetCustomer(string customerId)
        {
            try
            {
                Customer customer = new();
                idal1.GetCustomer(customerId).CopyPropertiestoIBL(customer);
                List<IDAL.DO.Parcel> ReceivedParcelListDal = (List<IDAL.DO.Parcel>)idal1.GetCustomerReceivedParcels(customerId);
                List<IDAL.DO.Parcel> SentParcelListDal = (List<IDAL.DO.Parcel>)idal1.GetCustomerSentParcels(customerId);
                ReceivedParcelListDal.ForEach(p => { customer.ReceivedParcels.Add(GetParcelAtCustomer(p.Id)); });// changes the list to a ParcelAtCustomerList
                SentParcelListDal.ForEach(p => { customer.SentParcels.Add(GetParcelAtCustomer(p.Id)); });
                return customer;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Customer.\n,", ex);
            }
        }

        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            List<CustomerToList> tempList = new();
            idal1.GetAllCustomers().CopyPropertyListtoIBLList(tempList);
            foreach (CustomerToList cus in tempList)
            {
                cus.NumPacksReceived = idal1.DeliveredParcels().Where(p => p.Receiver == cus.Id).Count();
                cus.NumPackSentDel = idal1.DeliveredParcels().Where(p => p.Sender == cus.Id).Count();
                cus.NumPackExp = idal1.UndeliveredParcels().Where(p => p.Receiver == cus.Id).Count();
                cus.NumPackSentDel = idal1.UndeliveredParcels().Where(p => p.Sender == cus.Id).Count();
            }
            return tempList;
        }
    }
}
