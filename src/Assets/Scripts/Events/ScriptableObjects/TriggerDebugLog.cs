using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameActions
{
    [CreateAssetMenu]

    public class TriggerDebugLog : GameAction
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void DoAction()
        {
            Debug.Log("Triggered by a choice in a dialogue");
        }
    }
}