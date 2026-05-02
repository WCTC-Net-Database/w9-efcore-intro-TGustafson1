using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models.Items
{
    public class Equipment
    {
        public int Id { get; set; }

        public int? WeaponId { get; set; }
        public int? ArmorId { get; set; }

        public virtual Item Weapon { get; set; }
        public virtual Item Armor { get; set; }

        public virtual Player Player { get; set; }
    }
}
