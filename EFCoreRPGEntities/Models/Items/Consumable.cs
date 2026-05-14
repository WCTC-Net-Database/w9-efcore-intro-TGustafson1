using EFCoreRPGEntities.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRPGEntities.Models.Items
{
    public class Consumable : Item
    {
        public int EffectStrength { get; set;  } = 0;

        public ConsumableEffect Effect { get; set; } = ConsumableEffect.None;

        public override void Use(Player player)
        {
            switch (Effect)
            {
                case ConsumableEffect.None:
                    // No effect
                    break;
                case ConsumableEffect.Heal:
                    player.TemporaryHealth += EffectStrength;
                    break;
                case ConsumableEffect.AbilityRestore:
                    //TODO: Make this successfully choose which ability to restore uses for
                    foreach (var ability in player.Abilities.OfType<PlayerAbility>())
                    {
                        ability.Uses += EffectStrength;
                    }
                    break;
                case ConsumableEffect.StrengthBoost:
                    player.TemporaryStrength += EffectStrength;
                    break;
                case ConsumableEffect.DefenseBoost:
                    player.TemporaryDefense += EffectStrength;
                    break;
                    // Add more cases for different effects as needed
            }
        }
    }
}
