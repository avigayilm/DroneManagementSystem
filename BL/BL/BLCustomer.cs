using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
using DO;
using DalApi;

namespace BL
{
    public partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(BO.Customer newCustomer)
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
                    throw new InvalidInputException("The Longitude is not in a existing range(betweeen -180 and 180)\n");
                DO.Customer customer = new DO.Customer();
                object obj1 = customer;
                newCustomer.CopyProperties(obj1);
                customer = (DO.Customer)obj1;
                customer.Longitude = newCustomer.Loc.Longitude;
                customer.Latitude = newCustomer.Loc.Latitude;
                lock (idal1)
                {
                    idal1.AddCustomer(customer);
                }
                
            }
            catch (DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't Add the Customer.", ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Register(BO.Customer cus, string user, string password, string imageSrc, string emailAdd)
        {
            try
            {
                DO.Login login = new() { UserName = user, Password = password, profileSource = imageSrc, emailAddress = emailAdd };
                lock (idal1)
                {
                    idal1.AddLogin(login);
                }
               
                try
                {
                    AddCustomer(cus);
                }
                catch (DO.DuplicateIdException ex) 
                // in this case it doesnt matter if customer exists already because we can have a customer with no login details
                {       
                }
            }
            catch (DO.DuplicateIdException ex)
            {
                throw new AddingException("User already exists", ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Login(string user, string pass)
        {
            try
            {
                lock (idal1)
                {
                    bool userType = idal1.ValidateLogin(user, pass);
                    return userType;
                }
                
            }
            catch (DO.LoginException ex)
            {
                throw new LoginBLException(ex.Message);
            }

        }
        public string getPic(string cuId)
        {
            try
            {
                return idal1.getPic(cuId);
            }
            catch(LoginException ex)
            {
                throw new RetrievalException(ex.Message);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(string customerId, string name, string phone)
        {
            try
            {
                lock (this)
                {
                    idal1.UpdateCustomer(customerId, name, phone);
                }
                
            }
            catch (DO.MissingIdException ex)
            {
                throw new UpdateIssueException("couldn't Update the Customer.\n,");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Customer GetCustomer(string customerId)
        {
            try
            {
                BO.Customer customer = new BO.Customer();
                lock (this)
                {
                    DO.Customer customerDal = idal1.GetCustomer(customerId);
                    customerDal.CopyProperties(customer);
                    customer.Loc = new Location() { Longitude = customerDal.Longitude, Latitude = customerDal.Latitude };
                }
               
                customer.SentParcels = GetParcelsAtCustomer(customerId, true);
                customer.ReceivedParcels = GetParcelsAtCustomer(customerId, false);

                //select new DroneInCharge
                //{
                //    Id = item.DroneId,
                //    Battery = temp != default ? temp.Battery : throw new RetrievalException("the Id number doesnt exist\n")
                //};
                //List < DO.Parcel > ReceivedParcelListDal = idal1.GetAllParcels(p => p.SenderId == customerId && p.Delivered != null).ToList();
                //List<DO.Parcel> SentParcelListDal = idal1.GetAllParcels(p => p.SenderId == customerId && p.PickedUpTime != null).ToList();
                //ReceivedParcelListDal.ForEach(p => { customer.ReceivedParcels.Add(GetParcelAtCustomer(p.Id)); });// changes the list to a ParcelAtCustomerList
                //SentParcelListDal.ForEach(p => { customer.SentParcels.Add(GetParcelAtCustomer(p.Id)); });
                return customer;
            }
            catch (DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Customer.", ex);
            }
        }

        private IEnumerable<ParcelAtCustomer> GetParcelsAtCustomer(string customerId, bool flag)
        {
            //_ = GetParcelAtCustomer(1006);
            return from item in GetAllParcels(p => flag ? (p.SenderId == customerId && p.PickedUp != null) : (p.ReceiverId == customerId && p.Delivered != null))
                   select GetParcelAtCustomer(item.Id, flag);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerInParcel GetCustomerInParcel(string customerId)
        {
            CustomerInParcel customerInParcelTemp = new CustomerInParcel();
            lock (idal1)
            {
                DO.Customer customerTemp = idal1.GetCustomer(customerId);
                customerInParcelTemp = new CustomerInParcel() { Id = customerTemp.Id, Name = customerTemp.Name };
            }
            return customerInParcelTemp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetAllCustomers(Predicate<DO.Customer> predicate = null)
        {
            lock (idal1)
            {

            }
            List<CustomerToList> tempList = new List<CustomerToList>();
            idal1.GetAllCustomers(predicate).CopyPropertyListtoIBLList(tempList);
            return from cus in tempList
                   select new CustomerToList()
                   {
                       Id = cus.Id,
                       Name = cus.Name,
                       PhoneNumber = cus.PhoneNumber,
                       NumPacksReceived = idal1.GetAllParcels(p => p.Delivered != null && p.ReceiverId == cus.Id).Count(),
                       NumPackSentDel = idal1.GetAllParcels(p => p.Delivered != null && p.SenderId == cus.Id).Count(),
                       NumPackExp = idal1.GetAllParcels(p => p.Delivered == null && p.ReceiverId == cus.Id).Count(),
                       NumPackSentUnDel = idal1.GetAllParcels(p => p.Delivered == null && p.SenderId == cus.Id).Count()
                   };
                      // (cus=>( cus.NumPacksReceived = idal1.GetAllParcels(p => p.Delivered != null && p.ReceiverId == cus.Id).Count(), cus.NumPackSentDel = idal1.GetAllParcels(p => p.Delivered != null && p.SenderId == cus.Id).Count(), cus.NumPackExp = idal1.GetAllParcels(p => p.Delivered == null && p.ReceiverId == cus.Id).Count(),
           // cus.NumPackSentUnDel = idal1.GetAllParcels(p => p.Delivered == null && p.SenderId == cus.Id).Count()));
            //foreach (CustomerToList cus in tempList)
            //{
            //    cus.NumPacksReceived = idal1.GetAllParcels(p => p.Delivered != null && p.ReceiverId == cus.Id).Count();
            //    cus.NumPackSentDel = idal1.GetAllParcels(p => p.Delivered != null && p.SenderId == cus.Id).Count();
            //    cus.NumPackExp = idal1.GetAllParcels(p => p.Delivered == null && p.ReceiverId == cus.Id).Count();
            //    cus.NumPackSentUnDel = idal1.GetAllParcels(p => p.Delivered == null && p.SenderId == cus.Id).Count();
            //}
            //return tempList;
        }
        public bool CustomerExists(string id)
        {
            try
            {
                idal1.CheckExistingCustomer(id);
                return true;
            }
            catch (MissingIdException ex)
            {
                throw new AddingException(ex.Message);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(string customerId)
        {
            try
            {
                idal1.DeleteCustomer(customerId);
            }
            catch (MissingIdException ex)
            {
                throw new RetrievalException(ex.Message);
            }
        }

    }
}
