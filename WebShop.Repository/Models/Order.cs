﻿namespace Repository.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Customer Customer { get; set; } = new(); // Navigation property
    public List<Product> Products { get; set; } = new(); // Navigation property
    public bool IsShipped { get; set; }
}
