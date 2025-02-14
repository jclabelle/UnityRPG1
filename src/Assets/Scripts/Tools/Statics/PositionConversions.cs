using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomTools
{
    public static class PositionConversions
    {
        public static Vector2 WorldToGUI(Vector2 worldPosition)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            screenPosition.y = Screen.height - screenPosition.y;
            return screenPosition;
        }

    }
}
