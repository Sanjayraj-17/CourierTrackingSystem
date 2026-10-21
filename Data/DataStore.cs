using System;
using System.Collections.Generic;
using System.Linq;
using CourierTrackingSystem.Models;

namespace CourierTrackingSystem.Data
{
    public class DataStore
    {
        public void LoadShipments(List<Shipment> savedShipments)
        {
            shipments.Clear();

            foreach (Shipment shipment in savedShipments)
            {
                AddShipment(shipment);
            }
        }
        private List<ShipmentStatusHistory> shipmentHistories = new List<ShipmentStatusHistory>();

        public void AddShipmentHistory(ShipmentStatusHistory history)
        {
            shipmentHistories.Add(history);
        }

        public List<ShipmentStatusHistory> GetHistoryByShipmentId(
            string shipmentId)
        {
            return shipmentHistories
                .Where(h => h.ShipmentId == shipmentId)
                .OrderBy(h => h.UpdatedAt)
                .ToList();
        }

        public List<ShipmentStatusHistory> GetAllShipmentHistories()
        {
            return shipmentHistories;
        }

        public void LoadShipmentHistories(
            List<ShipmentStatusHistory> savedHistories)
        {
            shipmentHistories.Clear();
            shipmentHistories.AddRange(savedHistories);
        }

        private List<Shipment> shipments = new List<Shipment>();
        public void LoadCustomers(List<Customer> savedCustomers)
        {
            customers.Clear();

            foreach (Customer customer in savedCustomers)
            {
                AddCustomer(customer);
            }
        }
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

        public void AddShipment(Shipment shipment)
        {
            if (shipments.Any(s => s.ShipmentId == shipment.ShipmentId))
            {
                throw new Exception("Tracking number already exists!");
            }

            shipments.Add(shipment);
        }

        public Shipment? GetShipmentById(string shipmentId)
        {
            return shipments.FirstOrDefault(
                s => s.ShipmentId == shipmentId);
        }

        public List<Shipment> GetAllShipments()
        {
            return shipments;
        }

        public bool ShipmentExists(string shipmentId)
        {
            return shipments.Any(
                s => s.ShipmentId == shipmentId);
        }


    }

}
