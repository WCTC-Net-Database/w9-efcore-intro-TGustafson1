namespace W09.Models;

using Models.Abilities;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class Character : ICharacter
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

    // Foreign key to Room
    public int? RoomId { get; set; }

    // Navigation property to Room
    public virtual Room Room { get; set; }

    public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();

    public virtual void Attack(ICharacter target)
    {
        Console.WriteLine($"{Name} tries to attack {target.Name}, but fails due to not being implemented!");
    }

    public virtual void UseAbility(Ability ability, ICharacter target)
    {
        if (target is Character characterTarget)
        {
            ability.Activate(this, characterTarget);
        }
    }
}