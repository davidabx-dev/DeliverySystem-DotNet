namespace DeliveryApi.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerName { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
}