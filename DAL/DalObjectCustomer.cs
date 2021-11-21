using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;


namespace DAL
{
    public partial class DalObject
    {
        /// <summary>
        /// ading a customer to the cusotmer list
        /// </summary>
        /// <param name="cus"></param>
        public void AddCustomer(Customer cus)
        {
            CheckDuplicateCustomer(cus.Id);
            DataSource.customerList.Add(cus);
        }

        /// <summary>
        /// returns the customer accorsing to the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Customer GetCustomer(string customerId)
        {
            int index = CheckExistingCustomer(customerId);
            return DataSource.customerList[index];
        }

        /// <summary>
        /// returns the list of customers as Ienumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> list = new();
            DataSource.customerList.ForEach(c => list.Add(c));
            return (IEnumerable<Customer>)list;
        }

        /// <summary>
        /// updates the customer, gets a customerId and changes the name and phone
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(string customerId, string name, string phone)
        {
            int index = CheckExistingCustomer(customerId);
            Customer tempCustomer = DataSource.customerList[index];
            if (name != "\n")
                 tempCustomer.Name = name;
            if (phone !="\n")
                 tempCustomer.Phone = phone;
            DataSource.customerList[index] = tempCustomer;
        }
        /// <summary>
        /// makes a list of all the parcels a certain customer sent
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomerSentParcels(string customerId)
        {
            int index = CheckExistingCustomer(customerId);
            List<Parcel> list = new();
            DataSource.parcelList.ForEach(p => { if (p.Sender == customerId && p.PickedUp != null) list.Add(p); });
            return (IEnumerable<Customer>)list;
        }

        /// <summary>
        /// makes a list with all the parcels a customer received
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomerReceivedParcels(string customerId)
        {
            int index = CheckExistingCustomer(customerId);
            List<Parcel> list = new();
            DataSource.parcelList.ForEach(p => { if (p.Sender == customerId && p.Delivered != null) list.Add(p); });
            return (IEnumerable<Customer>)list;
        }
        /// <summary>
        /// checks if a customer exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int CheckExistingCustomer(string customerId)
        {
            int index = DataSource.customerList.FindIndex(c => c.Id == customerId);
            if (index == -1)
            {
                throw new MissingIdException("No such Customer exists\n");
            }
            return index;
        }

        /// <summary>
        /// checks if a customer already exists, if it does it throws a duplicateIdException
        /// </summary>
        /// <param name="customerId"></param>
        public void CheckDuplicateCustomer(string customerId)
        {
            if (DataSource.customerList.Exists(c => c.Id == customerId))
            {
                throw new DuplicateIdException("Customer already exists\n");
            }
        }
        /// <summary>
        /// returns the list of customers having received a parcel
        /// </summary>
        /// <returns></returns>
        public List<Customer> CustomersDeliverdTo()
        {
            List<Customer> temp = new();
            foreach (Parcel p in DeliveredParcels())
                temp.Add(GetCustomer(p.Receiver));
            return temp;
        }
    }
}