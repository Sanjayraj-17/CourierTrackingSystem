
using System;
using System.Collections.Generic;
using System.Linq;
using CourierTrackingSystem.Data;
using CourierTrackingSystem.Models;

namespace CourierTrackingSystem.Services;

public class ShipmentService
{

    private readonly DataStore dataStore;
    private int nextShipmentNumber = 1;

    public ShipmentService(DataStore dataStore)
    {
        this.dataStore = dataStore;
    }

  

    // 1. Create Shipment
    public Shipment CreateShipment(
        string senderCustomerId,
        string receiverName,
        string receiverPhone,
        string destination,
        double weight,
        decimal shippingFee)
    {
        // Check sender exists
        if (dataStore.GetCustomerById(senderCustomerId) == null)
        {
            throw new Exception("Sender customer not found!");
        }

        // Validate receiver name
        if (string.IsNullOrWhiteSpace(receiverName))
        {
            throw new ArgumentException("Receiver name cannot be empty.");
        }

        // Validate receiver phone
        if (string.IsNullOrWhiteSpace(receiverPhone) ||
            receiverPhone.Length != 10 ||
            !receiverPhone.All(char.IsDigit))
        {
            throw new ArgumentException(
                "Receiver phone must contain 10 digits.");
        }

        // Validate destination
        if (string.IsNullOrWhiteSpace(destination))
        {
            throw new ArgumentException("Destination cannot be empty.");
        }

        // Validate weight
        if (weight <= 0)
        {
            throw new ArgumentException(
                "Weight must be greater than zero.");
        }

        // Validate shipping fee
        if (shippingFee < 0)
        {
            throw new ArgumentException(
                "Shipping fee cannot be negative.");
        }

        // Generate unique tracking number
        string trackingNumber;

        do
        {
            trackingNumber = $"SHP-{nextShipmentNumber:D4}";
            nextShipmentNumber++;
        }
        while (dataStore.ShipmentExists(trackingNumber));

        Shipment shipment = new Shipment(
            trackingNumber,
            senderCustomerId,
            receiverName.Trim(),
            receiverPhone,
            destination.Trim(),
            weight,
            shippingFee);

        dataStore.AddShipment(shipment);

        return shipment;
    }

    // 2. Get All Shipments
    public List<Shipment> GetAllShipments()
    {
        return dataStore.GetAllShipments()
            .OrderBy(s => s.CreatedDate)
            .ToList();
    }

    // 3. Get Shipment By Tracking Number
    public Shipment GetShipmentById(string trackingNumber)
    {
        Shipment? shipment =
            dataStore.GetShipmentById(trackingNumber);

        if (shipment == null)
        {
            throw new InvalidOperationException(
                "Shipment not found!");
        }

        return shipment;
    }

    // 4. Search Shipments
    public List<Shipment> SearchShipments(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            throw new ArgumentException(
                "Search keyword cannot be empty.");
        }

        return dataStore.GetAllShipments()
            .Where(s =>
                s.ShipmentId.Contains(
                    keyword, StringComparison.OrdinalIgnoreCase)
                || s.ReceiverName.Contains(
                    keyword, StringComparison.OrdinalIgnoreCase)
                || s.DestinationAddress.Contains(
                    keyword, StringComparison.OrdinalIgnoreCase)
                || s.SenderCustomerId.Contains(
                    keyword, StringComparison.OrdinalIgnoreCase)
                || s.CurrentStatus.Contains(
                    keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void UpdateShipmentStatus(
        string shipmentId,
        string newStatus,
        string location,
        string notes)
    {
        Shipment? shipment = dataStore.GetShipmentById(shipmentId);

        if (shipment == null)
            throw new InvalidOperationException("Shipment not found!");

        string[] validStatuses =
        {
        "PickedUp",
        "InTransit",
        "OutForDelivery",
        "Delivered"
    };

        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException("Invalid shipment status!");

        string currentStatus = shipment.CurrentStatus;

        bool validTransition =
            (currentStatus == "Created" && newStatus == "PickedUp") ||
            (currentStatus == "PickedUp" && newStatus == "InTransit") ||
            (currentStatus == "InTransit" && newStatus == "OutForDelivery") ||
            (currentStatus == "OutForDelivery" && newStatus == "Delivered");

        if (!validTransition)
            throw new InvalidOperationException(
                $"Cannot change status from {currentStatus} to {newStatus}!");

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be empty!");

        if (string.IsNullOrWhiteSpace(notes))
            throw new ArgumentException("Notes cannot be empty!");

        shipment.UpdateStatus(newStatus);

        ShipmentStatusHistory history = new ShipmentStatusHistory(
            shipmentId,
            newStatus,
            location.Trim(),
            notes.Trim()
        );

        dataStore.AddShipmentHistory(history);
    }

    public List<ShipmentStatusHistory> GetShipmentHistory(string shipmentId)
    {
        Shipment? shipment = dataStore.GetShipmentById(shipmentId);

        if (shipment == null)
            throw new InvalidOperationException("Shipment not found!");

        return dataStore.GetHistoryByShipmentId(shipmentId);
    }



}
