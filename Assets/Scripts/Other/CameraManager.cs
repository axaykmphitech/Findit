using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Android;

public class CameraManager : MonoBehaviour
{
    [Header("Sprite & Image")]
    public Sprite spriteImage;

    public static CameraManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        Debug.Log("Checking Camera Permission...");

        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Camera permission already granted.");
        }
        else
        {
            Debug.Log("Requesting Camera Permission...");
            RequestCameraPermission();
        }
    }

    public void RequestCameraPermission()
    {
        Debug.Log("RequestCameraPermission() CALLED!");

        var callbacks = new PermissionCallbacks();

        // Debugging logs before subscribing to eventsg
        Debug.Log("Subscribing to Permission Callbacks...");

        callbacks.PermissionGranted += (permissionName) =>
        {
            Debug.Log("Permission Granted: " + permissionName);
        };

        callbacks.PermissionDenied += (permissionName) =>
        {
            Debug.LogWarning("Permission Denied: " + permissionName);
        };

        callbacks.PermissionDeniedAndDontAskAgain += (permissionName) =>
        {
            Debug.LogError("Permission Denied (Don't Ask Again): " + permissionName);
        };

        // Debug before requesting permission
        Debug.Log("Requesting Permission Now...");
        Permission.RequestUserPermission(Permission.Camera);
        Debug.Log("Permission request sent!");
    }

    //public void Start()
    //{
    //    StartCoroutine(RequestCameraPermission());
    //}

    //public IEnumerator RequestCameraPermission()
    //{
    //    Debug.Log("Checking camera permission...");

    //    Debug.Log(Application.HasUserAuthorization(UserAuthorization.WebCam) + " Application.HasUserAuthorization(UserAuthorization.WebCam)");

    //    if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    //    {
    //        Debug.Log("Requesting camera permission...");
    //        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    //    }

    //    if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    //    {
    //        Debug.Log("Camera permission granted!");
    //    }
    //    else
    //    {
    //        Debug.Log("Camera permission denied!");
    //    }
    //}

    public void TakePicture(Image displayImage)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("Invalid image path");
                return;
            }

            Debug.Log("Image path: " + path);
            StartCoroutine(LoadImageAsync(path, displayImage));
        }, 128);
    }

    private IEnumerator LoadImageAsync(string path, Image displayImage)
    {
        Texture2D texture = NativeCamera.LoadImageAtPath(path, 128);

        if (texture == null)
        {
            Debug.Log("Couldn't load texture from " + path);
            yield break;
        }

        if (displayImage.sprite != null)
        {
            //Destroy(displayImage.sprite.texture);
            displayImage.GetComponent<Image>().sprite = spriteImage;
        }

        Sprite capturedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        displayImage.sprite = capturedSprite;

        SignInManager.Instance.imageFile = ConvertSpriteToBytes(capturedSprite);
        Debug.Log(SignInManager.Instance.imageFile + " imageFile");

        if (SignInManager.Instance.imageFile == null)
        {
            Debug.Log("ConvertSpriteToBytes returned null");
        }
        else
        {
            Debug.Log("Image converted successfully!");
            Debug.Log(SignInManager.Instance.imageFile + " imageFile");
        }
    }


    byte[] ConvertSpriteToBytes(Sprite sprite)
    {
        Debug.Log("ConvertSpriteToBytes");
        if (sprite == null)
        {
            Debug.LogError("Sprite is null!");
            return null;
        }


        // Convert Sprite to Texture2D
        Texture2D texture = SpriteToTexture2D(sprite);
        Debug.Log(texture + " texture");

        // Encode to PNG (You can also use EncodeToJPG())
        return texture.EncodeToPNG();
    }


    Texture2D SpriteToTexture2D(Sprite sprite)
    {
        Debug.Log("SpriteToTexture2D");
        // Create new texture
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGB24, false);
        Debug.Log(texture + " texture_1");


        texture.ReadPixels(new Rect(0, 0, (int)sprite.rect.width, (int)sprite.rect.height), 0, 0);

        return texture;
    }
}