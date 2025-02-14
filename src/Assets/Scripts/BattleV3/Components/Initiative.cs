using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class Initiative: IReactionWindow
    {
        public float value;
        public float maxValue;
        public bool Elapsed => value >= maxValue;
        public bool IsOpen => value <= maxValue;
        public float Duration => maxValue;
        public float RemainingTime => maxValue - value;

        public void Advance()
        {
            value = Elapsed ? 0 : value;
            value += Time.deltaTime;
        }

        public void Advance(float timeIncrement)
        {
            value += timeIncrement;
        }

        public Initiative(float maxValue)
        {
            this.maxValue = maxValue;
        }
        
        public Initiative(float maxValue, float initialValue)
        {
            this.maxValue = maxValue;
            this.value = initialValue;
        }
    }

    public interface IReactionWindow
    {
        public bool Elapsed { get; }
        public float Duration { get; }
        public void Advance();
        public float RemainingTime { get; }
        public bool IsOpen { get; }


    }
}