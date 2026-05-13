using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Containers
{
    public class Inventory : Container
    {
        public int MaxWeight { get; set; } = 100;
    }
}
