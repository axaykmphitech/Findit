using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    public static GameManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
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
                camera.orthographicSize = 13;
                ActivePanel(submitPanel.name);
                level.gameObject.SetActive(false);
                level.GetComponent<SpriteRenderer>().sprite = null;
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
        rightAns = level.transform.GetChild(0).gameObject;
        PlayerPrefs.SetFloat("xPos", rightAns.transform.position.x);
        PlayerPrefs.SetFloat("yPos", rightAns.transform.position.y);
        PlayerPrefs.SetFloat("xHeight", rightAns.transform.localScale.x);
        PlayerPrefs.SetFloat("yHeight", rightAns.transform.localScale.y);
    }

    public void Get()
    {
        xPos = PlayerPrefs.GetFloat("xPos");
        yPos = PlayerPrefs.GetFloat("yPos");
        xHeight = PlayerPrefs.GetFloat("xHeight");
        yHeight = PlayerPrefs.GetFloat("yHeight");

        GameObject newObject = new GameObject("RightAns");
        newObject.transform.SetParent(level);
        newObject.transform.position = new Vector2(xPos, yPos);
        newObject.transform.localScale = new Vector2(xHeight, yHeight);
        newObject.AddComponent<BoxCollider2D>();

        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = squre;
        spriteRenderer.sortingOrder = 2;
    }

    public void SubmitPanelBackBtnClick()
    {
        imageTitleInput.text = "";
        hintInput.text = "";
        SceneManager.LoadScene(1);
    }

    public void SelectPointButtonClick()
    {
        if (level.GetComponent<SpriteRenderer>().sprite != null)
        {
            level.gameObject.SetActive(true);
            FitCameraToSprite();
            ActivePanel(createLevelPanel.name);
        }
    }

    void FitCameraToSprite()
    {
        if (targetSprite == null || camera == null) return;

        Bounds spriteBounds = targetSprite.bounds;

        float spriteHeight = spriteBounds.size.y;
        float spriteWidth = spriteBounds.size.x;

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
        camera.orthographicSize = 13;
        level.gameObject.SetActive(false);
        ActivePanel(submitPanel.name);
        level.GetComponent<SpriteRenderer>().sprite = null;
        level.GetComponent<TapCreateSpriteLevel>().isAnsSpriteAvailable = false;
        level.GetComponent<TapCreateSpriteLevel>().isCreated = false;

        if (level.transform.childCount == 1)
        {
            Destroy(level.transform.GetChild(0).gameObject);
        }
    }

    public void ContinueButtonClick()
    {
        if (level.transform.childCount == 0)
        {
            level.GetComponent<TapCreateSpriteLevel>().enabled = false;
            level.GetComponent<BoxCollider2D>().enabled = false;
            canvas.gameObject.SetActive(true);
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

    public void OkButtonClick()
    {
        imageTitleInput.text = "";
        hintInput.text = "";
        SceneManager.LoadScene(1);
    }

    public IEnumerator GetData()
    {
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

    public void PickImages()
    {
        PickImage(2048);
    }

    public void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);

                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                renderer.sprite = sprite;
            }
        });
        Debug.Log("Permission result: " + permission);
    }

    public void ActivePanel(string panel)
    {
        submitPanel.SetActive(panel.Equals(submitPanel.name));
        createLevelPanel.SetActive(panel.Equals(createLevelPanel.name));
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