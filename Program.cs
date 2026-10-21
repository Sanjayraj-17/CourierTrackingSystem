
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
            ShipmentService shipmentService =new ShipmentService(store);
           

            DataPersistenceService persistence =
                new DataPersistenceService();

            CustomerService service = new CustomerService(store);
            List<ShipmentStatusHistory> savedHistories =  persistence.LoadShipmentHistories();

            store.LoadShipmentHistories(savedHistories);

            // Load saved customers when application starts
            List<Customer> savedCustomers = persistence.LoadCustomers();
            store.LoadCustomers(savedCustomers);

            List<Shipment> savedShipments = persistence.LoadShipments();
            store.LoadShipments(savedShipments);


            while (true)
            {
              
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. View All Customers");
                Console.WriteLine("3. Find Customer");
                Console.WriteLine("4. Search Customer");
                Console.WriteLine("5. Update Phone");
                Console.WriteLine("6. Update Address");
                Console.WriteLine("7. Delete Customer");
                Console.WriteLine("8. Create Shipment");
                Console.WriteLine("9. View All Shipments");
                Console.WriteLine("10. Find Shipment by Tracking Number");
                Console.WriteLine("11. Search Shipments");
                Console.WriteLine("12. Update Shipment Status");
                Console.WriteLine("13. View Shipment History");
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

                            persistence.SaveCustomers(store.GetAllCustomer());

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
                        case "8":
                            CreateShipment(shipmentService);
                            persistence.SaveShipments(shipmentService.GetAllShipments());
                            break;

                        case "9":
                            ViewAllShipments(shipmentService);
                            break;

                        case "10":
                            FindShipment(shipmentService);
                            break;

                        case "11":
                            SearchShipments(shipmentService);
                            break;
                        case "12":
                            UpdateShipmentStatus(shipmentService);
                            persistence.SaveShipmentHistories(
                                store.GetAllShipmentHistories());
                            break;

                        case "13":
                            ViewShipmentHistory(shipmentService);
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

        static void UpdateShipmentStatus(ShipmentService shipmentService)
        {
            Console.Write("Enter Tracking Number: ");
            string shipmentId = Console.ReadLine() ?? "";

            Console.WriteLine("1. PickedUp");
            Console.WriteLine("2. InTransit");
            Console.WriteLine("3. OutForDelivery");
            Console.WriteLine("4. Delivered");

            Console.Write("Choose new status: ");
            string choice = Console.ReadLine() ?? "";

            string newStatus = choice switch
            {
                "1" => "PickedUp",
                "2" => "InTransit",
                "3" => "OutForDelivery",
                "4" => "Delivered",
                _ => ""
            };

            if (newStatus == "")
            {
                Console.WriteLine("Invalid status choice!");
                return;
            }

            Console.Write("Enter current location: ");
            string location = Console.ReadLine() ?? "";

            Console.Write("Enter notes: ");
            string notes = Console.ReadLine() ?? "";

            shipmentService.UpdateShipmentStatus(
                shipmentId, newStatus, location, notes);

            Console.WriteLine("Shipment status updated successfully!");
        }

        static void ViewShipmentHistory(ShipmentService shipmentService)
        {
            Console.Write("Enter Tracking Number: ");
            string shipmentId = Console.ReadLine() ?? "";

            List<ShipmentStatusHistory> histories =
                shipmentService.GetShipmentHistory(shipmentId);

            if (histories.Count == 0)
            {
                Console.WriteLine("No history records found.");
                return;
            }

            Console.WriteLine("\n--- Shipment Status History ---");

            foreach (ShipmentStatusHistory history in histories)
            {
                Console.WriteLine($"Status   : {history.Status}");
                Console.WriteLine($"Date     : {history.UpdatedAt}");
                Console.WriteLine($"Location : {history.Location}");
                Console.WriteLine($"Notes    : {history.Notes}");
                Console.WriteLine("--------------------------------");
            }
        }



        static void CreateShipment(ShipmentService service)
        {
            Console.Write("Enter sender customer ID: ");
            string senderId = Console.ReadLine() ?? "";

            Console.Write("Enter receiver name: ");
            string receiverName = Console.ReadLine() ?? "";

            Console.Write("Enter receiver phone: ");
            string receiverPhone = Console.ReadLine() ?? "";

            Console.Write("Enter destination: ");
            string destination = Console.ReadLine() ?? "";

            Console.Write("Enter weight in kg: ");

            if (!double.TryParse(Console.ReadLine(), out double weight))
            {
                Console.WriteLine("Invalid weight.");
                return;
            }

            Console.Write("Enter shipping fee: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal fee))
            {
                Console.WriteLine("Invalid shipping fee.");
                return;
            }

            Shipment shipment = service.CreateShipment(
                senderId,
                receiverName,
                receiverPhone,
                destination,
                weight,
                fee);

            Console.WriteLine("\nShipment created successfully!");
            Console.WriteLine("Tracking Number: " + shipment.ShipmentId);
            Console.WriteLine("Current Status: " + shipment.CurrentStatus);
        }

        static void ViewAllShipments(ShipmentService service)
        {
            var shipments = service.GetAllShipments();

            if (shipments.Count == 0)
            {
                Console.WriteLine("No shipments found.");
                return;
            }

            foreach (Shipment s in shipments)
            {
                Console.WriteLine(
                    $"{s.ShipmentId} | {s.SenderCustomerId} | " +
                    $"{s.ReceiverName} | {s.DestinationAddress} | " +
                    $"{s.Weight} kg | ₹{s.ShippingFee} | {s.CurrentStatus}");
            }
        }

        static void FindShipment(ShipmentService service)
        {
            Console.Write("Enter tracking number: ");
            string trackingNumber = Console.ReadLine() ?? "";

            Shipment s = service.GetShipmentById(trackingNumber);

            Console.WriteLine($"Tracking Number: {s.ShipmentId}");
            Console.WriteLine($"Sender ID: {s.SenderCustomerId}");
            Console.WriteLine($"Receiver: {s.ReceiverName}");
            Console.WriteLine($"Receiver Phone: {s.ReceiverPhone}");
            Console.WriteLine($"Destination: {s.DestinationAddress}");
            Console.WriteLine($"Weight: {s.Weight} kg");
            Console.WriteLine($"Shipping Fee: ₹{s.ShippingFee}");
            Console.WriteLine($"Status: {s.CurrentStatus}");
        }

        static void SearchShipments(ShipmentService service)
        {
            Console.Write("Enter tracking number, sender ID, receiver, destination or status: ");
            string keyword = Console.ReadLine() ?? "";

            var results = service.SearchShipments(keyword);

            if (results.Count == 0)
            {
                Console.WriteLine("No matching shipments found.");
                return;
            }

            foreach (Shipment s in results)
            {
                Console.WriteLine(
                    $"{s.ShipmentId} | {s.ReceiverName} | " +
                    $"{s.DestinationAddress} | {s.CurrentStatus}");
            }
        }

    }



}
