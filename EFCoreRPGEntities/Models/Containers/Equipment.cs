using EFCoreRPGEntities.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Containers
{
    public class Equipment : Container
    {
        public int Id { get; set; }

        public int? WeaponId { get; set; }
        public int? ArmorId { get; set; }

        public virtual Item Weapon { get; set; }
        public virtual Item Armor { get; set; }

        public virtual Player Player { get; set; }
    }
}
