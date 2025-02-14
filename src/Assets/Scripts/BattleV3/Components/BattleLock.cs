using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3{
    
    public class BattleLock: ILock, IClock
    {
        private bool isLocked;
        public bool IsClockRunning => !isLocked;
        
        public bool TryGrabLock()
        {
            if (isLocked)
                return false;
            
            isLocked = true;
            return true;
        }

        public void ReleaseLock()
        {
            isLocked = false;
        }

        public void ForceGrabLock()
        {
             isLocked = true;
        }
    }
    
    public interface ILock
    {
        public bool TryGrabLock();
        public void ReleaseLock();
        public void ForceGrabLock();
    }

    public interface IClock
    {
        public bool IsClockRunning { get; }
    }
    
}

