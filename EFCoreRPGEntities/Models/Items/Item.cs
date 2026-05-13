using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCoreRPGEntities.Models.Containers;

namespace EFCoreRPGEntities.Models.Items
{
    public abstract class Item
    {   
        public int Id { get; set; }

        public int? ContainerId { get; set; }
        public virtual Container? Container { get; set; }
        public string Name { get; set; }
    
        public int Weight { get; set; }

        public int Value { get; set; }

        public abstract void Use(Player player);
    }
}
