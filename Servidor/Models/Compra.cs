namespace Servidor.Models;

public class Compras 
{
    public int id { get; set; }
  
   public  DateTime fecha { get; set; }

   public decimal total { get; set; }

   public string NombreCliente {get; set;}

   public string ApellidoCliente {get; set;}

   public string EmailCliente {get; set;}

}