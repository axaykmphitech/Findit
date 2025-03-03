using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Bitsplash.DatePicker;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class AdminManager : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject signInPanel;
    public GameObject forgotPasswordPanel;
    public GameObject authenticationPanel;
    public GameObject createPasswordPanel;
    public GameObject createPasswordDonePanel;
    public GameObject imageSelectPanel;
    public GameObject imageDetailPanel;
    public GameObject imageDetailopenPanel;
    public GameObject selectdatePanel;
    public GameObject scheduledImageDetailPanel;
    public GameObject deletePanel;
    public GameObject editImagedatePanel;
    public GameObject editImageDetaildatePanel;
    public GameObject settingPanel;
    public GameObject myProfilePanel;
    public GameObject editProfilePanel;
    public GameObject editPictureProfilePanel;
    public GameObject changePasswordPanel;
    public GameObject aboutsPanel;
    public GameObject privacyPolicyPanel;
    public GameObject termsandConditionsPanel;
    public GameObject signOutPanel;
    public GameObject deleteAccountPanel;
    public GameObject deleteAccountPopUpPanel;
    public GameObject submitPanel;
    public GameObject submitImageSelectPointPanel;
    public GameObject datePanel;
    public GameObject reasonPanel;
    public GameObject newImageObject;
    public GameObject scheduledImagesObject;
    public GameObject mainLevel;
    public GameObject otherReasonObject;
    public GameObject buttonObject;
    public GameObject editProfileName;
    public GameObject editProfileEmail;
    public GameObject settingContent;

    [Header("Button")]
    public Button signInPassInputHide;
    public Button createNewPassInputHide;
    public Button createConfirmPassInputHide;
    public Button newImagesButton;
    public Button scheduledImagesButton;
    public Button currentPasswordHideButton;
    public Button newPasswordHideButton;
    public Button confirmPasswordHideButton;
    public Button dOption1;
    public Button dOption2;
    public Button dOption3;
    public Button dOption4;
    public Button dOption5;
    public Button DateButton;
    public Button scheduledImagesDateButton;
    public Button submitImageDateButton;

    [Header("Bool")]
    public bool isSignpass = false;
    public bool isCreateNewPass = false;
    public bool isCreateConfirmPass = false;
    public bool isCurrentPasswordHide = false;
    public bool isNewPasswordHide = false;
    public bool isConfirmPasswordHide = false;
    public bool isCreatePasswordDonePanel = false;
    public bool isReasonPanel = false;
    public bool isSelectdatePanel = false;
    public bool isDeletePanel = false;
    public bool isEditImagedatePanel = false;
    public bool isEditImageDetaildatePanel = false;
    public bool isEditPictureProfilePanel = false;
    public bool isSignOutPanel = false;
    public bool isDeleteAccountPopUpPanel = false;
    public bool isDatePanel = false;
    public bool isSplash = false;

    [Header("Sprite & Image")]
    public Sprite on;
    public Sprite off;
    public Sprite blueButton;
    public Sprite whiteButton;
    public Image pictureImage;
    public SpriteRenderer renderer;
    public Sprite deletNormalButton;
    public Sprite deletSelectButton;
    public Sprite highlight;
    public SpriteRenderer targetSprite;

    [Header("InputField")]
    public TMP_InputField signPassInput;
    public TMP_InputField signEmailInput;
    public TMP_InputField forgotPasswordEmailInput;
    public TMP_InputField createNewPassInput;
    public TMP_InputField createConfirmPassInput;
    public TMP_InputField rejectReasonInput;
    public TMP_InputField editProfileNameInput;
    public TMP_InputField editProfileEmailInput;
    public TMP_InputField currentPasswordInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField otherDeletReasonInput;
    public TMP_InputField submitImageTitleInput;
    public TMP_InputField submitImageHintInput;
    public TMP_InputField verifyCode1Input;
    public TMP_InputField verifyCode2Input;
    public TMP_InputField verifyCode3Input;
    public TMP_InputField verifyCode4Input;

    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI adminMyProfileEmailInput;
    public TextMeshProUGUI adminMyProfileUserInput;
    public TextMeshProUGUI adminSettingEmailInput;
    public TextMeshProUGUI adminSettingUserInput;
    public TextMeshProUGUI otpVerifyCode;

    [Header("DatePickerSettings")]
    public DatePickerSettings[] DatePicker;

    [Header("Canvas")]
    public Canvas canvas;

    [Header("Camera")]
    public Camera camera;

    [Header("URl")]
    private string loginUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/admin/login";
    private string forgotPasswordUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/admin/forgotPassword";
    private string verifyOTPUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/admin/verifyOTP";
    private string resetPasswordUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/admin/resetPassword";

    public static AdminManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        isSignpass = true;
        isCreateNewPass = true;
        isCreateConfirmPass = true;
        isCurrentPasswordHide = true;
        isNewPasswordHide = true;
        isConfirmPasswordHide = true;
        ActivePanel(signInPanel.name);
    }

    public void Update()
    {
        if (imageSelectPanel.activeInHierarchy == true)
        {
            if (newImageObject.activeInHierarchy == true)
            {
                CheckButtons(newImagesButton, scheduledImagesButton);
            }
            else if (scheduledImagesObject.activeInHierarchy == true)
            {
                CheckButtons(scheduledImagesButton, newImagesButton);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (signInPanel.activeInHierarchy)
            {
                SceneManager.LoadScene(0);
            }
            if (forgotPasswordPanel.activeInHierarchy)
            {
                ActivePanel(signInPanel.name);
                forgotPasswordEmailInput.text = "";
            }
            else if (authenticationPanel.activeInHierarchy)
            {
                ActivePanel(forgotPasswordPanel.name);

                for (int i = 0; i < OTPInputManager.Instance.otpFields.Length; i++)
                {
                    OTPInputManager.Instance.otpFields[i].text = "";
                }
            }
            else if (createPasswordPanel.activeInHierarchy && !isCreatePasswordDonePanel)
            {
                ActivePanel(forgotPasswordPanel.name);
                createNewPassInput.text = "";
                createConfirmPassInput.text = "";
            }
            else if (createPasswordDonePanel.activeInHierarchy && isCreatePasswordDonePanel)
            {
                ActivePanel(signInPanel.name);
                isCreatePasswordDonePanel = false;
                createNewPassInput.text = "";
                createConfirmPassInput.text = "";
                forgotPasswordEmailInput.text = "";
            }
            else if (imageDetailPanel.activeInHierarchy && !isReasonPanel && !isSelectdatePanel)
            {
                DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
                DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
                imageDetailPanel.SetActive(false);
            }
            else if (reasonPanel.activeInHierarchy && isReasonPanel)
            {
                imageDetailPanel.SetActive(true);
                reasonPanel.SetActive(false);
                isReasonPanel = false;
                rejectReasonInput.text = "";
            }
            else if (imageDetailopenPanel.activeInHierarchy)
            {
                ActivePanel(imageSelectPanel.name);
            }
            else if (selectdatePanel.activeInHierarchy && isSelectdatePanel)
            {
                imageDetailPanel.SetActive(true);
                selectdatePanel.SetActive(false);
                isSelectdatePanel = false;
                DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
                DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
            }
            else if (scheduledImageDetailPanel.activeInHierarchy && !isDeletePanel && !isEditImagedatePanel)
            {
                scheduledImageDetailPanel.SetActive(false);
            }
            else if (deletePanel.activeInHierarchy && isDeletePanel)
            {
                scheduledImageDetailPanel.SetActive(true);
                deletePanel.SetActive(false);
                isDeletePanel = false;
            }
            else if (editImagedatePanel.activeInHierarchy && isEditImagedatePanel && !isEditImageDetaildatePanel)
            {
                scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
                scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
                scheduledImageDetailPanel.SetActive(true);
                editImagedatePanel.SetActive(false);
                isEditImagedatePanel = false;
            }
            else if (editImageDetaildatePanel.activeInHierarchy && isEditImageDetaildatePanel)
            {
                editImagedatePanel.SetActive(true);
                editImageDetaildatePanel.SetActive(false);
                isEditImageDetaildatePanel = false;
                scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
                scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
            }
            else if (settingPanel.activeInHierarchy && !isSignOutPanel)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                ActivePanel(imageSelectPanel.name);
            }
            else if (myProfilePanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                ActivePanel(settingPanel.name);
            }
            else if (editProfilePanel.activeInHierarchy && !isEditPictureProfilePanel)
            {
                ActivePanel(myProfilePanel.name);
                editProfileNameInput.text = "";
                editProfileEmailInput.text = "";
            }
            else if (editPictureProfilePanel.activeInHierarchy && isEditPictureProfilePanel)
            {
                editPictureProfilePanel.SetActive(false);
                isEditPictureProfilePanel = false;
            }
            else if (changePasswordPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                ActivePanel(settingPanel.name);
                currentPasswordInput.text = "";
                newPasswordInput.text = "";
                confirmPasswordInput.text = "";
            }
            else if (aboutsPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                ActivePanel(settingPanel.name);
            }
            else if (privacyPolicyPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                ActivePanel(settingPanel.name);
            }
            else if (termsandConditionsPanel.activeInHierarchy)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                ActivePanel(settingPanel.name);
            }
            else if (signOutPanel.activeInHierarchy && isSignOutPanel)
            {
                settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                signOutPanel.SetActive(false);
                isSignOutPanel = false;
            }
            else if (deleteAccountPanel.activeInHierarchy && !isDeleteAccountPopUpPanel)
            {
                if (buttonObject.activeInHierarchy)
                {
                    settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
                    ButtonColor();
                    ActivePanel(settingPanel.name);
                    otherDeletReasonInput.text = "";
                }
                else if (otherReasonObject.activeInHierarchy)
                {
                    ButtonColor();
                    buttonObject.SetActive(true);
                    otherReasonObject.SetActive(false);
                }
            }
            else if (deleteAccountPopUpPanel.activeInHierarchy && isDeleteAccountPopUpPanel)
            {
                deleteAccountPopUpPanel.SetActive(false);
                isDeleteAccountPopUpPanel = false;
            }
            else if (submitPanel.activeInHierarchy && !isDatePanel)
            {
                ActivePanel(imageSelectPanel.name);
                submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
                submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
                submitImageTitleInput.text = "";
                submitImageHintInput.text = "";
            }
            else if (submitImageSelectPointPanel.activeInHierarchy && !canvas.isActiveAndEnabled)
            {
                mainLevel.SetActive(false);
                camera.orthographicSize = 13;
                ActivePanel(submitPanel.name);
                mainLevel.GetComponent<SpriteRenderer>().sprite = null;

                if (mainLevel.transform.childCount == 1)
                {
                    Destroy(mainLevel.transform.GetChild(0).gameObject);
                }

                mainLevel.GetComponent<TapCreateSpriteLevel>().isAnsSpriteAvailable = false;
                mainLevel.GetComponent<TapCreateSpriteLevel>().isCreated = false;
            }
            else if (datePanel.activeInHierarchy && isDatePanel)
            {
                datePanel.SetActive(false);
                isDatePanel = false;
                submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
                submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
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

    public void CheckButtons(Button fButton, Button sButton)
    {
        fButton.GetComponent<Image>().sprite = blueButton;
        sButton.GetComponent<Image>().sprite = whiteButton;
        fButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        sButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
    }

    public IEnumerator FindItPanelChange()
    {
        yield return new WaitForSeconds(0.4f);
        ActivePanel(signInPanel.name);
    }

    public void SignInPasswordHideButtonClick()
    {
        if (!isSignpass)
        {
            signInPassInputHide.GetComponent<Image>().sprite = off;
            signPassInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isSignpass = true;
        }
        else if (isSignpass)
        {
            signInPassInputHide.GetComponent<Image>().sprite = on;
            signPassInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isSignpass = false;
        }
        SignInManager.Instance.RefreshInputField(signPassInput);
    }

    public void SignInButtonClick()
    {
        StartCoroutine(ForgotPasswordVerfyCodeCheck(signPassInput.text, signEmailInput.text, "android", "asfbsduyf", "admin"));
    } 

    public IEnumerator ForgotPasswordVerfyCodeCheck(string password, string email, string deviceType, string deviceToken, string userType)
    {
        WWWForm form = new WWWForm();
        form.AddField("password", password);
        form.AddField("email", email);
        form.AddField("deviceType", deviceType);
        form.AddField("deviceToken", deviceToken);
        form.AddField("userType", userType);

        Debug.Log(email + " email");
        Debug.Log(password + " password");
        Debug.Log(deviceType + " deviceType");
        Debug.Log(deviceToken + " deviceToken");
        Debug.Log(userType + " userType");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                ActivePanel(imageSelectPanel.name);
                signPassInput.text = "";
                signEmailInput.text = "";
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void ForgotPasswordButtonClick()
    {
        ActivePanel(forgotPasswordPanel.name);
    }

    public void ForgotPasswordBackButtonClick()
    {
        ActivePanel(signInPanel.name);
        forgotPasswordEmailInput.text = "";
    }

    public void ForgotPasswordSendButtonClick()
    {
        StartCoroutine(ForgotPasswordEmailCheck(forgotPasswordEmailInput.text));
    }

    public IEnumerator ForgotPasswordEmailCheck(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        Debug.Log(email + " email");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(forgotPasswordUrl, form))
        {
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                ActivePanel(authenticationPanel.name);
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void AuthenticationBackButtonClick()
    {
        ActivePanel(forgotPasswordPanel.name);

        for (int i = 0; i < OTPInputManager.Instance.otpFields.Length; i++)
        {
            OTPInputManager.Instance.otpFields[i].text = "";
        }
    }

    public void AuthenticationVerifyButtonClick()
    {
        string otpVerifyCodeText = otpVerifyCode.text;

        otpVerifyCodeText = verifyCode1Input.text + verifyCode2Input.text + verifyCode3Input.text + verifyCode4Input.text;

        StartCoroutine(AuthenticationVerifyOtpCheck(forgotPasswordEmailInput.text, otpVerifyCodeText));
    }

    public IEnumerator AuthenticationVerifyOtpCheck(string email, string otp)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("otp", otp);

        Debug.Log(email + " email");
        Debug.Log(otp + " otp");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(verifyOTPUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                ActivePanel(createPasswordPanel.name);
                for (int i = 0; i < OTPInputManager.Instance.otpFields.Length; i++)
                {
                    OTPInputManager.Instance.otpFields[i].text = "";
                }
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void AuthenticationResendButtonClick()
    {
        verifyCode1Input.text = "";
        verifyCode2Input.text = "";
        verifyCode3Input.text = "";
        verifyCode4Input.text = "";
        StartCoroutine(AuthenticationResendCodeCheck(forgotPasswordEmailInput.text));
    }

    public IEnumerator AuthenticationResendCodeCheck(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        Debug.Log(email + " email");

        using (UnityWebRequest request = UnityWebRequest.Post(forgotPasswordUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void CreatePasswordBackButtonClick()
    {
        ActivePanel(forgotPasswordPanel.name);
        createNewPassInput.text = "";
        createConfirmPassInput.text = "";
    }

    public void CreatePasswordResetButtonClick()
    {
        StartCoroutine(CreatePasswordResetCheck(forgotPasswordEmailInput.text, createNewPassInput.text));
    }

    public IEnumerator CreatePasswordResetCheck(string email, string newPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("newPassword", newPassword);

        Debug.Log(email + " email");
        Debug.Log(newPassword + " newPassword");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(resetPasswordUrl, form))
        {
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                createPasswordDonePanel.SetActive(true);
                isCreatePasswordDonePanel = true;
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void CreatePasswordNewPassHideButtonClick()
    {
        if (!isCreateNewPass)
        {
            createNewPassInputHide.GetComponent<Image>().sprite = off;
            createNewPassInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isCreateNewPass = true;
        }
        else if (isCreateNewPass)
        {
            createNewPassInputHide.GetComponent<Image>().sprite = on;
            createNewPassInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isCreateNewPass = false;
        }
        SignInManager.Instance.RefreshInputField(createNewPassInput);
    }

    public void CreatePasswordConfirmPassHideButtonClick()
    {
        if (!isCreateConfirmPass)
        {
            createConfirmPassInputHide.GetComponent<Image>().sprite = off;
            createConfirmPassInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isCreateConfirmPass = true;
        }
        else if (isCreateConfirmPass)
        {
            createConfirmPassInputHide.GetComponent<Image>().sprite = on;
            createConfirmPassInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isCreateConfirmPass = false;
        }
        SignInManager.Instance.RefreshInputField(createConfirmPassInput);
    }

    public void CreatePasswordDoneOKButtonClick()
    {
        ActivePanel(signInPanel.name);
        isCreatePasswordDonePanel = false;
        forgotPasswordEmailInput.text = "";
        createNewPassInput.text = "";
        createConfirmPassInput.text = "";
    }

    public void SettingButtonClick()
    {
        ActivePanel(settingPanel.name);
    }

    public void ImageUploadButtonClick()
    {
        ActivePanel(submitPanel.name);
    }

    public void NewImageButtonClick()
    {
        newImageObject.SetActive(true);
        scheduledImagesObject.SetActive(false);
    }

    public void ScheduledImagesButtonClick()
    {
        newImageObject.SetActive(false);
        scheduledImagesObject.SetActive(true);
    }

    public void AppleNewImageButtonClick()
    {
        imageDetailPanel.SetActive(true);
    }

    public void AppleScheduledImageButtonClick()
    {
        DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
        scheduledImageDetailPanel.SetActive(true);
    }

    public void CancelButtonClick()
    {
        DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
        ActivePanel(imageSelectPanel.name);
    }

    public void DoneButtonClick()
    {
        ActivePanel(imageSelectPanel.name);
    }

    public void SelectDateButtonClick()
    {
        imageDetailPanel.SetActive(false);
        selectdatePanel.SetActive(true);
        isSelectdatePanel = true;
        DatePicker[0].Content.Selection.Clear();
    }

    public void CancelDateButtonClick()
    {
        isSelectdatePanel = false;
        DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        DateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
        selectdatePanel.SetActive(false);
        imageDetailPanel.SetActive(true);
    }

    public void OkDateButtonClick()
    {
        selectdatePanel.SetActive(false);
        imageDetailPanel.SetActive(true);
        isSelectdatePanel = false;
    }

    public void ImageDetailOpenBackButtonClick()
    {
        ActivePanel(imageSelectPanel.name);
    }

    public void ScheduledImageEditButtonClick()
    {
        editImagedatePanel.SetActive(true);
        scheduledImageDetailPanel.SetActive(false);
        isEditImagedatePanel = true;
    }

    public void ScheduledImageDeleteButtonClick()
    {
        deletePanel.SetActive(true);
        scheduledImageDetailPanel.SetActive(false);
        isDeletePanel = true;
    }

    public void ScheduledImageCancelButtonClick()
    {
        scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
        ActivePanel(imageSelectPanel.name);
        isEditImagedatePanel = false;
    }

    public void ScheduledImageUpdateButtonClick()
    {
        scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
        ActivePanel(imageSelectPanel.name);
        isEditImagedatePanel = false;
    }

    public void ScheduledImageDateButtonClick()
    {
        editImageDetaildatePanel.SetActive(true);
        editImagedatePanel.SetActive(false);
        isEditImageDetaildatePanel = true;
        DatePicker[1].Content.Selection.Clear();
    }

    public void ScheduledImageDateCancelButtonClick()
    {
        editImageDetaildatePanel.SetActive(false);
        editImagedatePanel.SetActive(true);
        isEditImageDetaildatePanel = false;
        scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        scheduledImagesDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
    }

    public void ScheduledImageDateOkButtonClick()
    {
        editImageDetaildatePanel.SetActive(false);
        editImagedatePanel.SetActive(true);
        isEditImageDetaildatePanel = false;
    }

    public void DeletePanelDeleteButtonClick()
    {
        ActivePanel(imageSelectPanel.name);
        isDeletePanel = false;
    }

    public void DeletePanelCancelButtonClick()
    {
        ActivePanel(imageSelectPanel.name);
        isDeletePanel = false;
    }

    public void SettingBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        ActivePanel(imageSelectPanel.name);
    }

    public void MyProfileButtonClick()
    {
        ActivePanel(myProfilePanel.name);
    }

    public void ChangePasswordButtonClick()
    {
        ActivePanel(changePasswordPanel.name);
    }

    public void AboutUsButtonClick()
    {
        ActivePanel(aboutsPanel.name);
    }

    public void PrivacyPolicyButtonClick()
    {
        ActivePanel(privacyPolicyPanel.name);
    }

    public void TermsandConditionsButtonClick()
    {
        ActivePanel(termsandConditionsPanel.name);
    }

    public void LogOutButtonClick()
    {
        signOutPanel.SetActive(true);
        isSignOutPanel = true;
    }

    public void DeleteAccountButtonClick()
    {
        ActivePanel(deleteAccountPanel.name);
    }

    public void SignOutYesButtonClick()
    {
        SceneManager.LoadScene(0);
        isSignOutPanel = false;
    }

    public void SignOutNoButtonClick()
    {
        ActivePanel(settingPanel.name);
        isSignOutPanel = false;
    }

    public void MyProfileBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        ActivePanel(settingPanel.name);
    }

    public void MyProfileEditButtonClick()
    {
        ActivePanel(editProfilePanel.name);
    }

    public void EditProfileBackButtonClick()
    {
        ActivePanel(myProfilePanel.name);
        editProfileNameInput.text = "";
        editProfileEmailInput.text = "";
    }

    public void EditProfileUploadButtonClick()
    {
        ActivePanel(settingPanel.name);
        editProfileNameInput.text = "";
        editProfileEmailInput.text = "";
    }

    public void EditProfilePictureButtonClick()
    {
        editPictureProfilePanel.SetActive(true);
        isEditPictureProfilePanel = true;
    }

    public void CameraButtonClick()
    {
        editPictureProfilePanel.SetActive(false);
        isEditPictureProfilePanel = false;
        CameraManager.Instance.TakePicture(pictureImage);
    }

    public void GelleryButtonClick()
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
            }
        });
        Debug.Log("Permission result: " + permission);
    }

    public void ChangePasswordBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        ActivePanel(settingPanel.name);
        currentPasswordInput.text = "";
        newPasswordInput.text = "";
        confirmPasswordInput.text = "";
    }

    public void UpdatePasswordButtonClick()
    {
        ActivePanel(settingPanel.name);
        currentPasswordInput.text = "";
        newPasswordInput.text = "";
        confirmPasswordInput.text = "";
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
        ActivePanel(settingPanel.name);
    }

    public void PrivacyPolicyBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        ActivePanel(settingPanel.name);
    }

    public void TermsandConditionsBackButtonClick()
    {
        settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
        ActivePanel(settingPanel.name);
    }

    public void DeleteAccountBackButtonClick()
    {
        if (buttonObject.activeInHierarchy)
        {
            settingContent.GetComponent<RectTransform>().position = new Vector3(settingContent.transform.position.x, 0, settingContent.transform.position.z);
            ButtonColor();
            ActivePanel(settingPanel.name);
            otherDeletReasonInput.text = "";
        }
        else if (otherReasonObject.activeInHierarchy)
        {
            ButtonColor();
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
        otherDeletReasonInput.text = "";
        SceneManager.LoadScene(0);
        isDeleteAccountPopUpPanel = false;
    }

    public void CancelDeletPopButtonClick()
    {
        deleteAccountPopUpPanel.SetActive(false);
        isDeleteAccountPopUpPanel = false;
    }

    public void SubmitImageBackButtonClick()
    {
        ActivePanel(imageSelectPanel.name);
        submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
        submitImageTitleInput.text = "";
        submitImageHintInput.text = "";
    }

    public void SubmitImageUploadButtonClick()
    {
        PickSubmitImages();
    }

    public void PickSubmitImages()
    {
        PickSubmitImage(2048);
    }

    public void PickSubmitImage(int maxSize)
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

    public void SubmitImageSaveButtonClick()
    {
        ActivePanel(imageSelectPanel.name);
    }

    public void SubmitImageSelectPointButtonClick()
    {
        if (mainLevel.GetComponent<SpriteRenderer>().sprite != null)
        {
            mainLevel.SetActive(true);
            FitCameraToSprite();
            ActivePanel(submitImageSelectPointPanel.name);
        }
    }

    public void FitCameraToSprite()
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

    public void SubmitImageDateButtonClick()
    {
        datePanel.SetActive(true);
        isDatePanel = true;
        DatePicker[2].Content.Selection.Clear();
    }

    public void SelectPointBackButtonClick()
    {
        camera.orthographicSize = 13;
        mainLevel.SetActive(false);
        ActivePanel(submitPanel.name);
        mainLevel.GetComponent<SpriteRenderer>().sprite = null;

        if (mainLevel.transform.childCount == 1)
        {
            Destroy(mainLevel.transform.GetChild(0).gameObject);
        }

        mainLevel.GetComponent<TapCreateSpriteLevel>().isAnsSpriteAvailable = false;
        mainLevel.GetComponent<TapCreateSpriteLevel>().isCreated = false;
    }

    public void SelectPointContinueButtonClick()
    {
        if (mainLevel.transform.childCount == 0)
        {
            mainLevel.GetComponent<BoxCollider2D>().enabled = false;
            mainLevel.GetComponent<TapCreateSpriteLevel>().enabled = false;
            canvas.gameObject.SetActive(true);
        }
        else if (mainLevel.transform.childCount == 1)
        {
            canvas.gameObject.SetActive(true);
            mainLevel.GetComponent<BoxCollider2D>().enabled = false;
            mainLevel.GetComponent<TapCreateSpriteLevel>().enabled = false;
            mainLevel.transform.GetChild(0).GetComponent<DragCornerHandles>().enabled = false;
            mainLevel.transform.GetChild(0).GetComponent<DragSprite>().enabled = false;
        }
    }

    public void UploadSuccessfullOkButtonCLick()
    {
        camera.orthographicSize = 13;
        ActivePanel(imageSelectPanel.name);
        canvas.gameObject.SetActive(false);
        mainLevel.SetActive(false);
        mainLevel.GetComponent<BoxCollider2D>().enabled = true;
        mainLevel.GetComponent<TapCreateSpriteLevel>().enabled = true;
        mainLevel.GetComponent<TapCreateSpriteLevel>().isAnsSpriteAvailable = false;
        mainLevel.GetComponent<TapCreateSpriteLevel>().isCreated = false;
        mainLevel.GetComponent<SpriteRenderer>().sprite = null;
        submitImageTitleInput.text = "";
        submitImageHintInput.text = "";
        submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);

        if (mainLevel.transform.childCount == 1)
        {
            Destroy(mainLevel.transform.GetChild(0).gameObject);
        }
    }

    public void DateCancelButtonClick()
    {
        ActivePanel(submitPanel.name);
        isDatePanel = false;
        submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Select Date";
        submitImageDateButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(184, 184, 184, 255);
    }

    public void DateOkButtonClick()
    {
        ActivePanel(submitPanel.name);
        isDatePanel = false;
    }

    public void RejectImageButtonClick()
    {
        reasonPanel.SetActive(true);
        imageDetailPanel.SetActive(false);
        isReasonPanel = true;
    }

    public void RejectImageCancelButtonClick()
    {
        reasonPanel.SetActive(false);
        imageDetailPanel.SetActive(true);
        isReasonPanel = false;
        rejectReasonInput.text = "";
    }

    public void RejectImageDoneButtonClick()
    {
        reasonPanel.SetActive(false);
        imageDetailPanel.SetActive(true);
        isReasonPanel = false;
        rejectReasonInput.text = "";
    }

    public void EditPictureProfilePanelButtonClick()
    {
        editPictureProfilePanel.SetActive(false);
        isEditPictureProfilePanel = false;
    }

    public void DelectOtherCancelButtonClick()
    {
        ButtonColor();
        buttonObject.SetActive(true);
        otherReasonObject.SetActive(false);
        otherDeletReasonInput.text = "";
    }

    public void DelectOtherDoneButtonClick()
    {
        deleteAccountPopUpPanel.SetActive(true);
        isDeleteAccountPopUpPanel = true;
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

    public void ButtonSelect(Button selectButton, Button other1, Button other2, Button other3, Button other4, bool isOpen)
    {
        selectButton.GetComponent<Image>().sprite = deletSelectButton;
        other1.GetComponent<Image>().sprite = deletNormalButton;
        other2.GetComponent<Image>().sprite = deletNormalButton;
        other3.GetComponent<Image>().sprite = deletNormalButton;
        other4.GetComponent<Image>().sprite = deletNormalButton;

        otherDeletReasonInput.text = "";
    }

    public void ButtonColor()
    {
        dOption1.GetComponent<Image>().sprite = deletNormalButton;
        dOption2.GetComponent<Image>().sprite = deletNormalButton;
        dOption3.GetComponent<Image>().sprite = deletNormalButton;
        dOption4.GetComponent<Image>().sprite = deletNormalButton;
        dOption5.GetComponent<Image>().sprite = deletNormalButton;
    }

    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9_+&*-]+(?:\.[a-zA-Z0-9_+&*-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,7}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    public void ActivePanel(string panel)
    {
        signInPanel.SetActive(panel.Equals(signInPanel.name));
        forgotPasswordPanel.SetActive(panel.Equals(forgotPasswordPanel.name));
        authenticationPanel.SetActive(panel.Equals(authenticationPanel.name));
        createPasswordPanel.SetActive(panel.Equals(createPasswordPanel.name));
        createPasswordDonePanel.SetActive(panel.Equals(createPasswordDonePanel.name));
        imageSelectPanel.SetActive(panel.Equals(imageSelectPanel.name));
        imageDetailPanel.SetActive(panel.Equals(imageDetailPanel.name));
        imageDetailopenPanel.SetActive(panel.Equals(imageDetailopenPanel.name));
        selectdatePanel.SetActive(panel.Equals(selectdatePanel.name));
        scheduledImageDetailPanel.SetActive(panel.Equals(scheduledImageDetailPanel.name));
        deletePanel.SetActive(panel.Equals(deletePanel.name));
        editImagedatePanel.SetActive(panel.Equals(editImagedatePanel.name));
        editImageDetaildatePanel.SetActive(panel.Equals(editImageDetaildatePanel.name));
        settingPanel.SetActive(panel.Equals(settingPanel.name));
        myProfilePanel.SetActive(panel.Equals(myProfilePanel.name));
        editProfilePanel.SetActive(panel.Equals(editProfilePanel.name));
        editPictureProfilePanel.SetActive(panel.Equals(editPictureProfilePanel.name));
        changePasswordPanel.SetActive(panel.Equals(changePasswordPanel.name));
        aboutsPanel.SetActive(panel.Equals(aboutsPanel.name));
        privacyPolicyPanel.SetActive(panel.Equals(privacyPolicyPanel.name));
        termsandConditionsPanel.SetActive(panel.Equals(termsandConditionsPanel.name));
        signOutPanel.SetActive(panel.Equals(signOutPanel.name));
        deleteAccountPanel.SetActive(panel.Equals(deleteAccountPanel.name));
        deleteAccountPopUpPanel.SetActive(panel.Equals(deleteAccountPopUpPanel.name));
        submitPanel.SetActive(panel.Equals(submitPanel.name));
        submitImageSelectPointPanel.SetActive(panel.Equals(submitImageSelectPointPanel.name));
        datePanel.SetActive(panel.Equals(datePanel.name));
        reasonPanel.SetActive(panel.Equals(reasonPanel.name));
    }
}