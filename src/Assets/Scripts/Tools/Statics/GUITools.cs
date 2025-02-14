using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomTools
{
    public static class GUITools
    {
     

        static public GUIStyle CreateStyle(int fontsize = 48, Color? color = null, bool wordWrap = true,
            TextAnchor anchor = TextAnchor.UpperLeft, Texture2D texture = null)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = fontsize;
            if (color is Color c)
            {
                style.normal.textColor = c;
            }
            else
            {
                style.normal.textColor = Color.black;
            }

            style.wordWrap = wordWrap;
            style.alignment = anchor;
            style.padding = new RectOffset(5, 5, 5, 5);
            if (texture is Texture2D tex)
                style.normal.background = tex;

            return style;
        }

        static public GUIStyle GetDefaultStyle(string s)
        {
            switch (s)
            {
                case "button":
                {
                    return new GUIStyle(GUI.skin.button);
                }
                case "textfield":
                {
                    return new GUIStyle(GUI.skin.textField);
                }
            }

            return new GUIStyle();

        }

        public static Texture2D CreateBackground(float widthScreenRatio, float heightScreenRatio, Color32 color)
        {
            float width = Screen.width * widthScreenRatio;
            float height = Screen.height * heightScreenRatio;
            int widthInt = (int)width;
            int heightInt = (int)height;

            Texture2D background = new Texture2D(widthInt, heightInt, TextureFormat.RGBA32, false);
            Color32[] backgroundColors = background.GetPixels32();

            for (int i = 0; i < backgroundColors.Length; i++)
            {
                backgroundColors[i] = color;
            }

            background.SetPixels32(backgroundColors);
            background.Apply();
            return background;
        }
        
        
        public static Rect CreateRect(float x, float y, float w, float h)
        {
             return new Rect(x, y, w, h);
        }

    }
}