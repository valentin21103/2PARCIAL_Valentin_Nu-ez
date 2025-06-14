using System.Collections.Generic;
using System.Linq;

namespace Cliente.Models
{
    public class Carrito
    {
        public List<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();
        public decimal Total => Items.Sum(item => item.Subtotal);
    }
} 