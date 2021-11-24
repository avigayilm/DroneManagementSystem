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
      
        public void AddCustomer(Customer newCustomer)
        {
            try
            {
                if (string.IsNullOrEmpty(newCustomer.Id))
                    throw new InvalidInputException("invalid Id input");
                if (string.IsNullOrEmpty(newCustomer.Name))
                    throw new InvalidInputException("invalid name input");
                if (string.IsNullOrEmpty(newCustomer.PhoneNumber))
                    throw new InvalidInputException("invalid phonenumber");
                if (newCustomer.Loc.Latitude <= -90.0 || newCustomer.Loc.Latitude >= 90.0)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
                if (newCustomer.Loc.Longitude <= -180.0 || newCustomer.Loc.Longitude >= 180.0)// out of range of latitude
                    throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
                IDAL.DO.Customer customer = new();
                object obj1 = customer;
                newCustomer.CopyProperties(obj1);
                customer = (IDAL.DO.Customer)obj1;
                customer.Longitude = newCustomer.Loc.Longitude;
                customer.Latitude = newCustomer.Loc.Latitude;
                idal1.AddCustomer(customer);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't Add the Customer.", ex);
            }
        }



        public void UpdateCustomer(string customerId, string name, string phone)
        {
            try
            {
                idal1.UpdateCustomer(customerId, name, phone);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Couldn't Update the Customer.\n,", ex);
            }
        }

        public Customer GetCustomer(string customerId)
        {
            try
            {
                Customer customer = new();
                IDAL.DO.Customer customerDal = idal1.GetCustomer(customerId);
                customerDal.CopyProperties(customer);
                customer.Loc = new() { Longitude = customerDal.Longitude, Latitude = customerDal.Latitude };
                List<IDAL.DO.Parcel> ReceivedParcelListDal = idal1.GetAllParcels(p => p.SenderId == customerId && p.Delivered != null).ToList();
                List<IDAL.DO.Parcel> SentParcelListDal = idal1.GetAllParcels(p => p.SenderId == customerId && p.PickedUp != null).ToList();
                ReceivedParcelListDal.ForEach(p => { customer.ReceivedParcels.Add(GetParcelAtCustomer(p.Id)); });// changes the list to a ParcelAtCustomerList
                SentParcelListDal.ForEach(p => { customer.SentParcels.Add(GetParcelAtCustomer(p.Id)); });
                return customer;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Customer.", ex);
            }
        }

        public CustomerInParcel GetCustomerInParcel(string customerId)
        {
            CustomerInParcel customerInParcelTemp = new();
            IDAL.DO.Customer customerTemp = idal1.GetCustomer(customerId);
            customerInParcelTemp = new() { Id = customerTemp.Id, Name = customerTemp.Name };
            return customerInParcelTemp;
        }
        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            List<CustomerToList> tempList = new();
            idal1.GetAllCustomers().CopyPropertyListtoIBLList(tempList);
            foreach (CustomerToList cus in tempList)
            {
                cus.NumPacksReceived = idal1.GetAllParcels(p => p.Delivered != null && p.ReceiverId == cus.Id).Count();
                cus.NumPackSentDel = idal1.GetAllParcels(p => p.Delivered != null && p.SenderId == cus.Id).Count();
                cus.NumPackExp = idal1.GetAllParcels(p => p.Delivered == null && p.ReceiverId == cus.Id).Count();
                cus.NumPackSentDel = idal1.GetAllParcels(p => p.Delivered == null && p.SenderId == cus.Id).Count();
            }
            return tempList;
        }
    }
}
