using System;
using System.Collections.Generic;
using System.Text;


namespace CourierTrackingSystem.Models;

public class Shipment
{
    public string ShipmentId { get; private set; }

    public string SenderCustomerId { get; private set; }

    public string ReceiverName { get; private set; }

    public string ReceiverPhone { get; private set; }

    public string DestinationAddress { get; private set; }

    public DateTime CreatedDate { get; private set; }

    public double Weight { get; private set; }

    public decimal ShippingFee { get; private set; }

    public string CurrentStatus { get; private set; }

    public Shipment(
        string shipmentId,
        string senderCustomerId,
        string receiverName,
        string receiverPhone,
        string destinationAddress,
        double weight,
        decimal shippingFee)
    {
        ShipmentId = shipmentId;
        SenderCustomerId = senderCustomerId;
        ReceiverName = receiverName;
        ReceiverPhone = receiverPhone;
        DestinationAddress = destinationAddress;
        Weight = weight;
        ShippingFee = shippingFee;

        CreatedDate = DateTime.Now;
        CurrentStatus = "Created";
    }

    public void UpdateReceiverPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty.");

        ReceiverPhone = phone.Trim();
    }

    public void UpdateDestination(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.");

        DestinationAddress = address.Trim();
    }

    public void UpdateWeight(double weight)
    {
        if (weight <= 0)
            throw new ArgumentException("Weight must be greater than zero.");

        Weight = weight;
    }

    public void UpdateShippingFee(decimal fee)
    {
        if (fee < 0)
            throw new ArgumentException("Shipping fee cannot be negative.");

        ShippingFee = fee;
    }

    public void UpdateStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("Status cannot be empty!");

        CurrentStatus = newStatus;
    }

}
