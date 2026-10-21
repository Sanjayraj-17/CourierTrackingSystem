using System;
using System.Collections.Generic;
using System.Text;


using System;

namespace CourierTrackingSystem.Models;

public class ShipmentStatusHistory
{
    public string ShipmentId { get; private set; }
    public string Status { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Location { get; private set; }
    public string Notes { get; private set; }

    public ShipmentStatusHistory(
        string shipmentId,
        string status,
        string location,
        string notes)
    {
        ShipmentId = shipmentId;
        Status = status;
        UpdatedAt = DateTime.Now;
        Location = location;
        Notes = notes;
    }
}
