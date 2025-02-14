using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class MonoReaction : MonoBehaviour, IMonoReactions
    {
        private ReactionV2.ReactionQuality Quality { get; set; }
        public ReactionV2 Reaction { get; set; }
        private Battler User { get;  set; }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetValues(ReactionV2 reaction, ReactionV2.ReactionQuality quality, Battler user)
        {
            Reaction = reaction;
            Quality = quality;
            User = user;
        }

 

        public float GetReactionModifiers()
        {
            return Reaction.GetReactionModifier(Quality);
        }

        public bool ReactionHasType(ReactionV2.Type type)
        {
            return Reaction.types.Contains(type);
        }

        public ReactionV2 GetReaction()
        {
            return Reaction;
        }

        public Battler GetUser()
        {
            return User;
        }
        
        public void CallDestroy()
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
    
    public interface IMonoReactions
    {
        public float GetReactionModifiers();
        public bool ReactionHasType(ReactionV2.Type type);
        public ReactionV2 GetReaction();
        public Battler GetUser();

        public void CallDestroy();
    }
    
}