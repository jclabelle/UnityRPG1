using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogues
{
    public class TriggeredDialoguePlayer : DialoguePlayer, IEventProbeActive
    {


        protected new void Start()
        {
            base.Start();
        }

        private void PlayEvent()
        {
            StartCoroutine(PlayDialogueTree());
        }


        public void EventProbeLaunched(GameObject player)
        {
            PlayEvent();
        }
    }
}
