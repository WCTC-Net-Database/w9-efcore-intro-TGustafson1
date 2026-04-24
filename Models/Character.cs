namespace W09.Models;

using Models.Abilities;

public abstract class Character : ICharacter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }

    public int Health { get; set; }

    // Foreign key to Room
    public int RoomId { get; set; }

    // Navigation property to Room
    public virtual Room Room { get; set; }

    public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();

    public virtual void Attack(ICharacter target)
    {
        Console.WriteLine($"{Name} attacks {target.Name}!");
    }

    public virtual void UseAbility(Ability ability, ICharacter target)
    {
        if (target is Character characterTarget)
        {
            ability.Activate(this, characterTarget);
        }
    }
}