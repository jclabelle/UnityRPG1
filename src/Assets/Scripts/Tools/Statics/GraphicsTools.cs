using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomTools
{

    public static class GraphicsTools
    {
        public static void SaveImageToDisk(Texture2D tex, bool overwrite = true, string fileName = "Image")
        {
            byte[] bytes = tex.EncodeToPNG();
            var dirPath = Application.dataPath + "/../SavedImages/";
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }

            if(overwrite == false && System.IO.File.Exists(dirPath + fileName + ".png"))
            {
                int i = 1;

                while (i<2000)
                {
                    if (System.IO.File.Exists(dirPath + fileName + i.ToString() + ".png"))
                    {
                        i++;
                        continue;
                    }
                    else
                    {
                        System.IO.File.WriteAllBytes(dirPath + fileName + i + ".png", bytes);
                        break;

                    }
                }
            }
            else
            {
                System.IO.File.WriteAllBytes(dirPath + fileName + ".png", bytes);
            }
        }


        // Should only be used inside a coroutine, after yielding to WaitForEndOfFrame 
        static public Texture2D GetScreenshotNow(bool grayscale)
        {
            Texture2D ScreenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            RenderTexture transformedRenderTexture = null;
            RenderTexture renderTexture = RenderTexture.GetTemporary(
                Screen.width,
                Screen.height,
                24,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Default,
                1);
            try
            {
                ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
                transformedRenderTexture = RenderTexture.GetTemporary(
                    ScreenshotTexture.width,
                    ScreenshotTexture.height,
                    24,
                    RenderTextureFormat.ARGB32,
                    RenderTextureReadWrite.Default,
                    1);
                Graphics.Blit(
                    renderTexture,
                    transformedRenderTexture,
                    new Vector2(1.0f, -1.0f),
                    new Vector2(0.0f, 1.0f));
                RenderTexture.active = transformedRenderTexture;
                ScreenshotTexture.ReadPixels(
                    new Rect(0, 0, ScreenshotTexture.width, ScreenshotTexture.height),
                    0, 0);
            }
            catch (System.Exception e)
            {
                Debug.Log("Exception: " + e);
            }
            finally
            {
                RenderTexture.active = null;
                RenderTexture.ReleaseTemporary(renderTexture);
                if (transformedRenderTexture != null)
                {
                    RenderTexture.ReleaseTemporary(transformedRenderTexture);
                }
            }

            ScreenshotTexture.Apply();

            return ScreenshotTexture;
        }




    }
}