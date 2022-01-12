using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;
using DalApi;


namespace Dal
{
    internal sealed partial class DalObject
    {
        /// <summary>
        /// ading a customer to the cusotmer list
        /// </summary>
        /// <param name="cus"></param>
        public void AddCustomer(Customer cus)
        {
            if (DataSource.customerList.Exists(c => c.Id == cus.Id && !c.Deleted))
                throw new DuplicateIdException("Customer already exists\n");
            DataSource.customerList.Add(cus);
        }


        public Customer GetCustomer(string customerId)
        {
            int index = CheckExistingCustomer(customerId);
            return DataSource.customerList[index];
        }


        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            return DataSource.customerList.FindAll(c => predicate == null ? true : predicate(c) && !c.Deleted);
        }

        public void UpdateCustomer(string customerId, string name, string phone)
        {
            int index = CheckExistingCustomer(customerId);
            Customer tempCustomer = DataSource.customerList[index];
            if (name != null)
                tempCustomer.Name = name;
            if (phone != null)
                tempCustomer.PhoneNumber = phone;
            DataSource.customerList[index] = tempCustomer;
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
        /// returns the list of customers having received a parcel
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> CustomersDeliverdTo()
        {

            return from p in GetAllParcels(pa => pa.Delivered != null && !pa.Delete)
                   select (GetCustomer(p.ReceiverId));
        }

        public void AddLogin(Login log)
        {
            if (DataSource.loginList.Exists(c => c.UserName == log.UserName))
                throw new DuplicateIdException("User already exists\n");
            DataSource.loginList.Add(log);
        }

        public bool ValidateLogin(string user, string pass)
        {
            int index = DataSource.loginList.FindIndex(c => c.UserName == user);
            if (index == -1)
            {
                throw new LoginException("username doesnt exists");
            }
            else
            {
                if (DataSource.loginList[index].Password != pass)
                {
                    throw new LoginException("password is incorrect");
                }
                else
                    return DataSource.loginList[index].StaffOrUser;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(string customerId)
        {


            int cIndex = DataSource.customerList.FindIndex(c => c.Id == customerId); ;
            if (cIndex == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (DataSource.customerList[cIndex].Deleted)
            {
                throw new MissingIdException($"This Parcel:{ customerId } is deleted \n");
            }
            Customer tempCustomer = DataSource.customerList[cIndex];
            tempCustomer.Deleted = true;
            DataSource.customerList[cIndex] = tempCustomer;

        }

        public string getPic(string cuId)
        {

            int index = DataSource.loginList.FindIndex(l => l.UserName == cuId);
            if (index == -1)
            {
                throw new LoginException("username doesnt exists");
            }
            else
            {
                return DataSource.loginList[index].profileSource;
            }
            
        }
    }
    }