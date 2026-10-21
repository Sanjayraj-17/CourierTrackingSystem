using System;
using System.Collections.Generic;
using System.Text;


namespace CourierTrackingSystem.Models;

public class Customer
{
    public string CustomerId { get; set; }

    public string FullName { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public Customer(string customerId,string fullName, string phoneNumber, string address)
    {
        CustomerId = customerId;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException(
                "Phone number cannot be empty.");

        PhoneNumber = phoneNumber.Trim();
    }

    public void UpdateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException(
                "Address cannot be empty.");

        Address = address.Trim();
    }
}
