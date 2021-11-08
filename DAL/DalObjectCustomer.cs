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
                throw new CustomerException("Id exists already\n");
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
                throw new CustomerException("Id not found\n");
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
    }
}