using System;
using System.Collections.Generic;
using System.Linq;
using CourierTrackingSystem.Models;

namespace CourierTrackingSystem.Data
{
    public class DataStore
    {
       private List<Customer> customers = new List<Customer>();
        public void AddCustomer(Customer customer)
        {
            if(customers.Any(c =>c.CustomerId == customer.CustomerId))
            {
                throw new Exception("Customer id Alerady Exists");

            }
            customers.Add(customer);

        }
        public Customer? GetCustomerById (string customerId)
        {
            return customers.FirstOrDefault( c => c.CustomerId == customerId);
        }
        public List<Customer>GetAllCustomer()
        {
            return customers;
        }
        public bool CustomerExists(string customerid)
        {
            return customers.Any(c => c.CustomerId == customerid);
        }
        public void RemoveCustomer(string customerId)
        {
            Customer? customer = GetCustomerById(customerId);

            if(customer!=null)
            {
                customers.Remove(customer);
            }

        }

    }

}
