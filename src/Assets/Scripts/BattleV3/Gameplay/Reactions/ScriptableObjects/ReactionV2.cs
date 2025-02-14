using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/ReactionV2"), System.Serializable]
public class ReactionV2 : AbilityV2
{
    public EffectV2 matchingEffectClass;
    public List<Type> types;
    public bool IsInterrupt => types.Contains(ReactionV2.Type.Interrupt);

    public enum ReactionQuality
    {
        Weak,
        Average,
        Good,
        Great,
        Perfect,
    }

    public enum Type
    {
        ModifyIncomingEffectSelf, // The reaction modifies an incoming effect for the reactingUser in some way.
        ModifyIncomingEffectAll, // The reaction modifies an incoming effect for all targets in some way.
        TriggerAbility,       // The reaction triggers an ability.
        Interrupt, // The reaction can interrupt the ability.
    }

    public virtual bool IsMatch(EffectV2 effect)
    {
        return effect.GetType() == matchingEffectClass.GetType();
    }

    // Returns true if at least one of the ability's effect matches the reaction's effect type
    public virtual bool IsMatch(AbilityV2 ability)
    {
        foreach (var effect in ability.EffectPowerDict.Keys)
        {
            if (IsMatch(effect))
                return true;
        }

        return false;
    }

    public override Newtonsoft.Json.Linq.JObject GetSaveGameJson()
    {
        return base.GetSaveGameJson();
    }



    public virtual float GetReactionModifier(ReactionQuality qual)
    {
        // Todo: Implement this.
        return 0;
    }
    
    public static ReactionV2.ReactionQuality GetReactionQuality(float reactionTimeStamp, AbilityV2 ability, float reactionWindowDuration)
    {
        float atOneThird =  ability.BaseReactionWindowDuration /3;
        float abilityCounterStartTimestamp = Time.time - ability.BaseReactionWindowDuration - reactionWindowDuration;
        
        float weak = abilityCounterStartTimestamp + atOneThird;
        float average = abilityCounterStartTimestamp + atOneThird * 2;
        float good = abilityCounterStartTimestamp + atOneThird * 3;
        float great = abilityCounterStartTimestamp + ability.BaseReactionWindowDuration + (reactionWindowDuration * 0.8f);
        //float perfect = abilityCounterStartTimestamp + abilityReactedTo.activationTime + abilityAnnouncementDuration;

        if (reactionTimeStamp <= weak)
        {
            return ReactionV2.ReactionQuality.Weak;
        }
        else if (reactionTimeStamp <= average)
        {
            return ReactionV2.ReactionQuality.Average;
        }
        else if (reactionTimeStamp <= good)
        {
            return ReactionV2.ReactionQuality.Good;
        }
        else if (reactionTimeStamp <= great)
        {
            return ReactionV2.ReactionQuality.Great;
        }
        else
        {
            return ReactionV2.ReactionQuality.Perfect;
        }

    }


}


