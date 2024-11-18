using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Customer Customer { get; set; } // Navigation property
    public List<Product> Products { get; set; }
}
