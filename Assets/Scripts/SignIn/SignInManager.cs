using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class SignInManager : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject findItPanel;
    public GameObject signInPanel;
    public GameObject forgotPasswordPanel;
    public GameObject authenticationPanel;
    public GameObject createPasswordPanel;
    public GameObject createPasswordDonePanel;
    public GameObject createAccountPanel;
    public GameObject otpManagePanel;
    public GameObject pictureChoosePanel;

    public GameObject congratulationsPanel;
    public GameObject gameDetailsPanel;
    public GameObject termsandConditionsPanel;
    public GameObject privacyPolicyPanel;

    [Header("InputField")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField forgotPasswordInput;
    public TMP_InputField createAccountPasswordInput;
    public TMP_InputField createAccountemailInput;
    public TMP_InputField createAccountUsernameInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField confirmNewPasswordInput;
    public TMP_InputField otp1Input;
    public TMP_InputField otp2Input;
    public TMP_InputField otp3Input;
    public TMP_InputField otp4Input;
    public TMP_InputField verifyCode1Input;
    public TMP_InputField verifyCode2Input;
    public TMP_InputField verifyCode3Input;
    public TMP_InputField verifyCode4Input;

    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI Otp;
    public TextMeshProUGUI otpVerifyCode;

    [Header("Bool")]
    public bool isHide = false;
    public bool isNewPasswordHide = false;
    public bool isConfirmNewPasswordHide = false;
    public bool isCreateAccountPasswordHide = false;
    public bool isOpenPasswordPanel = false;
    public bool isPicturePanelOpen = false;
    public bool isCongratulationsPanel = false;

    [Header("Button")]
    public Button hideButton;
    public Button newpassWordHideButton;
    public Button confirmNewpassWordHideButton;
    public Button createAccountpassWordHideButton;

    [Header("Sprite & Image")]
    public Sprite on;
    public Sprite off;
    public Image pictureImage;
    public Sprite mainImage;

    [Header("URl")]
    private string loginUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/auth/login";
    private string signupUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/auth/signup";
    private string sendOtp = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/auth/sendOTP";
    private string forgotPasswordUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/auth/forgotPassword";
    private string forgotPasswordverifyOTPUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/auth/verifyOTP";
    private string resetPasswordUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/auth/resetPassword";

    public byte[] imageFile;

    public GameObject objects;

    [Header("Root")]
    public RootSign rootData;

    public static SignInManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isHide = true;
        isNewPasswordHide = true;
        isConfirmNewPasswordHide = true;
        isCreateAccountPasswordHide = true;

        if (PlayerPrefs.GetInt("PanelState", 0) == 0)
        {
            ActivePanel(findItPanel.name);
            SetPanelState();
        }
        else
        {
            ActivePanel(signInPanel.name);
        }
    }

    void SetPanelState()
    {
        PlayerPrefs.SetInt("PanelState", 1);
        PlayerPrefs.Save();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PlayerPrefs.SetInt("PanelState", 0);
        }
        else
        {
            StartCoroutine(ChangeTime());
        }
    }

    public IEnumerator ChangeTime()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerPrefs.SetInt("PanelState", 1);
    }

    private void Update()
    {
        if (findItPanel.activeInHierarchy == true)
        {
            StartCoroutine(FindItPanelChange());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (signInPanel.activeInHierarchy)
            {
                Application.Quit();
            }
            else if (forgotPasswordPanel.activeInHierarchy)
            {
                ActivePanel(signInPanel.name);
                forgotPasswordInput.text = "";
            }
            else if (authenticationPanel.activeInHierarchy)
            {
                ActivePanel(forgotPasswordPanel.name);
                for (int i = 0; i < OTPInputManager.Instance.otpFields.Length; i++)
                {
                    OTPInputManager.Instance.otpFields[i].text = "";
                }
            }
            else if (createPasswordPanel.activeInHierarchy && !isOpenPasswordPanel)
            {
                ActivePanel(forgotPasswordPanel.name);
                newPasswordInput.text = "";
                confirmNewPasswordInput.text = "";
            }
            else if (createPasswordDonePanel.activeInHierarchy && isOpenPasswordPanel)
            {
                ActivePanel(signInPanel.name);
                isOpenPasswordPanel = false;
            }
            else if (createAccountPanel.activeInHierarchy && !isPicturePanelOpen && !isCongratulationsPanel)
            {
                ActivePanel(signInPanel.name);
                createAccountUsernameInput.text = "";
                createAccountPasswordInput.text = "";
                createAccountemailInput.text = "";
                pictureImage.GetComponent<Image>().sprite = mainImage;
            }
            else if (otpManagePanel.activeInHierarchy)
            {
                ActivePanel(createAccountPanel.name);
            }
            else if (pictureChoosePanel.activeInHierarchy && isPicturePanelOpen)
            {
                ActivePanel(createAccountPanel.name);
                isPicturePanelOpen = false;
            }
            else if (congratulationsPanel.activeInHierarchy && isCongratulationsPanel)
            {
                ActivePanel(otpManagePanel.name);
                isCongratulationsPanel = false;
            }
            else if (privacyPolicyPanel.activeInHierarchy)
            {
                ActivePanel(createAccountPanel.name);
            }
            else if (termsandConditionsPanel.activeInHierarchy)
            {
                ActivePanel(createAccountPanel.name);
            }
        }
    }

    public IEnumerator FindItPanelChange()
    {
        yield return new WaitForSeconds(1);
        ActivePanel(signInPanel.name);
    }

    public void TemporaryAdminButtonClick()
    {
        SceneManager.LoadScene(4);
    }

    public void LoginButtonClick()
    {
        StartCoroutine(LoginRequest(emailInput.text, passwordInput.text, "user", "android", "abcd"));

        //if (IsValidEmail(emailInput.text))
        //{
        //ActivePanel(gameDetailsPanel.name);
        //emailInput.text = "";
        //passwordInput.text = "";
        //}
        //else
        //{
        //    //if (passwordInput.text != "1234")
        //    //{
        //    //    Debug.Log("Enter wrong Password");
        //    //}

        //    if (!IsValidEmail(emailInput.text))
        //    {
        //        Debug.Log("Enter wrong Email");
        //    }
        //}
    }

    public IEnumerator LoginRequest(string email, string password, string userType, string deviceType, string deviceToken)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("userType", userType);
        form.AddField("deviceType", deviceType);
        form.AddField("deviceToken", deviceToken);

        Debug.Log(email + " email");
        Debug.Log(password + " password");
        Debug.Log(userType + " userType");
        Debug.Log(deviceType + " deviceType");
        Debug.Log(deviceToken + " deviceToken");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                ActivePanel(gameDetailsPanel.name);
                emailInput.text = "";
                passwordInput.text = "";


                rootData = JsonUtility.FromJson<RootSign>(request.downloadHandler.text);

                Debug.Log(rootData.data.email + " rootData.data.email");
                ApiDataCall.Instnce.email = rootData.data.email;
                Debug.Log(ApiDataCall.Instnce.email + " ApiDataCall.Instnce.email");
                ApiDataCall.Instnce.userName = rootData.data.userName;
                Debug.Log(ApiDataCall.Instnce.userName + " ApiDataCall.Instnce.userName");
                ApiDataCall.Instnce.profile = rootData.data.profile;
                Debug.Log(ApiDataCall.Instnce.profile + " ApiDataCall.Instnce.profile");
                ApiDataCall.Instnce.totalPoint = rootData.data.totalPoint;
                Debug.Log(ApiDataCall.Instnce.totalPoint + " ApiDataCall.Instnce.totalPoint");
                ApiDataCall.Instnce.token = rootData.data.token;
                Debug.Log(ApiDataCall.Instnce.token + " ApiDataCall.Instnce.token");
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void SignUpButtonClick()
    {
        ActivePanel(createAccountPanel.name);
    }

    public void ForgetPasswordSendEmailCOdeButtonClick()
    {
        StartCoroutine(ForgotPasswordEmail(forgotPasswordInput.text));
    }

    public IEnumerator ForgotPasswordEmail(string email)
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

    public void VerifyCodeButtonClick()
    {
        string otpVerifyCodeText = otpVerifyCode.text;

        otpVerifyCodeText = verifyCode1Input.text + verifyCode2Input.text + verifyCode3Input.text + verifyCode4Input.text;

        StartCoroutine(ForgotPasswordVerfyCodeCheck(otpVerifyCodeText, forgotPasswordInput.text));
    }

    public IEnumerator ForgotPasswordVerfyCodeCheck(string otp, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("otp", otp);
        form.AddField("email", email);

        Debug.Log(email + " email");
        Debug.Log(otp + " otp");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(forgotPasswordverifyOTPUrl, form))
        {
            yield return request.SendWebRequest();

            // Check response
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

    public void ResendCodeButtonClick()
    {
        verifyCode1Input.text = "";
        verifyCode2Input.text = "";
        verifyCode3Input.text = "";
        verifyCode4Input.text = "";

        StartCoroutine(ResendCodeEmail(forgotPasswordInput.text));
    }

    public IEnumerator ResendCodeEmail(string email)
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
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void ResetPasswordButtonClick()
    {
        StartCoroutine(ForgotPasswordresetPasswordCheck(newPasswordInput.text, forgotPasswordInput.text));
    }

    public IEnumerator ForgotPasswordresetPasswordCheck(string newPassword, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("newPassword", newPassword);
        form.AddField("email", email);

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
                isOpenPasswordPanel = true;
                createPasswordDonePanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void PasswordChangeOkButtonClick()
    {
        ActivePanel(signInPanel.name);
        isOpenPasswordPanel = false;
        newPasswordInput.text = "";
        confirmNewPasswordInput.text = "";
        forgotPasswordInput.text = "";
    }

    public void CreateAccountSignUpButtonClick()
    {
        StartCoroutine(PostSignUp(createAccountUsernameInput.text, createAccountemailInput.text));
    }

    public IEnumerator PostSignUp(string userName, string email)
    {
        // Create form data
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("email", email);

        Debug.Log("email " + email);
        Debug.Log("userName " + userName);

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(sendOtp, form))
        {
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                ActivePanel(otpManagePanel.name);
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
    }

    public void CreateAccountSignInButtonClick()
    {
        ActivePanel(signInPanel.name);
        createAccountUsernameInput.text = "";
        createAccountPasswordInput.text = "";
        createAccountemailInput.text = "";
        pictureImage.GetComponent<Image>().sprite = mainImage;
    }

    public void CreatePictureAccountButtonClick()
    {
        pictureChoosePanel.SetActive(true);
        isPicturePanelOpen = true;
    }

    public void CameraChooseButtonClick()
    {
        pictureChoosePanel.SetActive(false);
        isPicturePanelOpen = false;
        CameraManager.Instance.TakePicture(pictureImage);
    }

    public void GalleryChooseButtonClick()
    {
        pictureChoosePanel.SetActive(false);
        isPicturePanelOpen = false;
        PickImages();
    }

    public void ContinueSignUpButtonClick()
    {
        ActivePanel(gameDetailsPanel.name);
        isCongratulationsPanel = false;
    }

    public void GameDetailsStartButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ForgetPasswordBackButtonClick()
    {
        ActivePanel(signInPanel.name);
        forgotPasswordInput.text = "";
    }

    public void AuthenticationPanelBackButtonClick()
    {
        ActivePanel(forgotPasswordPanel.name);

        for (int i = 0; i < OTPInputManager.Instance.otpFields.Length; i++)
        {
            OTPInputManager.Instance.otpFields[i].text = "";
        }
    }

    public void CreatePasswordPanelBackButtonClick()
    {
        ActivePanel(forgotPasswordPanel.name);

        newPasswordInput.text = "";
        confirmNewPasswordInput.text = "";
    }

    public void ForgotPasswordButtonClick()
    {
        ActivePanel(forgotPasswordPanel.name);
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

    public void PasswordHidButtonClick()
    {
        if (!isHide)
        {
            hideButton.GetComponent<Image>().sprite = off;
            passwordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isHide = true;
        }
        else if (isHide)
        {
            hideButton.GetComponent<Image>().sprite = on;
            passwordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isHide = false;
        }
        RefreshInputField(passwordInput);
    }

    public void RefreshInputField(TMP_InputField inputField)
    {
        string text = inputField.text;
        inputField.text = "";
        inputField.text = text;
    }

    public void NewPasswordHidButtonClick()
    {
        if (!isNewPasswordHide)
        {
            newpassWordHideButton.GetComponent<Image>().sprite = off;
            newPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isNewPasswordHide = true;
        }
        else if (isNewPasswordHide)
        {
            newpassWordHideButton.GetComponent<Image>().sprite = on;
            newPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isNewPasswordHide = false;
        }
        RefreshInputField(newPasswordInput);
    }

    public void ConfirmNewPasswordHidButtonClick()
    {
        if (!isConfirmNewPasswordHide)
        {
            confirmNewpassWordHideButton.GetComponent<Image>().sprite = off;
            confirmNewPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isConfirmNewPasswordHide = true;
        }
        else if (isConfirmNewPasswordHide)
        {
            confirmNewpassWordHideButton.GetComponent<Image>().sprite = on;
            confirmNewPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isConfirmNewPasswordHide = false;
        }
        RefreshInputField(confirmNewPasswordInput);
    }

    public void CreateAccountPasswordHideButtonClick()
    {
        if (!isCreateAccountPasswordHide)
        {
            createAccountpassWordHideButton.GetComponent<Image>().sprite = off;
            createAccountPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;
            isCreateAccountPasswordHide = true;
        }
        else if (isCreateAccountPasswordHide)
        {
            createAccountpassWordHideButton.GetComponent<Image>().sprite = on;
            createAccountPasswordInput.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
            isCreateAccountPasswordHide = false;
        }
        RefreshInputField(createAccountPasswordInput);
    }

    public void EmailCheck(TMP_InputField inputField)
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.textComponent.color = Color.black;
        }
    }

    public void ValidateEmail(TMP_InputField inputField)
    {
        if (IsValidEmail(inputField.text))
        {
            Debug.Log("Valid Email");
            inputField.textComponent.color = Color.black;
        }
        else
        {
            Debug.Log("Invalid Email Format");
            inputField.textComponent.color = Color.red;
        }
    }

    public void PrivacyPolicyButtonClick()
    {
        ActivePanel(privacyPolicyPanel.name);
    }

    public void TermsandConditionsButtonClick()
    {
        ActivePanel(termsandConditionsPanel.name);
    }

    public void PrivacyPolicyBackButtonClick()
    {
        ActivePanel(createAccountPanel.name);
    }

    public void TermsandConditionsBackButtonClick()
    {
        ActivePanel(createAccountPanel.name);
    }

    public void PictureChoosePanelButtonClick()
    {
        pictureChoosePanel.SetActive(false);
        isPicturePanelOpen = false;
    }

    public void OtpManageBackButtonClick()
    {
        ActivePanel(createAccountPanel.name);
        createAccountUsernameInput.text = "";
        createAccountPasswordInput.text = "";
        createAccountemailInput.text = "";
        pictureImage.GetComponent<Image>().sprite = mainImage;
    }

    public void OtpManageVerifyButtonClick()
    {
        string otpText = Otp.text;

        otpText = otp1Input.text + otp2Input.text + otp3Input.text + otp4Input.text;

        //otp1Input.text = otpText;
        //otp2Input.text = otpText;
        //otp3Input.text = otpText;
        //otp4Input.text = otpText;

        Debug.Log(otpText + " otpText");

        StartCoroutine(VerifySignUp(otpText, imageFile, createAccountUsernameInput.text, createAccountemailInput.text, createAccountPasswordInput.text, "user", "android", "abcd"));
    }

    public IEnumerator VerifySignUp(string otp, byte[] profile, string userName, string email, string password, string userType, string deviceType, string deviceToken)
    {
        // Create form data
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("email", email);
        form.AddField("otp", otp);
        form.AddField("userType", userType);
        form.AddBinaryData("profile", profile, "profileImage", "image/png");
        form.AddField("password", password);
        form.AddField("deviceType", deviceType);
        form.AddField("deviceToken", deviceToken);

        Debug.Log("email " + email);
        Debug.Log("userName " + userName);
        Debug.Log("otp " + otp);
        Debug.Log("profile " + profile);
        Debug.Log("password " + password);
        Debug.Log("userType " + userType);
        Debug.Log("deviceType " + deviceType);
        Debug.Log("deviceToken " + deviceToken);

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(signupUrl, form))
        {
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sign-Up Successful: " + request.downloadHandler.text);
                congratulationsPanel.SetActive(true);
                isCongratulationsPanel = true;

                rootData = JsonUtility.FromJson<RootSign>(request.downloadHandler.text);

                ApiDataCall.Instnce.email = rootData.data.email;
                ApiDataCall.Instnce.userName = rootData.data.userName;
                ApiDataCall.Instnce.profile = rootData.data.profile;
                ApiDataCall.Instnce.totalPoint = rootData.data.totalPoint;
            }
            else
            {
                Debug.LogError("Sign-Up Failed: " + request.error);
            }
        }
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
        createAccountPanel.SetActive(panel.Equals(createAccountPanel.name));
        otpManagePanel.SetActive(panel.Equals(otpManagePanel.name));
        findItPanel.SetActive(panel.Equals(findItPanel.name));
        authenticationPanel.SetActive(panel.Equals(authenticationPanel.name));
        createPasswordPanel.SetActive(panel.Equals(createPasswordPanel.name));
        createPasswordDonePanel.SetActive(panel.Equals(createPasswordDonePanel.name));
        pictureChoosePanel.SetActive(panel.Equals(pictureChoosePanel.name));
        congratulationsPanel.SetActive(panel.Equals(congratulationsPanel.name));
        gameDetailsPanel.SetActive(panel.Equals(gameDetailsPanel.name));
        termsandConditionsPanel.SetActive(panel.Equals(termsandConditionsPanel.name));
        privacyPolicyPanel.SetActive(panel.Equals(privacyPolicyPanel.name));
    }
}

[System.Serializable]
public class ApiData
{
    public string userName;
    public string email;
    public string profile;
    public string ucode;
    public string userType;
    public int totalPoint;
    public string toDayPoint;
    public string token;
    public string deleteReason;
    public string isSubscription;
    public string planExpiry;
    public string lastPurchaseToken;
    public string originalTransactionId;
    public string appleTransactionId;
    public string productId;
    public int isFreeTrialUsed;
    public string userId;
}

[System.Serializable]
public class RootSign
{
    public string version;
    public int statusCode;
    public bool isSuccess;
    public ApiData data;
    public string message;
}

[System.Serializable]
public class SignupData
{
    public string userName;
    public string email;
    public string password;

    public SignupData(string userName, string email, string password)
    {
        this.userName = userName;
        this.email = email;
        this.password = password;
    }
}