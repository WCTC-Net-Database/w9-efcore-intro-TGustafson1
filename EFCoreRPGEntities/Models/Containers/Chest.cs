using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Containers
{
    public class Chest : Container
    {
        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }
        //TODO: Add chest description
    }
}
