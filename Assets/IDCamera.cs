using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IDCamera : MonoBehaviour
{
    [SerializeField] private Camera _photoCamera;

    [ButtonMethod]
    public void TakePicture()
    {
        var width = 1000;
        var height = 1200;

        var mod = 4;
        width *= mod;
        height *= mod;

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        RenderTexture.active = renderTexture;

        _photoCamera.targetTexture = renderTexture;
        _photoCamera.Render();
        _photoCamera.targetTexture = null;

        Rect captureRect = new Rect(0, height/3, width/2, height/2);
        Texture2D screenshot = new Texture2D(width/2, height/2, TextureFormat.RGB24, false);
        screenshot.ReadPixels(captureRect, 0, 0);
        screenshot.Apply();

        var btyes = screenshot.EncodeToPNG();
        var path = Application.streamingAssetsPath + "/ID_images";

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        File.WriteAllBytes(path + "/IDimage.png", btyes);

        if (Application.isPlaying) Destroy(screenshot);
        else DestroyImmediate(screenshot);
    }
}
