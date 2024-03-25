﻿using System;
using System.Collections.Generic;

namespace WebAPIBanco.Modelos;

public partial class Pago
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Monto { get; set; }

    public int TarjetaId { get; set; }

    public virtual ICollection<Tarjeta> Tarjetas { get; set; } = new List<Tarjeta>();
}
