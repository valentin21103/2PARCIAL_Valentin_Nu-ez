using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cliente.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = string.Empty;

        public List<ItemCompra> Items { get; set; } = new List<ItemCompra>();
        public decimal Total => Items.Sum(item => item.Subtotal);
    }
} 