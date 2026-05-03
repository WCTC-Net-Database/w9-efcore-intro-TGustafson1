using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Items
{
    public class Item
    {   public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public int Attack {  get; set; }

        public int Defense { get; set; }
    
        public int Weight { get; set; }

        public int Value { get; set; }
    }
}
