using EFCoreRPGEntities.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Containers
{
    public class Container : IContainer
    {
        public int Id { get; set; }

        public string ContainerType { get; set; } = string.Empty;

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public virtual void AddItem(Item item)
        {
            item.ContainerId = Id;
            Items.Add(item);
        }

        public virtual bool RemoveItem(Item item)
        {
            return Items.Remove(item);
        }


    }
}
