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
            if (DataSource.customerList.Exists(c => c.Id == cus.Id))
                throw new DuplicateIdException("Customer already exists\n");
            DataSource.customerList.Add(cus);
        }

       
        public Customer GetCustomer(string customerId)
        {
            int index = CheckExistingCustomer(customerId);
            return DataSource.customerList[index];
        }

        
        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null )
        {
           return DataSource.customerList.FindAll(c => predicate == null?true: predicate(c));
        }

        public void UpdateCustomer(string customerId, string name, string phone)
        {
            int index = CheckExistingCustomer(customerId);
            Customer tempCustomer = DataSource.customerList[index];
            if (name != null)
                 tempCustomer.Name = name;
            if (phone !=null)
                 tempCustomer.PhoneNumber = phone;
            DataSource.customerList[index] = tempCustomer;
        }
    

        
        /// <summary>
        /// checks if a customer exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        internal int CheckExistingCustomer(string customerId)
        {
            int index = DataSource.customerList.FindIndex(c => c.Id == customerId);
            if (index == -1)
            {
                throw new MissingIdException("No such Customer exists\n");
            }
            return index;
        }

       
        /// <summary>
        /// returns the list of customers having received a parcel
        /// </summary>
        /// <returns></returns>
        public List<Customer> CustomersDeliverdTo()
        {
            List<Customer> temp = new();
            foreach (Parcel p in GetAllParcels(pa => pa.Delivered != null))//for every delivered parcel add its customer to the list
                temp.Add(GetCustomer(p.ReceiverId));
            return temp;
        }
    }
}