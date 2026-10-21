
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CourierTrackingSystem.Models;


namespace CourierTrackingSystem.Data;

public class DataPersistenceService
{
    private readonly string filePath = "DataFiles/customers.json";

    public void SaveCustomers(List<Customer> customers)
    {
        string? directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        string json = JsonSerializer.Serialize(
            customers,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filePath, json);
    }

    private readonly string historyFilePath =
        "DataFiles/shipment-history.json";

    public void SaveShipmentHistories(
        List<ShipmentStatusHistory> histories)
    {
        string? directory = Path.GetDirectoryName(historyFilePath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(
            histories,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(historyFilePath, json);
    }

    public List<ShipmentStatusHistory> LoadShipmentHistories()
    {
        if (!File.Exists(historyFilePath))
        {
            return new List<ShipmentStatusHistory>();
        }

        string json = File.ReadAllText(historyFilePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<ShipmentStatusHistory>();
        }

        return JsonSerializer.Deserialize<List<ShipmentStatusHistory>>(json)
               ?? new List<ShipmentStatusHistory>();
    }


    public List<Customer> LoadCustomers()
    {
        if (!File.Exists(filePath))
            return new List<Customer>();

        string json = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<Customer>();

        return JsonSerializer.Deserialize<List<Customer>>(json)
               ?? new List<Customer>();
    }

    private readonly string shipmentFilePath = "DataFiles/shipments.json";

    public void SaveShipments(List<Shipment> shipments)
    {
        string? directory = Path.GetDirectoryName(shipmentFilePath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonSerializer.Serialize(
            shipments,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(shipmentFilePath, json);
    }

    public List<Shipment> LoadShipments()
    {
        if (!File.Exists(shipmentFilePath))
        {
            return new List<Shipment>();
        }

        string json = File.ReadAllText(shipmentFilePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Shipment>();
        }

        return JsonSerializer.Deserialize<List<Shipment>>(json)
               ?? new List<Shipment>();
    }

}
