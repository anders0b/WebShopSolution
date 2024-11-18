namespace Repository.Models;

// Produktmodellen representerar en produkt i webbshoppen
public class Product
{
    public int Id { get; set; } // Unikt ID f�r produkten
    public string Name { get; set; } // Namn p� produkten
    public string Description { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
}