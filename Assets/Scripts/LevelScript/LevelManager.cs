using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

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

    [Header("Url")]
    private string contactUsUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/user/contactUs";
    private string deleteAccountUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/user/deleteAccount";
    private string changePasswordUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/user/changePassword";
    private string updateProfileUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/user/updateProfile";

    public byte[] imageFile;

    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        isCurrentPasswordHide = true;
        isNewPasswordHide = true;
        isConfirmPasswordHide = true;
        PanelActive(levelPanel.name);

        myProfileEmailInput.text = ApiDataCall.Instnce.email;
        settingEmailInput.text = ApiDataCall.Instnce.email;

        string imageUrl = ApiDataCall.Instnce.profile; // Get image URL
        StartCoroutine(LoadImageFromURL(imageUrl));

        myProfileUserInput.text = ApiDataCall.Instnce.userName;
        settingUserInput.text = ApiDataCall.Instnce.userName;
    }

    IEnumerator LoadImageFromURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = ConvertTextureToSprite(texture);
                settingPictureImage.sprite = sprite; // Set the sprite to the UI Image
                myProfilePictureImage.sprite = sprite; // Set the sprite to the UI Image
            }
            else
            {
                Debug.LogError("Image download failed: " + request.error);
            }
        }
    }

    Sprite ConvertTextureToSprite(Texture2D texture)
    {
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
        PanelActive(tapPanel.name);
    }

    public void playPastButtonClick()
    {
        PanelActive(mapsPanel.name);
        LevelSet();
    }

    public void SubmitPhotoBtnClick()
    {
        SceneManager.LoadScene(3);
    }

    public void SettingButtonClick()
    {
        PanelActive(settingPanel.name);
    }

    public void BackMapsBtnClick()
    {
        PanelActive(levelPanel.name);
    }

    public void SettingBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(levelPanel.name);
    }

    public void MyProfileButtonClick()
    {
        PanelActive(myProfilePanel.name);
    }

    public void AllImagesButtonClick()
    {
        PanelActive(allImagePanel.name);
    }

    public void AllImagesBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
    }

    public void AllImagesDetailsButtonClick()
    {
        allImagedetailPanel.SetActive(true);
        isAllImagedetailPanel = true;
    }

    public void AllImagedetailCancelButtonClick()
    {
        allImagedetailPanel.SetActive(false);
        isAllImagedetailPanel = false;
    }

    public void SubscriptionButtonClick()
    {
        PanelActive(subscriptionPanel.name);
    }

    public void SubscriptionBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
    }

    public void SubscriptionTermsandConditionsButtonClick()
    {
        isSubscriptionPanelClick = true;
        PanelActive(termsandConditionsPanel.name);
    }

    public void SubscriptionprivacyPolicyButtonClick()
    {
        isSubscriptionPanelClick = true;
        PanelActive(privacyPolicyPanel.name);
    }

    public void SubscribeButtonClick()
    {
        PanelActive(settingPanel.name);
    }

    public void ChangePasswordButtonClick()
    {
        PanelActive(changePasswordPanel.name);
    }

    public void AboutUsButtonClick()
    {
        PanelActive(aboutsPanel.name);
    }

    public void PrivacyPolicyButtonClick()
    {
        PanelActive(privacyPolicyPanel.name);
    }

    public void ContactUsButtonClick()
    {
        PanelActive(contactUsPanel.name);
    }

    public void TermsandConditionsButtonClick()
    {
        PanelActive(termsandConditionsPanel.name);
    }

    public void DeleteAccountButtonClick()
    {
        PanelActive(deleteAccountPanel.name);
    }

    public void SignOutButtonClick()
    {
        signOutPanel.SetActive(true);
        isSignOutPanel = true;
    }

    public void SignOutYesButtonClick()
    {
        SceneManager.LoadScene(0);
        isSignOutPanel = false;
    }

    public void SignOutNoButtonClick()
    {
        signOutPanel.SetActive(false);
        isSignOutPanel = false;
    }

    public void MyProfileBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
    }

    public void EditProfileBackButtonClick()
    {
        PanelActive(myProfilePanel.name);
        editProfileNameInput.text = "";
        editProfileEmailInput.text = "";
    }

    public void EditButtonClick()
    {
        PanelActive(editProfilePanel.name);
    }

    public void UpdateProfileButtonClick()
    {
        StartCoroutine(UpdateProfileCheck(editProfileNameInput.text, imageFile));
    }

    public IEnumerator UpdateProfileCheck(string userName, byte[] profile)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddBinaryData("profile", profile, "profileImage", "image/png");

        Debug.Log(userName + " userName");
        Debug.Log(profile + " profile");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(updateProfileUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instnce.token);
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                ApiDataCall.Instnce.userName = userName;
                myProfileUserInput.text = ApiDataCall.Instnce.userName;
                settingUserInput.text = ApiDataCall.Instnce.userName;
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                PanelActive(settingPanel.name);
                editProfileNameInput.text = "";
                editProfileEmailInput.text = "";
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void CameraButtonClick()
    {
        editPictureProfilePanel.SetActive(true);
        isEditPictureProfilePanel = true;
    }

    public void CameraChooseButtonClick()
    {
        editPictureProfilePanel.SetActive(false);
        isEditPictureProfilePanel = false;
        CameraManager.Instance.TakePicture(pictureImage);
    }

    public void GalleryButtonClick()
    {
        editPictureProfilePanel.SetActive(false);
        isEditPictureProfilePanel = false;
        PickImages();
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
        Debug.Log("SpriteToTexture2D");
        // Create new texture
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGB24, false);
        Debug.Log(texture + " texture_1");


        texture.ReadPixels(new Rect(0, 0, (int)sprite.rect.width, (int)sprite.rect.height), 0, 0);

        return texture;
    }

    public void ChangePasswordBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        PanelActive(settingPanel.name);
        currentPasswordInput.text = "";
        newPasswordInput.text = "";
        confirmPasswordInput.text = "";
    }

    public void UpdatePasswordButtonClick()
    {
        StartCoroutine(UpdatePasswordCheck(currentPasswordInput.text, newPasswordInput.text));
    }

    public IEnumerator UpdatePasswordCheck(string oldPassword, string newPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("oldPassword", oldPassword);
        form.AddField("newPassword", newPassword);

        Debug.Log(oldPassword + " oldPassword");
        Debug.Log(newPassword + " newPassword");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(changePasswordUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instnce.token);
            Debug.Log(ApiDataCall.Instnce.token);
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
                Debug.LogError("Sign-Up Failed: " + request.error);
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
        contactusEmailInput.text = "";
        contactusSubjectInput.text = "";
        contactusMessageInput.text = "";
    }

    public void ContactUsSubmitButtonClick()
    {
        StartCoroutine(contactUsCheck(contactusUsernameInput.text, contactusEmailInput.text, contactusSubjectInput.text, contactusMessageInput.text));
    }

    public IEnumerator contactUsCheck(string name, string email, string subject, string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("subject", subject);
        form.AddField("message", message);

        Debug.Log(email + " email");
        Debug.Log(name + " name");
        Debug.Log(subject + " subject");
        Debug.Log(message + " message");

        // Create request
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
                Debug.LogError("Sign-Up Failed: " + request.error);
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
        StartCoroutine(DeleteCheck(ohterReasonInput.text));
    }

    public IEnumerator DeleteCheck(string deleteReason)
    {
        WWWForm form = new WWWForm();
        form.AddField("deleteReason", deleteReason);

        Debug.Log(deleteReason + " deleteReason");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(deleteAccountUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instnce.token);
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                isDeleteAccountPopUpPanel = false;
                ohterReasonInput.text = "";
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
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
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= levelNumber; i++)
        {
            GameObject A = Instantiate(levelPrefab, content.transform);
            A.transform.localScale = Vector3.one;
            A.name = i.ToString();
            A.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
            A.GetComponent<Button>().onClick.AddListener(ClenderLevelButtonClick);
        }
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
    }

    public void DeletOption2ButtonClick()
    {
        ButtonSelect(dOption2, dOption1, dOption3, dOption4, dOption5, false);
    }

    public void DeletOption3ButtonClick()
    {
        ButtonSelect(dOption3, dOption1, dOption2, dOption4, dOption5, false);
    }

    public void DeletOption4ButtonClick()
    {
        ButtonSelect(dOption4, dOption1, dOption3, dOption2, dOption5, false);
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
}