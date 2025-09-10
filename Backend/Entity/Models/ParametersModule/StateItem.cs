﻿using Entity.Models.Base;
using Entity.Models.System;

namespace Entity.Models.ParametersModule
{
    public class StateItem : GenericEntity
    {
        // Propiedades de Navegacion Inversa
        public List<Item> Items { get; set; } = new List<Item>();
        public List<InventaryDetail> InventaryDetails { get; set; } = new List<InventaryDetail>();
    }
}