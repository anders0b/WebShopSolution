﻿using Repository.Models;

namespace WebShop.Repository.Models;

public class OrderItem
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } 
}
