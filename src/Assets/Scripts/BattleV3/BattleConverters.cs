using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public static class BattleConverters
    {
        public static Vector2 DirectionToVector2(IMovement.EDirection eDirection)
        {
            return eDirection switch
            {
                IMovement.EDirection.Right => Vector2.right,
                IMovement.EDirection.Up => Vector2.up,
                IMovement.EDirection.Left => Vector2.left,
                IMovement.EDirection.Down => Vector2.down,
                _ => throw new System.ArgumentOutOfRangeException(nameof(eDirection), eDirection, null)
            };
        }
        
        public static IMovement.EDirection Vector2ToDirection(Vector2 vector)
        {
            switch(vector)
            {
                case var v when v.Equals(Vector2.right):
                    return IMovement.EDirection.Right;
                case var v when v.Equals(Vector2.left):
                    return IMovement.EDirection.Left;
                case var v when v.Equals(Vector2.up):
                    return IMovement.EDirection.Up;
                case var v when v.Equals(Vector2.down):
                    return IMovement.EDirection.Down;
            };
        
            throw new System.SystemException("Vector2 in Vector2ToDirection is not cardinal vector");
        }
        
        public static Vector2 ResolveLookDirection(Vector2 from, Vector2 to)
        {
            var cardinalDirection = to - from;
            
            if (Mathf.Abs(cardinalDirection.x) > Mathf.Abs(cardinalDirection.y))
            {
                cardinalDirection.y = 0;
                cardinalDirection.x = cardinalDirection.x >= 0 ? 1 : -1;
            }
            else
            {
                cardinalDirection.x = 0;
                cardinalDirection.y = cardinalDirection.y >= 0 ? 1 : -1;
            }

            return cardinalDirection;
        }
        
        public static Vector2 LineBetween(Vector2 to, Vector2 from)
        {
            return to - from;
        }
        
        public static float Resolve360Angle(Vector2 from, Vector2 to)
        {
            var angle = Vector2.Angle(from, to);
            return Vector2.Angle(Vector2.right, to) > 90f ? 360f - angle : angle;
        }
    }
}