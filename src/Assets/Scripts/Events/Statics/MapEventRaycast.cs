using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapEventRaycast
{
    public static Vector2 GetRaycastDirection(IMovement.EDirection eDirection)
    {

        
        switch (eDirection)
        {
            case IMovement.EDirection.Up:
                return Vector2.up;
            
            case IMovement.EDirection.Down:
                return Vector2.down;
            
            case IMovement.EDirection.Left:
                return Vector2.left;
            
            case IMovement.EDirection.Right:
                return Vector2.right;

    
        }

        return Vector2.zero ;
    }

    public static void TriggerEvents(RaycastHit2D hits, GameObject player)
    {
        if(hits.collider.GetComponentInParent<IEventProbeActive>() is IEventProbeActive eventProbeLaunched)
        {
            eventProbeLaunched.EventProbeLaunched(player);
            return;
        }


    }
    
    
    public static void TriggerEventsUnderPlayer(RaycastHit2D hits, GameObject player)
    {
        if(hits.collider.GetComponentInParent<IEventProbePassive>() is IEventProbePassive eventProbeLaunched)
        {
            eventProbeLaunched.EventProbePassive(player);
            return;
        }

  
    }
}