using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [Header("Int & Float")]
    float xPos;
    float yPos;
    float xHeight;
    float yHeight;

    [Header("GameObject")]
    public GameObject submitPanel;
    public GameObject createLevelPanel;
    public GameObject rightAns;
    public RawImage updateImagePreview;

    [Header("Buttons")]
    public Button deletePreviewImageButton;
    public Button uploadPreviewImageButton;

    [Header("Transform")]
    public Transform level;

    [Header("Sprite")]
    public Sprite squre;
    public SpriteRenderer renderer;
    public SpriteRenderer targetSprite;

    [Header("Root_Class")]
    public Root root;

    [Header("Bool")]
    public bool isCameraMove = false;
    public bool isOpenPanel = false;

    [Header("TMP_InputField")]
    public TMP_InputField imageTitleInput;
    public TMP_InputField hintInput;

    [Header("Canvas")]
    public Canvas canvas;

    [Header("Camera")]
    public Camera camera;

    private byte[] imageFile;

    [Header("URl")]
    private string uploadPhotoUrl = "";

    public static GameManager Instance;

    public void Awake()
    {
        Debug.Log("Awake");
        Instance = this;
    }

    public void Start()
    {
        Debug.Log("Start");
        uploadPhotoUrl = ApiDataCall.Instance.baseUrl + "user/uploadPhoto";

        ActivePanel(submitPanel.name);
        StartCoroutine(GetData());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (submitPanel.activeInHierarchy)
            {
                imageTitleInput.text = "";
                hintInput.text = "";
                SceneManager.LoadScene(1);
            }
            else if (createLevelPanel.activeInHierarchy && !canvas.isActiveAndEnabled)
            {
                camera.orthographicSize =  13;
                ActivePanel(submitPanel.name);
                level.gameObject.SetActive(false);
                //level.GetComponent<SpriteRenderer>().sprite = null;
                level.GetComponent<TapCreateSpriteLevel>().isAnsSpriteAvailable = false;
                level.GetComponent<TapCreateSpriteLevel>().isCreated = false;

                if (level.transform.childCount == 1)
                {
                    Destroy(level.transform.GetChild(0).gameObject);
                }
            }
        }
    }

    public void Save()
    {
        Debug.Log("Save");
        rightAns = level.transform.GetChild(0).gameObject;
        PlayerPrefs.SetFloat("xPos",      rightAns.transform.position.x);
        PlayerPrefs.SetFloat("yPos",      rightAns.transform.position.y);
        PlayerPrefs.SetFloat("xHeight", rightAns.transform.localScale.x);
        PlayerPrefs.SetFloat("yHeight", rightAns.transform.localScale.y);
    }

    public void Get()
    {
        Debug.Log("get");
        xPos =      PlayerPrefs.GetFloat("xPos");
        yPos =    PlayerPrefs.GetFloat("yPos");
        xHeight = PlayerPrefs.GetFloat("xHeight");
        yHeight = PlayerPrefs.GetFloat("yHeight");

        GameObject newObject = new GameObject("RightAns");
        newObject.transform.SetParent(level);
        newObject.transform.position =   new Vector2(xPos, yPos);
        newObject.transform.localScale = new Vector2(xHeight, yHeight);
        newObject.AddComponent<BoxCollider2D>();

        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite =   squre;
        spriteRenderer.sortingOrder = 2;
    }

    public void SubmitPanelBackBtnClick()
    {
        Debug.Log("sumit panel back button click");
        imageTitleInput.text = "";
        hintInput.text = "";
        updateImagePreview.texture = null;
        deletePreviewImageButton.gameObject.SetActive(false);
        uploadPreviewImageButton.gameObject.SetActive(true) ;
        SceneManager.LoadScene(1);
    }

    public void SelectPointButtonClick()
    {
        Debug.Log("Select point butotn click");
        if (level.GetComponent<SpriteRenderer>().sprite != null)
        {
            level.gameObject.SetActive(true);
            FitCameraToSprite();
            ActivePanel(createLevelPanel.name);
        }
    }

    void FitCameraToSprite()
    {
        Debug.Log("Fit camera to sprite");
        if (targetSprite == null || camera == null) return;

        Bounds spriteBounds = targetSprite.bounds;

        float spriteHeight = spriteBounds.size.y;
        float spriteWidth =  spriteBounds.size.x;

        float screenRatio = (float)Screen.width / Screen.height;
        float spriteRatio = spriteWidth / spriteHeight;

        if (screenRatio >= spriteRatio)
        {
            camera.orthographicSize = spriteHeight / 2;
        }
        else
        {
            camera.orthographicSize = (spriteWidth / 2) / screenRatio;
        }
    }

    public void CreateLevelImageBackButtonClick()
    {
        Debug.Log("Create level image back buton click");
        camera.orthographicSize = 13;
        level.gameObject.SetActive(false);
        ActivePanel(submitPanel.name);
        //level.GetComponent<SpriteRenderer>().sprite = null;
        //level.GetComponent<TapCreateSpriteLevel>().isAnsSpriteAvailable = false;
        //level.GetComponent<TapCreateSpriteLevel>().isCreated = false;

        //if (level.transform.childCount == 1)
        //{
        //    Destroy(level.transform.GetChild(0).gameObject);
        //}
    }

    public void ContinueButtonClick()
    {
        Debug.Log("Continue button click");
        AddImage();
    }

    public void AddImage()
    {
        Debug.Log("Add image");
        string xPos = TapCreateSpriteLevel.Instance.xPos.ToString();
        string yPos = TapCreateSpriteLevel.Instance.yPos.ToString();
        string xScale = TapCreateSpriteLevel.Instance.xScale.ToString();
        string yScale = TapCreateSpriteLevel.Instance.yScale.ToString();

        StartCoroutine(AddImageRoutine(imageFile, imageTitleInput.text, hintInput.text, xPos, yPos, xScale, yScale));
    }

    public IEnumerator AddImageRoutine(byte[] photo, string title, string hint, string xPos, string yPos, string xScale, string yScale)
    {
        Debug.Log("Add Image routine");
        WWWForm form = new WWWForm();
        form.AddBinaryData("photo", photo, "photo.png", "image/png");
        form.AddField("title", title);
        form.AddField("hint", hint);
        form.AddField("xPos", xPos);
        form.AddField("yPos", yPos);
        form.AddField("xScale", xScale);
        form.AddField("yScale", yScale);

        if(!level.GetComponent<TapCreateSpriteLevel>().isAnsSpriteAvailable)
        {
            DialogCanvas.Instance.ShowFailedDialog("Please submit the right answer");
        }
        else
        {

            using (UnityWebRequest request = UnityWebRequest.Post(uploadPhotoUrl, form))
            {
                request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
                yield return request.SendWebRequest();

                // Check response
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("POST request successful: " + request.downloadHandler.text);
                    string Json = request.downloadHandler.text;
                    SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);

                    int ResponseCode = status["ResponseCode"];
                    Debug.Log(ResponseCode);

                    if (level.transform.childCount == 0)
                    {
                        level.GetComponent<TapCreateSpriteLevel>().enabled = false;
                        level.GetComponent<BoxCollider2D>().enabled = false;
                        canvas.gameObject.SetActive(true);
                        updateImagePreview.texture = null;
                        deletePreviewImageButton.gameObject.SetActive(false);
                        uploadPreviewImageButton.gameObject.SetActive(true) ;
                    }
                    else if (level.transform.childCount == 1)
                    {
                        level.GetComponent<TapCreateSpriteLevel>().enabled = false;
                        level.GetComponent<BoxCollider2D>().enabled = false;
                        level.transform.GetChild(0).GetComponent<DragCornerHandles>().enabled = false;
                        level.transform.GetChild(0).GetComponent<DragSprite>().enabled = false;
                        canvas.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogError("POST request failed!");
                    Debug.LogError("Error: " + request.error);
                    Debug.LogError("Response Code: " + request.responseCode);
                    Debug.LogError("Response Text: " + request.downloadHandler.text);

                    string Json = request.downloadHandler.text;
                    SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);
                    DialogCanvas.Instance.ShowFailedDialog(status["message"]);
                }
            }
        }
}

    public void OkButtonClick()
    {
        Debug.Log("Ok button click");
        imageTitleInput.text = "";
        hintInput.text = "";
        SceneManager.LoadScene(1);
    }

    public IEnumerator GetData()
    {
        Debug.Log("Get data");
        yield return new WaitForSeconds(0);
        //WWWForm form = new WWWForm();
        //UnityWebRequest www = UnityWebRequest.Get("https://api.jsonbin.io/v3/qs/6762b7e8acd3cb34a8bbb02b");

        //yield return www.SendWebRequest();

        //if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    Debug.Log(www.downloadHandler.text);

        //    root = JsonUtility.FromJson<Root>(www.downloadHandler.text);

        //    Debug.Log(root);
        //    Debug.Log(root.record.x);
        //    Debug.Log(root.record.y);
        //    Debug.Log(root.record.w);
        //    Debug.Log(root.record.h);

        //    xPos = root.record.x;
        //    yPos = root.record.y;
        //    xHeight = root.record.w;
        //    yHeight = root.record.h;

        //    GameObject newObject = new GameObject("RightAns");
        //    newObject.transform.SetParent(level);
        //    newObject.transform.position = Vector3.zero;
        //    newObject.transform.localPosition = new Vector2(xPos, yPos);
        //    newObject.transform.localScale = new Vector2(xHeight, yHeight);
        //    newObject.AddComponent<BoxCollider2D>();

        //    SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        //    spriteRenderer.sprite = squre;
        //    spriteRenderer.sortingOrder = 2;
        //}
    }

    public void SubmitImageUploadButtonClick()
    {
        Debug.Log("Submit image upload butotn click");
        PickImages();
    }

    public void PickImages()
    {
        Debug.Log("Pick Image");
        PickImage(2048);
    }

    public void PickImage(int maxSize)
    {
        Debug.Log("Pick Image");
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                updateImagePreview.texture = texture;

                uploadPreviewImageButton.gameObject.SetActive(false);
                deletePreviewImageButton.gameObject.SetActive(true) ;

                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                renderer.sprite = sprite;

                imageFile = ConvertSpriteToBytes(sprite);
            }
        });
    }

    public void DeletePreviewImage()
    {
        Debug.Log("Delete Preview Image");
        updateImagePreview.texture = null;
        renderer.sprite = null;
        uploadPreviewImageButton.gameObject.SetActive(true);
        deletePreviewImageButton.gameObject.SetActive(false);
    }

    public void ActivePanel(string panel)
    {
        Debug.Log("Active panel");
        submitPanel.SetActive(panel.Equals(submitPanel.name));
        createLevelPanel.SetActive(panel.Equals(createLevelPanel.name));
    }

    byte[] ConvertSpriteToBytes(Texture texture)
    {
        Debug.Log("Convert sprite to bytes");
        if (texture == null)
        {
            return null;
        }

        Texture2D texture2D = TextureToTexture2D(texture);

        return texture2D.EncodeToPNG();
    }

    byte[] ConvertSpriteToBytes(Sprite sprite)
    {
        Debug.Log("Convert sprite to bytes");
        if (sprite == null)
        {
            return null;
        }


        // Convert Sprite to Texture2D
        Texture2D texture = SpriteToTexture2D(sprite);

        // Encode to PNG (You can also use EncodeToJPG())
        return texture.EncodeToPNG();
    }

    Texture2D SpriteToTexture2D(Sprite sprite)
    {
        Debug.Log("Sprite to texture 2d");
        if (sprite == null)
        {
            return null;
        }

        // Create a new Texture2D with the same dimensions as the sprite
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);

        // Get the original sprite texture
        Texture2D originalTexture = sprite.texture;

        // Create a RenderTexture
        RenderTexture renderTexture = RenderTexture.GetTemporary(originalTexture.width, originalTexture.height);

        // Copy the original texture to the RenderTexture
        Graphics.Blit(originalTexture, renderTexture);

        // Store the previous active RenderTexture
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Read pixels from the RenderTexture
        texture.ReadPixels(new Rect(sprite.rect.x, sprite.rect.y, sprite.rect.width, sprite.rect.height), 0, 0);
        texture.Apply();

        // Restore the previous RenderTexture and clean up
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture;
    }

    Texture2D TextureToTexture2D(Texture texture)
    {
        Debug.Log("Textrute to texture 2d");
        if (texture == null)
        {
            return null;
        }

        // Create a new Texture2D with the same dimensions as the input texture
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        // Create a RenderTexture
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height);

        // Copy the input texture to the RenderTexture
        Graphics.Blit(texture, renderTexture);

        // Store the previous active RenderTexture
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Read pixels from the RenderTexture
        texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture2D.Apply();

        // Restore the previous RenderTexture and clean up
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }
}

[System.Serializable]
public class Metadata
{
    public string name;
    public int readCountRemaining;
    public int timeToExpire;
    public DateTime createdAt;
}

[System.Serializable]
public class Record
{
    public float x;
    public float y;
    public float w;
    public float h;
}

[System.Serializable]
public class Root
{
    public string id;
    public Record record;
    public Metadata metadata;
}