using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W09.Models.Abilities
{
    public abstract class Ability
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

        public abstract void Activate(Character user, Character target);
    }
}
