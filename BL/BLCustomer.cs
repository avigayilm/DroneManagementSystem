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
                if (!string.IsNullOrEmpty(newCustomer.Id))
                    throw new InvalidInputException("invalid Id input");
                if (!string.IsNullOrEmpty(newCustomer.Name))
                    throw new InvalidInputException("invalid name input");
                if (!string.IsNullOrEmpty(newCustomer.PhoneNumber))
                    throw new InvalidInputException("invalid phonenumber");
                IDAL.DO.Customer customer = new();
                newCustomer.CopyPropertiestoIDAL(customer);
                idal1.AddCustomer(customer);
            }
            catch (InvalidInputException ex)
            {
                throw new AddingException("Couldn't Add the Customer.\n,", ex);
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
                List<IDAL.DO.Parcel> ReceivedParcelListDal = idal1.GetAllParcels(p => p.Sender == customerId && p.Delivered != null).ToList();
                List<IDAL.DO.Parcel> SentParcelListDal = idal1.GetAllParcels(p => p.Sender == customerId && p.PickedUp != null).ToList();
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
                cus.NumPacksReceived = idal1.GetAllParcels(p => p.Delivered != null && p.Receiver == cus.Id).Count();
                cus.NumPackSentDel = idal1.GetAllParcels(p => p.Delivered != null && p.Sender == cus.Id).Count();
                cus.NumPackExp = idal1.GetAllParcels(p => p.Delivered == null && p.Receiver == cus.Id).Count();
                cus.NumPackSentDel = idal1.GetAllParcels(p => p.Delivered == null && p.Sender == cus.Id).Count();
            }
            return tempList;
        }
    }
}
