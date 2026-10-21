
using System;
using CourierTrackingSystem.Data;
using CourierTrackingSystem.Models;
using CourierTrackingSystem.Services;

namespace CourierTrackingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataStore store = new DataStore();
            CustomerService service = new CustomerService(store);

            while (true)
            {
              
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. View All Customers");
                Console.WriteLine("3. Find Customer");
                Console.WriteLine("4. Search Customer");
                Console.WriteLine("5. Update Phone");
                Console.WriteLine("6. Update Address");
                Console.WriteLine("7. Delete Customer");
                Console.WriteLine("0. Exit");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter name: ");
                            string name = Console.ReadLine() ?? "";

                            Console.Write("Enter phone: ");
                            string phone = Console.ReadLine() ?? "";

                            Console.Write("Enter address: ");
                            string address = Console.ReadLine() ?? "";

                            Customer customer =
                                service.RegisterCustomer(name, phone, address);

                            Console.WriteLine("Customer registered!");
                            Console.WriteLine("ID: " + customer.CustomerId);
                            break;

                        case "2":
                            foreach (Customer c in service.GetAllCustomer())
                            {
                                Console.WriteLine(
                                    $"{c.CustomerId} | {c.FullName} | {c.PhoneNumber} | {c.Address}");
                            }
                            break;

                        case "3":
                            Console.Write("Enter customer ID: ");
                            Customer found = service.GetCustomerById(
                                Console.ReadLine() ?? "");

                            Console.WriteLine(
                                $"{found.CustomerId} | {found.FullName} | {found.PhoneNumber} | {found.Address}");
                            break;

                        case "4":
                            Console.Write("Enter name or phone: ");
                            var results = service.SearchCustomers(
                                Console.ReadLine() ?? "");

                            foreach (Customer c in results)
                            {
                                Console.WriteLine(
                                    $"{c.CustomerId} | {c.FullName} | {c.PhoneNumber}");
                            }
                            break;

                        case "5":
                            Console.Write("Enter customer ID: ");
                            string phoneId = Console.ReadLine() ?? "";

                            Console.Write("Enter new phone: ");
                            string newPhone = Console.ReadLine() ?? "";

                            service.UpdatePhoneNumber(phoneId, newPhone);
                            Console.WriteLine("Phone updated!");
                            break;

                        case "6":
                            Console.Write("Enter customer ID: ");
                            string addressId = Console.ReadLine() ?? "";

                            Console.Write("Enter new address: ");
                            string newAddress = Console.ReadLine() ?? "";

                            service.UpdateAddress(addressId, newAddress);
                            Console.WriteLine("Address updated!");
                            break;

                        case "7":
                            Console.Write("Enter customer ID: ");
                            string deleteId = Console.ReadLine() ?? "";

                            service.DeleteCustomer(deleteId);
                            Console.WriteLine("Customer deleted!");
                            break;

                        case "0":
                            Console.WriteLine("Thank you!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
