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
        //TODO: Add properties to define the effect and strength of the consumable 
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
                    //TODO: Account for multiple abilities and restore them properly
                    ((PlayerAbility)player.Abilities.FirstOrDefault()).Uses += EffectStrength;
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
