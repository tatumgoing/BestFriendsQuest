using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class IDCamera : MonoBehaviour
{
    [SerializeField] private Camera _photoCamera;
    [SerializeField] private DragToSpin _characterSpinner;
    [SerializeField] private Sound _cameraSound;
    [SerializeField] private GameObject _greenScreen;

    private void Start()
    {
        _cameraSound = Instantiate(_cameraSound);   
    }

    public async void TakePicture()
    {
        _cameraSound.Play();
        GetComponent<CameraController>().ResetZoom(true);
        _characterSpinner.Reset();
        UIManager.i.FlashCamera();

        await Task.Delay(100);


        _greenScreen.SetActive(true);

        var width = 1200;
        var height = 1200;

        var mod = 4;
        width *= mod;
        height *= mod;

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        RenderTexture.active = renderTexture;

        _photoCamera.targetTexture = renderTexture;
        _photoCamera.Render();
        _photoCamera.targetTexture = null;

        Rect captureRect = new Rect(width/8, height * 0.52f, width/2, height/2);
        Texture2D idPicture = new Texture2D(width/4, height/4, TextureFormat.ARGB32, false);
        idPicture.ReadPixels(captureRect, 0, 0);
        idPicture.Apply();

        Color target = Utils.HexToColor("#83E221");
        Color[] pixels = idPicture.GetPixels();

        for (int i = 0; i < pixels.Length; i++) {
            // exact match
            /*if (pixels[i].Equals(target)) {
                pixels[i].a = 0f; // make transparent
            }*/
            
            // OPTIONAL: fuzzy match (tolerance)
            if (Vector3.Distance(new Vector3(pixels[i].r, pixels[i].g, pixels[i].b),
                                 new Vector3(target.r, target.g, target.b)) < 0.05f)
            {
                 pixels[i].a = 0f;
            }
        }

        idPicture.SetPixels(pixels);
        idPicture.Apply();


        _greenScreen.SetActive(false);

        var btyes = idPicture.EncodeToPNG();
        var path = Application.streamingAssetsPath + "/ID_images";

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        File.WriteAllBytes(path + "/IDimage.png", btyes);

        FindFirstObjectByType<IdPhotoController>(FindObjectsInactive.Include).ShowPicture(idPicture);

        //if (Application.isPlaying) Destroy(idPicture);
        //else DestroyImmediate(idPicture);
    }
}
