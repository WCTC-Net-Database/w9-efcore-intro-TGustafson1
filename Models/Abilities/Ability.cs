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
        public string Name { get; set; } = "Default Ability";

        public string Description { get; set; } = "Default description";

        public ICollection<Character> Characters { get; set; } = new List<Character>();
        public int AbilityLevel { get; set; }

        public int DefenseModifier { get; set; } = 0;
        public int StrengthModifier { get; set; } = 0;
        public int HealthModifier { get; set; } = 0;

        public abstract void Activate(Character user, Character target);
    }
}
