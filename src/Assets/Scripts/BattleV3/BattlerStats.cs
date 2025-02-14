using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class BattlerStats: IBattlerStats
    {
   

        public HealthV2 Health { get; set; }
        public StaminaV2 Stamina { get; set; }
        public ManaV2 Mana { get; set; }
        
        public PhysicalAttackV2 PhyAtk { get; set; }
        public PhysicalDefenseV2 PhyDef { get; set; }
        public PhysicalSpeedV2 PhySpd { get; set; }
        
        public MagicalAttackV2 MagAtk { get; set; }
        public MagicalDefenseV2 MagDef { get; set; }
        public MagicalSpeedV2 MagSpd { get; set; }
        public bool IsAlive => Health.CurrentValue > 0;

        public bool CheckForCritical(StatV2.EResourceType stat) 
        {
            return false;
        }
        
        public float GetCriticalHitPowerModifier(StatV2.EResourceType stat) 
        {
            return 0;
        }

        public float GetBaseEffectPower(EffectV2 effect)
        {
            //todo: Implement modifiers from stats, equipment, buffs and debuffs.
            return 1;
        }

        public float GetResistanceOrVulnerability(EffectV2 effect, float value)
        {
            //todo: Implement Resistance modifiers from stats, equipment, buffs and debuffs.
            return value;
        }
        
        public BattlerStats(BattlerData data)
        {
            Health = new HealthV2();
            Stamina = new StaminaV2();
            Mana = new ManaV2();
            PhyAtk = new PhysicalAttackV2();
            PhyDef = new PhysicalDefenseV2();
            PhySpd = new PhysicalSpeedV2();
            MagAtk = new MagicalAttackV2();
            MagDef = new MagicalDefenseV2();
            MagSpd = new MagicalSpeedV2();
            
            Health.Init(data.Stats.Health);
            Stamina.Init(data.Stats.Stamina);
            Mana.Init(data.Stats.Mana);
            PhyAtk.Init(data.Stats.PhyAtk);
            PhyDef.Init(data.Stats.PhyDef);
            PhySpd.Init(data.Stats.PhySpd);
            MagAtk.Init(data.Stats.MagAtk);
            MagDef.Init(data.Stats.MagDef);
            MagSpd.Init(data.Stats.MagSpd);
            
            
        }
        
        public StatV2 GetStatOfType(StatV2.EStatType eStatType)
        {

            switch (eStatType)
            {
                case StatV2.EStatType.Health:
                    return Health;
                case StatV2.EStatType.Stamina:
                    return Stamina;
                case StatV2.EStatType.Mana:
                    return Mana;
                case StatV2.EStatType.PhysicalAttack:
                    return PhyAtk;
                case StatV2.EStatType.PhysicalDefense:
                    return PhyDef;
                case StatV2.EStatType.PhysicalSpeed:
                    return PhySpd;
                case StatV2.EStatType.MagicalAttack:
                    return MagAtk;
                case StatV2.EStatType.MagicalDefense:
                    return MagDef;
                case StatV2.EStatType.MagicalSpeed:
                    return MagSpd;
            }
            return null;
        }
        
        public void DecreaseStatBy(StatV2.EStatType eStatType, int value)
        {
            switch (eStatType)
            {
                case StatV2.EStatType.Health:
                    Health.Decrease(value);
                    break;
                case StatV2.EStatType.Stamina:
                    Stamina.Decrease(value);
                    break;
                case StatV2.EStatType.Mana:
                    Mana.Decrease(value);
                    break;
                case StatV2.EStatType.PhysicalAttack:
                    PhyAtk.Decrease(value);
                    break;
                case StatV2.EStatType.PhysicalDefense:
                    PhyDef.Decrease(value);
                    break;
                case StatV2.EStatType.PhysicalSpeed:
                    PhySpd.Decrease(value);
                    break;
                case StatV2.EStatType.MagicalAttack:
                    PhyAtk.Decrease(value);
                    break;
                case StatV2.EStatType.MagicalDefense:
                    PhyDef.Decrease(value);
                    break;
                case StatV2.EStatType.MagicalSpeed:
                    PhySpd.Decrease(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eStatType), eStatType, "BattlerStats.DecreaseStat: EStatType Case not handled.");
            }
        }
        
        public void IncreaseStatBy(StatV2.EStatType eStatType, int value)
        {
            switch (eStatType)
            {
                case StatV2.EStatType.Health:
                    Health.Increase(value);
                    break;
                case StatV2.EStatType.Stamina:
                    Stamina.Increase(value);
                    break;
                case StatV2.EStatType.Mana:
                    Mana.Increase(value);
                    break;
                case StatV2.EStatType.PhysicalAttack:
                    PhyAtk.Increase(value);
                    break;
                case StatV2.EStatType.PhysicalDefense:
                    PhyDef.Increase(value);
                    break;
                case StatV2.EStatType.PhysicalSpeed:
                    PhySpd.Increase(value);
                    break;
                case StatV2.EStatType.MagicalAttack:
                    PhyAtk.Increase(value);
                    break;
                case StatV2.EStatType.MagicalDefense:
                    PhyDef.Increase(value);
                    break;
                case StatV2.EStatType.MagicalSpeed:
                    PhySpd.Increase(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eStatType), eStatType, "BattlerStats.IncreaseStat: EStatType Case not handled.");
            }

            return;
        }
    }

    public interface IBattlerStats
    {
        public StatV2 GetStatOfType(StatV2.EStatType eStatType);
        public void DecreaseStatBy(StatV2.EStatType eStatType, int value);
        public void IncreaseStatBy(StatV2.EStatType eStatType, int value);

        public float GetResistanceOrVulnerability(EffectV2 effect, float value);
        public float GetBaseEffectPower(EffectV2 effect);
        public bool CheckForCritical(StatV2.EResourceType stat);
        public float GetCriticalHitPowerModifier(StatV2.EResourceType stat);
        
        public HealthV2 Health { get; set; }
        public StaminaV2 Stamina { get; set; }
        public ManaV2 Mana { get; set; }
        
        public PhysicalAttackV2 PhyAtk { get; set; }
        public PhysicalDefenseV2 PhyDef { get; set; }
        public PhysicalSpeedV2 PhySpd { get; set; }
        
        public MagicalAttackV2 MagAtk { get; set; }
        public MagicalDefenseV2 MagDef { get; set; }
        public MagicalSpeedV2 MagSpd { get; set; }
        public bool IsAlive { get; }
    }
    
}