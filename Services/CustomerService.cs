

using System;
using System.Collections.Generic;
using System.Linq;
using CourierTrackingSystem.Data;
using CourierTrackingSystem.Models;
namespace CourierTrackingSystem.Services;

public class CustomerService
{
    private readonly DataStore dataStore;

    public CustomerService(DataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public Customer RegisterCustomer(string name,string phone,string address)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("the name does not exists");
        }
        if(string.IsNullOrWhiteSpace(phone)||phone.Length!=10|| !phone.All(char.IsDigit))
        {
            throw new ArgumentException("Enter a valid 10-digit phone number.");
        }
        if (string.IsNullOrWhiteSpace(address))
        {

            throw new ArgumentException("Address cannot be empty.");

        }
        string customerId = "CUS-" + (dataStore.GetAllCustomer().Count + 1).ToString("D4");

        while (dataStore.CustomerExists(customerId))
        {
            int nextId = dataStore.GetAllCustomer().Count + 2;
            customerId = "CUS-" + nextId.ToString("D4");
        }
        Customer customer = new Customer(customerId, name.Trim(), phone.Trim(), address.Trim());

        dataStore.AddCustomer(customer);
        return customer;


    }
    public List<Customer> GetAllCustomer()
    {
        return dataStore.GetAllCustomer()
            .OrderBy(c => c.FullName)
            .ToList();
    }

    public Customer GetCustomerById(string customerId)
    {
        Customer? customer = dataStore.GetCustomerById(customerId);

        if (customer == null)
            throw new InvalidOperationException("Customer not found.");

        return customer;
    }
    public List<Customer> SearchCustomers(string keyword)
    {
        return dataStore.GetAllCustomer()
            .Where(c =>c.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || c.PhoneNumber.Contains(keyword)).ToList();
    }
    public void UpdatePhoneNumber(string customerId, string newPhone)
    {
        Customer customer = GetCustomerById(customerId);

        if (string.IsNullOrWhiteSpace(newPhone) ||
            newPhone.Length != 10 ||
            !newPhone.All(char.IsDigit))
            throw new ArgumentException("Invalid phone number.");

        customer.UpdatePhoneNumber(newPhone);
    }
    public void UpdateAddress(string customerId, string newAddress)
    {
        Customer customer = GetCustomerById(customerId);

        if (string.IsNullOrWhiteSpace(newAddress))
            throw new ArgumentException("Address cannot be empty.");

        customer.UpdateAddress(newAddress);
    }
    public void DeleteCustomer(string customerId)
    {
        Customer customer = GetCustomerById(customerId);

        dataStore.RemoveCustomer(customer.CustomerId);
    }


}













