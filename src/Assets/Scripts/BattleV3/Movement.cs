using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using BattleV3;
using UnityEngine;

[System.Serializable]
public class Movement
{
    public enum EType
    {
        ShortStepTowardsTarget,
        MoveToTarget,
        MoveToCenterOfBattle,
        StayInPlace,
    }
    
    [field: SerializeField] public Vector2 LocalPositionOrigin { get; set; }
    [field: SerializeField] public Vector2 LocalPositionDestination { get; set; }
    public Vector2 Length => LocalPositionDestination - LocalPositionOrigin;
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float StopMinimumDistance { get; set; } = 0.1f;
    [field: SerializeField] public float MaximumStepSize { get; set; } = 0.1f;
    [field: SerializeField] public float Duration { get; set; } = 1f;

    public Movement(){}

    public Movement(Vector2 localPositionOrigin, Vector2 localPositionDestination, 
        float speed, float stopMinimumDistance = 0.1f, float maximumStepSize = 0.1f)
    {
        LocalPositionOrigin = localPositionOrigin;
        LocalPositionDestination = localPositionDestination;
        Speed = speed;
        StopMinimumDistance = stopMinimumDistance;
        MaximumStepSize = maximumStepSize;
    }
    
    public Movement(Vector2 localPositionOrigin, Vector2 localPositionDestination, 
        EType type, float speed = 1.0f, float stopMinimumDistance = 0.1f, float maximumStepSize = 0.1f)
    {
        LocalPositionOrigin = localPositionOrigin;
        Speed = speed;
        StopMinimumDistance = stopMinimumDistance;
        MaximumStepSize = maximumStepSize;
        
        switch(type)
        {
            case EType.MoveToTarget:
                LocalPositionDestination = localPositionDestination;
                break;
            case EType.ShortStepTowardsTarget:
                LocalPositionDestination = localPositionOrigin + BattleConverters.ResolveLookDirection(localPositionOrigin, localPositionDestination);
                break;
            case EType.MoveToCenterOfBattle:
                LocalPositionDestination = Battle.GetCenterPositionOfBattlefield();
                break;
            case EType.StayInPlace:
                LocalPositionDestination = LocalPositionOrigin;
                StopMinimumDistance = 9999.0f;
                break;
        }
    }

    public void SetValues(Vector2 origin, Vector2 destination,
        float speed, float stopMinimumDistance = 0.1f, float maximumStepSize = 0.1f)
    {
        LocalPositionOrigin = origin;
        LocalPositionDestination = destination;
        Speed = speed;
        StopMinimumDistance = stopMinimumDistance;
        MaximumStepSize = maximumStepSize;
    }
    
    public bool ReachedDestination(Vector2 position)
    {
        return Vector2.Distance(position, LocalPositionDestination) <= StopMinimumDistance;
    }

    public Vector2 NextPosition(Vector2 currentPosition)
    {
        var nextPosition = currentPosition + (LocalPositionDestination - LocalPositionOrigin).normalized * (Speed * Time.fixedDeltaTime);
        var lenghtOfTravel = nextPosition - LocalPositionOrigin;
        if (lenghtOfTravel.sqrMagnitude > Length.sqrMagnitude)
            return LocalPositionDestination;
        
        return nextPosition;
    }

   
}