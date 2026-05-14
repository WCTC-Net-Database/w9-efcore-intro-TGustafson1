using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCoreRPGEntities.Models;

namespace EFCoreRPGEntities.Models.Containers
{
    public class Chest : Container, ILockable
    {
        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }

        public bool IsLocked { get; set; } = false;
        public int? RequiredKeyItemId { get; set; } = null;
        //TODO: Add chest description
    }
}
