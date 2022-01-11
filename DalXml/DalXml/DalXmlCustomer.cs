using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DalApi;
using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer cus)
        {
            //List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            loadingToList(ref customers, CustomerXml);
            if (customers.Exists(c => c.Id == cus.Id && !c.Deleted))
                throw new DuplicateIdException("Customer already exists\n");
            customers.Add(cus);
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(string customerId)
        {
            int index = CheckExistingCustomer(customerId);
            return customers[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            //return DataSource.customerList.FindAll(c => predicate == null ? true : predicate(c));
            loadingToList(ref customers, CustomerXml);
            return customers.FindAll(c => predicate == null ? true : predicate(c) && !c.Deleted);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(string customerId, string name, string phone)
        {

            //List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            int index = CheckExistingCustomer(customerId);
            Customer tempCustomer = customers[index];
            if (name != null)
                tempCustomer.Name = name;
            if (phone != null)
                tempCustomer.PhoneNumber = phone;
            //int index = customers.FindIndex(c => c.Id == customerId);
            //if (index == -1)
            //{
            //    throw new MissingIdException("No such Customer exists\n");
            //}
            //Customer tempCustomer = customers[index];
            //if (name != null)
            //    tempCustomer.Name = name;
            //if (phone != null)
            //    tempCustomer.PhoneNumber = phone;
            customers[index] = tempCustomer;
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }



        [MethodImpl(MethodImplOptions.Synchronized)]
        public int CheckExistingCustomer(string customerId)
        {
            //List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            loadingToList(ref customers, CustomerXml);

            int index = customers.FindIndex(c => c.Id == customerId && !c.Deleted);
            if (index == -1)
            {
                throw new MissingIdException("No such Customer exists\n");
            }
            return index;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> CustomersDeliverdTo()
        {
            //List<Customer> temp = new List<Customer>();
            //foreach (Parcel p in GetAllParcels(pa => pa.Delivered != null && !pa.Delete))//for every delivered parcel add its customer to the list
            //    temp.Add(GetCustomer(p.ReceiverId));
            //return temp;
            return from p in GetAllParcels(pa => pa.Delivered != null && !pa.Delete)
                   select (GetCustomer(p.ReceiverId));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddLogin(Login log)
        {
           // List<Login> logins = XMLTools.LoadListFromXMLSerializer<Login>(LoginXml);
            loadingToList(ref logins, LoginXml);
            if (logins.Exists(c => c.UserName == log.UserName))
                throw new DuplicateIdException("User already exists\n");
            logins.Add(log);
            XMLTools.SaveListToXMLSerializer(logins, LoginXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool ValidateLogin(string user, string pass)
        {
            //List<Login> logins = XMLTools.LoadListFromXMLSerializer<Login>(LoginXml);
            loadingToList(ref logins, LoginXml);
            int index = logins.FindIndex(c => c.UserName == user);
            if (index == -1)
            {
                throw new LoginException("username doesnt exists");
            }
            else
            {
                if (logins[index].Password != pass)
                {
                    throw new LoginException("password is incorrect");
                }
                else
                    return logins[index].StaffOrUser;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(string customerId)
        {
            //parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            loadingToList(ref customers,CustomerXml);
            int cIndex = customers.FindIndex(c => c.Id == customerId);
            if (cIndex == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (customers[cIndex].Deleted)
            {
                throw new MissingIdException($"This Parcel:{ customerId } is deleted \n");
            }
            Customer tempCustomer = customers[cIndex];
            tempCustomer.Deleted = true;
            customers[cIndex] = tempCustomer;
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        public string getPic(string cuId)
        {
            loadingToList(ref logins, LoginXml);
            int index = logins.FindIndex(c => c.UserName == cuId);
            if (index == -1)
            {
                throw new LoginException("username doesnt exists");
            }
            else
            {
                return logins[index].profileSource;
            }
        }
      
    }
}
