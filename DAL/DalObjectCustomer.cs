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
            int index = DataSource.customerList.FindIndex(c => c.Id == cus.Id);
            if (index != -1)
            {
                throw new DuplicateIdException("Customer already exists\n");
            }
            else
            {
                DataSource.customerList.Add(cus);
            }
        }

        /// <summary>
        /// returns the customer accorsing to the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Customer GetCustomer(string ID)
        {
            int index = DataSource.customerList.FindIndex(c => c.Id == ID);
            if (index == -1)
            {
                throw new DuplicateIdException("No such Customer exists in list\n");
            }
            else
            {
                return DataSource.customerList[index];
            }
            //return DataSource.customerList.Find(c => c.id == ID);
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
            int index = DataSource.customerList.FindIndex(c => c.Id == customerId);
            if (index == -1)
            {
                throw new MissingIdException("No such station\n");
            }
            else
            {
                Customer tempCustomer = DataSource.customerList[index];
                if (name != "\n")
                    tempCustomer.Name = name;
                if (phone !="\n")
                    tempCustomer.Phone = phone;
                DataSource.customerList[index] = tempCustomer;
            }

        }

       
    }
}