using EFCoreRPGEntities.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Containers
{
    public interface IContainer
    {
        ICollection<Item> Items { get; }

        void AddItem(Item item);
        bool RemoveItem(Item item);
    }
}
