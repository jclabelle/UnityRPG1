using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// StatV2 -> HealthV2, PhysicalPowerV2, etc.
/// Goes into the StatsControllerV2
/// </summary>
///

[System.Serializable]
public class StatV2
{
    int currentValue;
    public int CurrentValue
    {
        get => currentValue;
    }
    //todo: Min and Max values need to take into account Debuffs (changes).
    int maxValue;
    public int MaxValue
    {
        set => maxValue = value;
        get => maxValue;
    }

    int minValue;
    public int MinValue
    {
        set => minValue = value;
        get => minValue;
    }

    List<ChangeStatV2> changes;

    public virtual int Decrease(int value)
    {
        currentValue -= value;
        currentValue = Mathf.Clamp(CurrentValue, MinValue, MaxValue);

        return value;

        //Debug.Log(name + "Decreased Health by:" + value);
    }

    public void Increase(int value)
    {
        currentValue += value;
        currentValue = Mathf.Clamp(CurrentValue, MinValue, MaxValue);

            
    }

    public virtual void Init(int max)
    {
        maxValue = max;
        currentValue = max;
        minValue = 0;
    }

    public virtual void Init(int current, int max, int min = 0, string charName = "")
    {
        maxValue = max;
        currentValue = current;
        minValue = min;
    }

    public enum EStatType
    {
        Health,
        Mana,
        Stamina,
        PhysicalAttack,
        PhysicalDefense,
        PhysicalSpeed,
        MagicalAttack,
        MagicalDefense,
        MagicalSpeed,
    }

    public enum EResourceType
    {
        Health,
        Mana,
        Stamina,
    }
}
