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
        public void AddCustomer(Customer cus)
        {
            int index = DataSource.customerList.FindIndex(c => c.id == cus.id);
            if (index != -1)
            {
                throw new DuplicateIdException("Customer already exists\n");
            }
            else
            {
                DataSource.customerList.Add(cus);
            }
        }

        public Customer GetCustomer(string ID)
        {
            int index = DataSource.customerList.FindIndex(c => c.id == ID);
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


        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> list = new();
            DataSource.customerList.ForEach(c => list.Add(c));
            return (IEnumerable<Customer>)list;
        }

        public void UpdateCustomer(string customerId, string name, string phone)
        {
            int index = DataSource.customerList.FindIndex(c => c.id == customerId);
            if (index == -1)
            {
                throw new MissingIdException("No such station\n");
            }
            else
            {
                Customer tempCustomer = DataSource.customerList[index];
                if (name != "\n")
                    tempCustomer.name = name;
                if (phone != "\n")
                    tempCustomer.phone = phone;
                DataSource.customerList[index] = tempCustomer;
            }

        }

       
    }
}