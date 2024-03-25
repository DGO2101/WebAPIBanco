using System;
using System.Collections.Generic;

namespace WebAPIBanco.Modelos;

public partial class Tarjeta
{
    public int Id { get; set; }

    public string Numero { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
