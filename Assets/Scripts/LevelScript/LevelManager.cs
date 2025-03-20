using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

public class LevelManager : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject levelPanel;
    public GameObject tapPanel;
    public GameObject mapsPanel;
    public GameObject settingPanel;
    public GameObject myProfilePanel;
    public GameObject editProfilePanel;
    public GameObject editPictureProfilePanel;
    public GameObject allImagePanel;
    public GameObject allImagedetailPanel;
    public GameObject changePasswordPanel;
    public GameObject contactUsPanel;
    public GameObject deleteAccountPanel;
    public GameObject deleteAccountPopUpPanel;
    public GameObject aboutsPanel;
    public GameObject privacyPolicyPanel;
    public GameObject termsandConditionsPanel;
    public GameObject signOutPanel;
    public GameObject subscriptionPanel;
    public GameObject content;
    public GameObject levelPrefab;
    public GameObject otherReasonObject;
    public GameObject buttonObject;
    public GameObject editProfileName;
    public GameObject editProfileEmail;
    public GameObject settingContent;
    public Transform userPhotoListContent;
    public GameObject userPhotoItemPrefab;

    [Header("Int & Float")]
    public int levelNumber = 0;
    private float lastClickTime = 0f;
    private float doubleClickTime = 0.3f;

    [Header("Bool")]
    public bool isPanelOpen = false;
    public bool isCurrentPasswordHide = false;
    public bool isNewPasswordHide = false;
    public bool isConfirmPasswordHide = false;
    public bool isEditPictureProfilePanel = false;
    public bool isAllImagedetailPanel = false;
    public bool isDeleteAccountPopUpPanel = false;
    public bool isSignOutPanel = false;
    public bool isSubscriptionPanelClick = false;

    [Header("Image & Sprite")]
    public Image pictureImage;
    public Sprite on;
    public Sprite off;
    public Sprite deletNormalButton;
    public Sprite deletSelectButton;
    public Sprite highlight;

    public Image settingPictureImage;
    public Image myProfilePictureImage;

    [Header("Button")]
    public Button currentPasswordHideButton;
    public Button newPasswordHideButton;
    public Button confirmPasswordHideButton;
    public Button dOption1;
    public Button dOption2;
    public Button dOption3;
    public Button dOption4;
    public Button dOption5;

    [Header("InputField")]
    public TMP_InputField currentPasswordInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField editProfileNameInput;
    public TMP_InputField editProfileEmailInput;
    public TMP_InputField contactusUsernameInput;
    public TMP_InputField contactusEmailInput;
    public TMP_InputField contactusSubjectInput;
    public TMP_InputField contactusMessageInput;
    public TMP_InputField ohterReasonInput;

    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI myProfileEmailInput;
    public TextMeshProUGUI myProfileUserInput;
    public TextMeshProUGUI settingUserInput;
    public TextMeshProUGUI settingEmailInput;
    public TextMeshProUGUI[] totalStars;
    public TextMeshProUGUI[] todayStars;

    [Header("Url")]
    private string contactUsUrl      = "";
    private string deleteAccountUrl  = "";
    private string changePasswordUrl = "";
    private string updateProfileUrl  = "";
    private string userPhotoList     = "";
    private string getiImageByDate   = "";
    private string logOutUrl         = "";
    private string userPhotoHistory  = ""; 

    public byte[] imageFile;
    private string deleteReason;

    UJsonResponse uJsonResponse;

    public TextMeshProUGUI dateText;
    private int cM;
    private int cY;
    
    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        contactUsUrl = ApiDataCall.Instance.baseUrl + "user/contactUs";
        deleteAccountUrl = ApiDataCall.Instance.baseUrl + "user/deleteAccount";
        changePasswordUrl = ApiDataCall.Instance.baseUrl + "user/changePassword";
        updateProfileUrl = ApiDataCall.Instance.baseUrl + "user/updateProfile";
        userPhotoList = ApiDataCall.Instance.baseUrl + "user/userPhotosList";
        getiImageByDate = ApiDataCall.Instance.baseUrl + "user/getImagesByDate";
        logOutUrl = ApiDataCall.Instance.baseUrl + "user/logOut";
        userPhotoHistory = ApiDataCall.Instance.baseUrl + "user/getUserHistory";


        isCurrentPasswordHide = true;
        isNewPasswordHide = true;
        isConfirmPasswordHide = true;
        PanelActive(levelPanel.name);

        myProfileEmailInput.text = ApiDataCall.Instance.email;
        settingEmailInput.text =   ApiDataCall.Instance.email;
        editProfileEmailInput.text = ApiDataCall.Instance.email;
        editProfileNameInput.text = ApiDataCall.Instance.userName;

        string imageUrl = ApiDataCall.Instance.profile;
        StartCoroutine(LoadImageFromURL(imageUrl));

        myProfileUserInput.text = ApiDataCall.Instance.userName;
        settingUserInput.text =   ApiDataCall.Instance.userName;
        SetStars();
    }

    IEnumerator LoadImageFromURL(string url)
    {
        Debug.Log("load iamge from url");
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Texture2DToSprite(texture);
                settingPictureImage.sprite = sprite; // Set the sprite to the UI Image
                myProfilePictureImage.sprite = sprite;
                pictureImage.sprite = sprite;// Set the sprite to the UI Image
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                //string Json = request.downloadHandler.text;
                //SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);
                //DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
        }
    }

    IEnumerator LoadImageFromURL(Image image ,string url)
    {
        Debug.Log("load iamge form url");
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Texture2DToSprite(texture);
                image.sprite = sprite;
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode statuss = SimpleJSON.JSON.Parse(Json);
                DialogCanvas.Instance.ShowFailedDialog(statuss["message"]);
            }
        }
    }

    Sprite Texture2DToSprite(Texture2D texture)
    {
        Debug.Log("texture 2d sprite");
        if (texture == null)
        {
            Debug.LogError("Texture is null!");
            return null;
        }

        // Create a new sprite with full texture dimensions
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mapsPanel.activeInHierarchy)
            {
                PanelActive(levelPanel.name);
            }
            else if (settingPanel.activeInHierarchy && !isSignOutPanel)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(levelPanel.name);
            }
            else if (myProfilePanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
            }
            else if (editProfilePanel.activeInHierarchy && !isEditPictureProfilePanel)
            {
                PanelActive(myProfilePanel.name);
                editProfileNameInput.text = "";
                editProfileEmailInput.text = "";
            }
            else if (editPictureProfilePanel.activeInHierarchy && isEditPictureProfilePanel)
            {
                PanelActive(editProfilePanel.name);
                isEditPictureProfilePanel = false;
            }
            else if (allImagePanel.activeInHierarchy && !isAllImagedetailPanel)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
            }
            else if (allImagedetailPanel.activeInHierarchy && isAllImagedetailPanel)
            {
                PanelActive(allImagePanel.name);
                isAllImagedetailPanel = false;
            }
            else if (changePasswordPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
                currentPasswordInput.text = "";
                newPasswordInput.text = "";
                confirmPasswordInput.text = "";
            }
            else if (contactUsPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
                contactusUsernameInput.text = "";
                contactusEmailInput.text = "";
                contactusSubjectInput.text = "";
                contactusMessageInput.text = "";
            }
            else if (deleteAccountPanel.activeInHierarchy && !isDeleteAccountPopUpPanel)
            {
                if (buttonObject.activeInHierarchy)
                {
                    ButtonColor();
                    PanelActive(settingPanel.name);
                    ohterReasonInput.text = "";
                    settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                }
                else if (otherReasonObject.activeInHierarchy)
                {
                    ButtonColor();
                    ohterReasonInput.text = "";
                    buttonObject.SetActive(true);
                    otherReasonObject.SetActive(false);
                }
            }
            else if (aboutsPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
            }
            else if (privacyPolicyPanel.activeInHierarchy)
            {
                if (isSubscriptionPanelClick)
                {
                    PanelActive(subscriptionPanel.name);
                    isSubscriptionPanelClick = false;
                }
                else if (!isSubscriptionPanelClick)
                {
                    settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                    PanelActive(settingPanel.name);
                }
            }
            else if (termsandConditionsPanel.activeInHierarchy)
            {
                if (isSubscriptionPanelClick)
                {
                    PanelActive(subscriptionPanel.name);
                    isSubscriptionPanelClick = false;
                }
                else if (!isSubscriptionPanelClick)
                {
                    settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                    PanelActive(settingPanel.name);
                }
            }
            else if (signOutPanel.activeInHierarchy && isSignOutPanel)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
                isSignOutPanel = false;
            }
            else if (subscriptionPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
            }
            else if (deleteAccountPopUpPanel.activeInHierarchy)
            {
                deleteAccountPopUpPanel.SetActive(false);
                isDeleteAccountPopUpPanel = false;
            }
        }

        //if (editProfilePanel.activeInHierarchy)
        //{
        //    if (editProfileNameInput.isFocused)
        //    {
        //        editProfileName.GetComponent<Image>().sprite = highlight;
        //        editProfileEmail.GetComponent<Image>().sprite = deletNormalButton;
        //    }
        //    else if (editProfileEmailInput.isFocused)
        //    {
        //        editProfileEmail.GetComponent<Image>().sprite = highlight;
        //        editProfileName.GetComponent<Image>().sprite = deletNormalButton;
        //    }
        //}
    }

    public void PlayTodaysPhotosButtonClick()
    {
        if (ApiDataCall.Instance.toDayPoint <= 0)
        {
            if (ApiDataCall.Instance.totalPoint >= 3)
            {
                ApiDataCall.Instance.toDayPoint = 3;
                ApiDataCall.Instance.totalPoint -= 3;
                Debug.Log("play todays photos burron click");
                StartCoroutine(GetImageByDateRoutine());
            }
            else
            {
                DialogCanvas.Instance.ShowFailedDialog("Insufficient points in your account");
            }
        }
        else
        {
            Debug.Log("play todays photos burron click");
            StartCoroutine(GetImageByDateRoutine());
        }

    }

    public IEnumerator GetImageByDateRoutine()
    {
        Debug.Log("get iamge by date routine");
        WWWForm form = new WWWForm();

        DateTime todayDate = DateTime.Today;
        string formattedDate = todayDate.ToString("yyyy-MM-dd");

        form.AddField("date", formattedDate);

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(getiImageByDate, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("POST request successful: " + request.downloadHandler.text);
                PanelActive(tapPanel.name);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);

                Debug.Log(status["data"]["findImg"]["title"]);
                Debug.Log(status["data"]["findImg"]["photo"]);
                Debug.Log(status["data"]["findImg"]["hint"]) ;
                Debug.Log(status["data"]["findImg"]["xPos"]) ;
                Debug.Log(status["data"]["findImg"]["xPos"]) ; 

                ApiDataCall.Instance.title  =    status["data"]["findImg"]["title"];
                ApiDataCall.Instance.photo  =    status["data"]["findImg"]["photo"];
                ApiDataCall.Instance.hint   =     status["data"]["findImg"]["hint"];
                ApiDataCall.Instance.xPos   =     status["data"]["findImg"]["xPos"];
                ApiDataCall.Instance.yPos   =     status["data"]["findImg"]["yPos"];
                ApiDataCall.Instance.xScale =   status["data"]["findImg"]["xScale"];
                ApiDataCall.Instance.yScale =   status["data"]["findImg"]["yScale"];
                ApiDataCall.Instance.id     =       status["data"]["findImg"]["id"];
                
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

    public IEnumerator GetImageByDateRoutine(string formattedDate)
    {
        Debug.Log("get iamge by date routine");
        WWWForm form = new WWWForm();

        form.AddField("date", formattedDate);
        Debug.Log(formattedDate);

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(getiImageByDate, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("POST request successful: " + request.downloadHandler.text);
                PanelActive(tapPanel.name);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);

                Debug.Log(status["data"]["findImg"]["title"]);
                Debug.Log(status["data"]["findImg"]["photo"]);
                Debug.Log(status["data"]["findImg"]["hint"]);
                Debug.Log(status["data"]["findImg"]["xPos"]);
                Debug.Log(status["data"]["findImg"]["xPos"]);

                ApiDataCall.Instance.title = status["data"]["findImg"]["title"];
                ApiDataCall.Instance.photo = status["data"]["findImg"]["photo"];
                ApiDataCall.Instance.hint = status["data"]["findImg"]["hint"];
                ApiDataCall.Instance.xPos = status["data"]["findImg"]["xPos"];
                ApiDataCall.Instance.yPos = status["data"]["findImg"]["yPos"];
                ApiDataCall.Instance.xScale = status["data"]["findImg"]["xScale"];
                ApiDataCall.Instance.yScale = status["data"]["findImg"]["yScale"];
                ApiDataCall.Instance.id = status["data"]["findImg"]["id"];

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

    public void playPastButtonClick()
    {
        string currentMonth = DateTime.Now.ToString("MM");
        string currentYear  = DateTime.Now.ToString("yyyy");
        cM = int.Parse(currentMonth);
        cY = int.Parse(currentYear);
        StartCoroutine(PastPhotoesRoutine(currentMonth, currentYear));
    }

    public static string GetMonthName(string monthNumber)
    {
        if (int.TryParse(monthNumber, out int month) && month >= 1 && month <= 12)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }
        return "Invalid Month";
    }

    public void PreviousMonthButton()
    {
        cM--;
        if(cM < 1)
        {
            cY--;
            cM = 12;
        }
        StartCoroutine(PastPhotoesRoutine(cM.ToString(), cY.ToString()));
    }

    public void NextMonthButton()
    {
        cM++;
        if (cM > 12)
        {
            cY++;
            cM = 1;
        }
        StartCoroutine(PastPhotoesRoutine(cM.ToString(), cY.ToString()));
    }

    public IEnumerator PastPhotoesRoutine(string currentMonth, string currentYear)
    {
        WWWForm form = new WWWForm();

        form.AddField("month", currentMonth);
        form.AddField("year",  currentYear);

        string dateCombo = GetMonthName(currentMonth) + " "+ currentYear;
        dateText.text = dateCombo;

        using (UnityWebRequest request = UnityWebRequest.Post(userPhotoHistory, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                PanelActive(mapsPanel.name);
                LevelSet();

                Debug.Log(request.downloadHandler.text);
                StarResponse response = JsonConvert.DeserializeObject<StarResponse>(request.downloadHandler.text);
                Debug.Log(response.Data.Count);
                Debug.Log(response.Data.Count);

                foreach (Transform item in content.transform)
                {
                    Destroy(item.gameObject);
                }

                for (int i = 0; i < response.Data.Count; i++)
                {
                    Debug.Log("response" + response.Data.Count);
                    GameObject levelItem = Instantiate(levelPrefab, content.transform);
                    DateTime dateObject = DateTime.Parse(response.Data[i].Date);
                    string day = dateObject.Day.ToString();
                    levelItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = day;
                    int stars = int.Parse(response.Data[i].UserStar);
                    if(stars == 3)
                    {
                        levelItem.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
                        levelItem.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(true);
                        levelItem.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);
                    }
                    else if(stars == 2)
                    {
                        levelItem.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                        levelItem.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(true) ;
                        levelItem.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true) ;
                    }
                    else if(stars == 1)
                    {
                        levelItem.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                        levelItem.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
                        levelItem.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);
                    }
                    else if(stars == 0)
                    {
                        levelItem.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                        levelItem.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(false);
                        levelItem.transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(false);
                    }

                    Debug.Log(response.Data[i].Date);
                    int index = i;
                    string date = response.Data[index].Date;
                    levelItem.GetComponent<Button>().onClick.AddListener(() => LevelButton(date));
                    Debug.Log("response" + response.Data[i].Date);
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

    private void LevelButton(string date)
    {
        if (ApiDataCall.Instance.toDayPoint > 0)
        {
            Debug.Log(date);
            StartCoroutine(GetImageByDateRoutine(date));
        }
        else
        {
            DialogCanvas.Instance.ShowFailedDialog("Insufficient points in your account");
        }
    }

    public void SubmitPhotoBtnClick()
    {
        Debug.Log("submit photo btn click");
        SceneManager.LoadScene(3);
    }

    public void SettingButtonClick()
    {
        Debug.Log("setting button click");
        PanelActive(settingPanel.name);
    }

    public void BackMapsBtnClick()
    {
        PanelActive(levelPanel.name);
    }

    public void SettingBackButtonClick()
    {
        Debug.Log("settings back button lcik");
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(levelPanel.name);
    }

    public void MyProfileButtonClick()
    {
        Debug.Log("my profile button clikc");
        PanelActive(myProfilePanel.name);
    }

    public void AllImagesButtonClick()
    {
        Debug.Log("all image button lcik");
        PanelActive(allImagePanel.name);
        StartCoroutine(UserPhotoList());
    }

    public IEnumerator UserPhotoList()
    {
        Debug.Log("user photo list");
        WWWForm form = new WWWForm();
        form.AddField("skip" ,  0);
        form.AddField("limit", 10);

        using (UnityWebRequest request = UnityWebRequest.Post(userPhotoList, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("photlist " + request.downloadHandler.text);
                string Json = request.downloadHandler.text;
                uJsonResponse = JsonConvert.DeserializeObject<UJsonResponse>(Json);

                Debug.Log(Json);

                if (uJsonResponse != null)
                {

                    foreach (Transform item in userPhotoListContent)
                    {
                        Destroy(item.gameObject);
                    }
                    for (int i = 0; i < uJsonResponse.Data.imagesList.Count; i++)
                    {
                        ImageData image = uJsonResponse.Data.imagesList[i];

                        Debug.Log(image.title + " " + image.date);

                        GameObject imageItem = Instantiate(userPhotoItemPrefab, userPhotoListContent);
                        imageItem.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = image.title;

                        string date = image.date;
                        if (date == "" || date.ToLower() == "invalid date")
                            date = "No date assigned";

                        imageItem.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = date;

                        if (image.status == "accepted")
                        {
                            Debug.Log("approved");
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(true) ;
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(3).gameObject.SetActive(false);
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(false);
                        }

                        if (image.status == "pending")
                        {
                            Debug.Log("pending");
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(true) ;
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(3).gameObject.SetActive(false);
                        }

                        if (image.status == "rejected")
                        {
                            Debug.Log("rejected");
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(false);
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);
                            imageItem.transform.GetChild(0).GetChild(1).GetChild(3).gameObject.SetActive(true) ;
                        }

                        StartCoroutine(LoadImageFromURL(imageItem.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>(), image.photo));
                    }

                    ///image 000
                    ///title 0010
                    ///date 00111
                    ///approved 0012
                    ///rejected 0013
                    ///pending  0014
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

    public void AllImagesBackButtonClick()
    {
        Debug.Log("allimage back button click");
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
    }

    public void AllImagesDetailsButtonClick()
    {
        Debug.Log("all iamge details button clik");
        allImagedetailPanel.SetActive(true);
        isAllImagedetailPanel = true;
    }

    public void AllImagedetailCancelButtonClick()
    {
        Debug.Log("all image detail cancle button lcick");
        allImagedetailPanel.SetActive(false);
        isAllImagedetailPanel = false;
    }

    public void SubscriptionButtonClick()
    {
        Debug.Log("subscription button click");
        PanelActive(subscriptionPanel.name);
    }

    public void SubscriptionBackButtonClick()
    {
        Debug.Log("subscripn back button click");
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
    }

    public void SubscriptionTermsandConditionsButtonClick()
    {
        Debug.Log("subscription termsandcondition button click");
        isSubscriptionPanelClick = true;
        PanelActive(termsandConditionsPanel.name);
    }

    public void SubscriptionprivacyPolicyButtonClick()
    {
        Debug.Log("subscription privacy policy button click");
        isSubscriptionPanelClick = true;
        PanelActive(privacyPolicyPanel.name);
    }

    public void SubscribeButtonClick()
    {
        Debug.Log("subscribe button click");
        PanelActive(settingPanel.name);
    }

    public void ChangePasswordButtonClick()
    {
        Debug.Log("change password button click");
        PanelActive(changePasswordPanel.name);
    }

    public void AboutUsButtonClick()
    {
        Debug.Log("about us button clilck");
        PanelActive(aboutsPanel.name);
    }

    public void PrivacyPolicyButtonClick()
    {
        Debug.Log("privacy policy button lcick");
        PanelActive(privacyPolicyPanel.name);
    }

    public void ContactUsButtonClick()
    {
        Debug.Log("contact us button click");
        PanelActive(contactUsPanel.name);
    }

    public void TermsandConditionsButtonClick()
    {
        Debug.Log("termsand condition button click");
        PanelActive(termsandConditionsPanel.name);
    }

    public void DeleteAccountButtonClick()
    {
        Debug.Log("delete account button click");
        PanelActive(deleteAccountPanel.name);
    }

    public void SignOutButtonClick()
    {
        Debug.Log("signoutbutton click");
        signOutPanel.SetActive(true);
        isSignOutPanel = true;
    }

    public void SignOutYesButtonClick()
    {
        Debug.Log("signout yes button click");
        StartCoroutine(LogOutRoutine());
    }

    public IEnumerator LogOutRoutine()
    {
        Debug.Log("logout routine");
        WWWForm form = new WWWForm();

        using (UnityWebRequest request = UnityWebRequest.Post(logOutUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Lgout " + request.downloadHandler.text);
                SceneManager.LoadScene(0);
                isSignOutPanel = false;
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

    public void SignOutNoButtonClick()
    {
        Debug.Log("sign out no button click");
        signOutPanel.SetActive(false);
        isSignOutPanel = false;
    }

    public void MyProfileBackButtonClick()
    {
        Debug.Log("my profile back button lcick");
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
    }

    public void EditProfileBackButtonClick()
    {
        Debug.Log("edit profile back button click");
        PanelActive(myProfilePanel.name);
        //editProfileNameInput.text = "";
        //editProfileEmailInput.text = "";
    }

    public void EditButtonClick()
    {
        Debug.Log("edit button click");
        PanelActive(editProfilePanel.name);
    }

    public void UpdateProfileButtonClick()
    {
        Debug.Log("update profile butotn");
        StartCoroutine(UpdateProfileCheck(editProfileNameInput.text, imageFile));
    }

    public IEnumerator UpdateProfileCheck(string userName, byte[] profile)
    {
        Debug.Log("update profile check");

        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddBinaryData("profile", profile, "profileImage", "image/png");

        Debug.Log(userName + " userName");
        Debug.Log(profile + " profile");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(updateProfileUrl, form))
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


                ApiDataCall.Instance.userName = userName;
                myProfileUserInput.text = ApiDataCall.Instance.userName;
                settingUserInput.text =   ApiDataCall.Instance.userName;
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name) ;
                editProfileNameInput.text =  "";
                editProfileEmailInput.text = "";

                StartCoroutine(LoadImageFromURL(settingPictureImage, status["data"]["profile"]));
                StartCoroutine(LoadImageFromURL(myProfilePictureImage, status["data"]["profile"]));
                StartCoroutine(LoadImageFromURL(pictureImage, status["data"]["profile"]));
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

    public void CameraButtonClick()
    {
        Debug.Log("camera button click");
        editPictureProfilePanel.SetActive(true);
        isEditPictureProfilePanel = true;
    }

    public void CameraChooseButtonClick()
    {
        Debug.Log("camera choose button lcick");
        editPictureProfilePanel.SetActive(false);
        isEditPictureProfilePanel = false;
        CameraManager.Instance.TakePicture(pictureImage);
    }

    public void GalleryButtonClick()
    {
        Debug.Log("gallery button lcik");
        editPictureProfilePanel.SetActive(false);
        isEditPictureProfilePanel = false;
        PickImages();
    }

    public void PickImages()
    {
        Debug.Log("pick images");
        PickImage(2048);
    }

    public void PickImage(int maxSize)
    {
        Debug.Log("pick image");
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

                pictureImage.sprite = sprite;
                imageFile = ConvertSpriteToBytes(sprite);
                Debug.Log(imageFile + " imageFile");

                if (imageFile == null)
                {
                    Debug.Log("ConvertSpriteToBytes returned null");
                }
                else
                {
                    Debug.Log("Image converted successfully!");
                    Debug.Log(imageFile + " imageFile");
                }
            }
        });
        Debug.Log("Permission result: " + permission);
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
        Debug.Log("sprite to texture 2d");
        if (sprite == null)
        {
            Debug.LogError("Sprite is null!");
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

    public void ChangePasswordBackButtonClick()
    {
        Debug.Log("change password back button lcick");
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
        currentPasswordInput.text = "";
        newPasswordInput.text = "";
        confirmPasswordInput.text = "";
    }

    public void UpdatePasswordButtonClick()
    {
        Debug.Log("update password button lcick");
        StartCoroutine(UpdatePasswordCheck(currentPasswordInput.text, newPasswordInput.text));
    }

    public IEnumerator UpdatePasswordCheck(string oldPassword, string newPassword)
    {
        Debug.Log("update password chceck");
        WWWForm form = new WWWForm();
        form.AddField("oldPassword", oldPassword);
        form.AddField("newPassword", newPassword);

        Debug.Log(oldPassword + " oldPassword");
        Debug.Log(newPassword + " newPassword");

        if(oldPassword.Length < 8 || newPassword.Length < 8 || confirmPasswordInput.text.Length < 8)
        {
            DialogCanvas.Instance.ShowFailedDialog("Password should be 8 character long");
        }
        else if(newPassword != confirmPasswordInput.text)
        {
            DialogCanvas.Instance.ShowFailedDialog("New password and confirm password doed not match");
        }
        else
        {
            // Create request
            using (UnityWebRequest request = UnityWebRequest.Post(changePasswordUrl, form))
            {
                request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
                Debug.Log(ApiDataCall.Instance.token);
                yield return request.SendWebRequest();

                // Check response
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                    PanelActive(settingPanel.name);
                    currentPasswordInput.text = "";
                    newPasswordInput.text = "";
                    confirmPasswordInput.text = "";
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

    public void CurrentPasswordHideButtonClick()
    {
        if (!isCurrentPasswordHide)
        {
            currentPasswordHideButton.GetComponent<Image>().sprite = off;
            currentPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isCurrentPasswordHide = true;
        }
        else if (isCurrentPasswordHide)
        {
            currentPasswordHideButton.GetComponent<Image>().sprite = on;
            currentPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isCurrentPasswordHide = false;
        }
        SignInManager.Instance.RefreshInputField(currentPasswordInput);
    }

    public void NewPasswordHideButtonClick()
    {
        if (!isNewPasswordHide)
        {
            newPasswordHideButton.GetComponent<Image>().sprite = off;
            newPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isNewPasswordHide = true;
        }
        else if (isNewPasswordHide)
        {
            newPasswordHideButton.GetComponent<Image>().sprite = on;
            newPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isNewPasswordHide = false;
        }
        SignInManager.Instance.RefreshInputField(newPasswordInput);
    }

    public void ConfirmPasswordHideButtonClick()
    {
        if (!isConfirmPasswordHide)
        {
            confirmPasswordHideButton.GetComponent<Image>().sprite = off;
            confirmPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isConfirmPasswordHide = true;
        }
        else if (isConfirmPasswordHide)
        {
            confirmPasswordHideButton.GetComponent<Image>().sprite = on;
            confirmPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isConfirmPasswordHide = false;
        }
        SignInManager.Instance.RefreshInputField(confirmPasswordInput);
    }

    public void AboutUsBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
    }

    public void PrivacyPolicyBackButtonClick()
    {
        if (isSubscriptionPanelClick)
        {
            PanelActive(subscriptionPanel.name);
            isSubscriptionPanelClick = false;
        }
        else if (!isSubscriptionPanelClick)
        {
            settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
            PanelActive(settingPanel.name);
        }
    }

    public void TermsandConditionsBackButtonClick()
    {
        if (isSubscriptionPanelClick)
        {
            PanelActive(subscriptionPanel.name);
            isSubscriptionPanelClick = false;
        }
        else if (!isSubscriptionPanelClick)
        {
            settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
            PanelActive(settingPanel.name);
        }
    }

    public void ContactUsBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
        contactusUsernameInput.text = "";
        contactusEmailInput.text =    "";
        contactusSubjectInput.text =  "";
        contactusMessageInput.text =  "";
    }

    public void ContactUsSubmitButtonClick()
    {
        StartCoroutine(contactUsCheck(contactusUsernameInput.text, contactusEmailInput.text, contactusSubjectInput.text, contactusMessageInput.text));
    }

    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9_+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    public IEnumerator contactUsCheck(string name, string email, string subject, string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("subject", subject);
        form.AddField("message", message);

        //Debug.Log(email + " email");
        //Debug.Log(name + " name");
        //Debug.Log(subject + " subject");
        //Debug.Log(message + " message");

        // Create request

        if (!IsValidEmail(email))
        {
            DialogCanvas.Instance.ShowFailedDialog("Please enter valid email address");
        }
        else
        {
            using (UnityWebRequest request = UnityWebRequest.Post(contactUsUrl, form))
            {
                yield return request.SendWebRequest();

                // Check response
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                    PanelActive(settingPanel.name);
                    contactusUsernameInput.text = "";
                    contactusEmailInput.text = "";
                    contactusSubjectInput.text = "";
                    contactusMessageInput.text = "";
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

    public void DeleteAccountBackButtonClick()
    {
        if (buttonObject.activeInHierarchy)
        {
            settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
            ButtonColor();
            PanelActive(settingPanel.name);
            ohterReasonInput.text = "";
        }
        else if (otherReasonObject.activeInHierarchy)
        {
            ButtonColor();
            ohterReasonInput.text = "";
            buttonObject.SetActive(true);
            otherReasonObject.SetActive(false);
        }
    }

    public void AccountDeletButtonClick()
    {
        deleteAccountPopUpPanel.SetActive(true);
        isDeleteAccountPopUpPanel = true;
    }

    public void DeleteButtonClick()
    {
        StartCoroutine(DeleteCheck(deleteReason));
    }

    public IEnumerator DeleteCheck(string deleteReason)
    {
        if(deleteReason == null)
        {
            deleteReason = ohterReasonInput.text;
        }

        WWWForm form = new WWWForm();
        form.AddField("deleteReason", deleteReason);
        
        using (UnityWebRequest request = UnityWebRequest.Post(deleteAccountUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                isDeleteAccountPopUpPanel = false;
                ohterReasonInput.text = "";
                SceneManager.LoadScene(0) ;
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: "         + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
        }
    }

    public void CancelButtonClick()
    {
        deleteAccountPopUpPanel.SetActive(false);
        isDeleteAccountPopUpPanel = false;
    }

    public void LevelSet()
    {
        //foreach (Transform child in content.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        //for (int i = 1; i <= levelNumber; i++)
        //{
        //    GameObject A = Instantiate(levelPrefab, content.transform);
        //    A.transform.localScale = Vector3.one;
        //    A.name = i.ToString();
        //    A.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
        //    A.GetComponent<Button>().onClick.AddListener(ClenderLevelButtonClick);
        //}
    }

    public void ClenderLevelButtonClick()
    {
        SceneManager.LoadScene(2);
    }

    public void DoubleTapButtonClick()
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickTime)
        {
            SceneManager.LoadScene(2);
        }
        lastClickTime = Time.time;
    }

    public void DeletOption1ButtonClick()
    {
        ButtonSelect(dOption1, dOption2, dOption3, dOption4, dOption5, false);
        deleteReason = "i'm using deferent account";
    }

    public void DeletOption2ButtonClick()
    {
        ButtonSelect(dOption2, dOption1, dOption3, dOption4, dOption5, false);
        deleteReason = "the app isn't working properly";
    }

    public void DeletOption3ButtonClick()
    {
        ButtonSelect(dOption3, dOption1, dOption2, dOption4, dOption5, false);
        deleteReason = "i'm worried about my privacy";
    }

    public void DeletOption4ButtonClick()
    {
        ButtonSelect(dOption4, dOption1, dOption3, dOption2, dOption5, false);
        deleteReason = "no one repliles";
    }

    public void DeletOption5ButtonClick()
    {
        ButtonSelect(dOption5, dOption1, dOption3, dOption4, dOption2, true);
        otherReasonObject.SetActive(true);
        buttonObject.SetActive(false);
    }

    public void DeleteOtherCancelButtonClick()
    {
        ButtonColor();
        otherReasonObject.SetActive(false);
        buttonObject.SetActive(true);
        ohterReasonInput.text = "";
    }

    public void DeleteOtherDoneButtonClick()
    {
        //otherReasonObject.SetActive(false);
        //buttonObject.SetActive(true);
        //deleteButton.gameObject.SetActive(true);
        deleteAccountPopUpPanel.SetActive(true);
        isDeleteAccountPopUpPanel = true;
    }

    public void EditPictureProfilePanelButtonClick()
    {
        editPictureProfilePanel.SetActive(false);
        isEditPictureProfilePanel = false;
    }

    public void ButtonSelect(Button selectButton, Button other1, Button other2, Button other3, Button other4, bool message)
    {
        selectButton.GetComponent<Image>().sprite = deletSelectButton;
        other1.GetComponent<Image>().sprite = deletNormalButton;
        other2.GetComponent<Image>().sprite = deletNormalButton;
        other3.GetComponent<Image>().sprite = deletNormalButton;
        other4.GetComponent<Image>().sprite = deletNormalButton;
        ohterReasonInput.text = "";
    }

    public void ButtonColor()
    {
        dOption1.GetComponent<Image>().sprite = deletNormalButton;
        dOption2.GetComponent<Image>().sprite = deletNormalButton;
        dOption3.GetComponent<Image>().sprite = deletNormalButton;
        dOption4.GetComponent<Image>().sprite = deletNormalButton;
        dOption5.GetComponent<Image>().sprite = deletNormalButton;
    }

    public void PanelActive(string panel)
    {
        levelPanel.SetActive(panel.Equals(levelPanel.name));
        mapsPanel.SetActive(panel.Equals(mapsPanel.name));
        tapPanel.SetActive(panel.Equals(tapPanel.name));
        settingPanel.SetActive(panel.Equals(settingPanel.name));
        myProfilePanel.SetActive(panel.Equals(myProfilePanel.name));
        editProfilePanel.SetActive(panel.Equals(editProfilePanel.name));
        editPictureProfilePanel.SetActive(panel.Equals(editPictureProfilePanel.name));
        changePasswordPanel.SetActive(panel.Equals(changePasswordPanel.name));
        contactUsPanel.SetActive(panel.Equals(contactUsPanel.name));
        deleteAccountPanel.SetActive(panel.Equals(deleteAccountPanel.name));
        deleteAccountPopUpPanel.SetActive(panel.Equals(deleteAccountPopUpPanel.name));
        aboutsPanel.SetActive(panel.Equals(aboutsPanel.name));
        privacyPolicyPanel.SetActive(panel.Equals(privacyPolicyPanel.name));
        termsandConditionsPanel.SetActive(panel.Equals(termsandConditionsPanel.name));
        allImagePanel.SetActive(panel.Equals(allImagePanel.name));
        allImagedetailPanel.SetActive(panel.Equals(allImagedetailPanel.name));
        signOutPanel.SetActive(panel.Equals(signOutPanel.name));
        subscriptionPanel.SetActive(panel.Equals(subscriptionPanel.name));
    }

    public void SetStars()
    {
        foreach (var item in totalStars)
            item.text = ApiDataCall.Instance.totalPoint.ToString();

        foreach (var item in todayStars)
            item.text = ApiDataCall.Instance.toDayPoint.ToString();
    }
}

[Serializable]
public class UserData
{
    public string UserId { get; set; }
    public string Photo { get; set; }
    public string Title { get; set; }
    public string Hint { get; set; }
    public string Date { get; set; }
    public string Status { get; set; }
    public string RejectReason { get; set; }
    public string Id { get; set; }
}

[Serializable]
public class UData
{
    public int TotalRecords { get; set; }
    public List<ImageData> ImagesList { get; set; }
}

[Serializable]
public class UJsonResponse
{
    public string Version { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public Data Data      { get; set; }
    public string Message { get; set; }
}

[Serializable]
public class StarData
{
    public string Date { get; set; }
    public string UserStar { get; set; }
    public string Id { get; set; }
}

[Serializable]
public class StarResponse
{
    public string Version { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<StarData> Data { get; set; }
    public string Message { get; set; }
}