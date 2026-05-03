using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models
{
    public interface ICharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        public int Health { get; set; }

        [NotMapped]
        public int TemporaryHealth { get; set; }

        public int Defense { get; set; }

        [NotMapped]
        public int TemporaryDefense { get; set; }

        public int Strength { get; set; }

        [NotMapped]
        public int TemporaryStrength { get; set; }

        void Attack(ICharacter target);
    }
}
