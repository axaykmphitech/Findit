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

    public Toggle acceptToggle;

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
    private string loginUrl                   = "";
    private string signupUrl                  = "";
    private string sendOtp                    = "";
    private string forgotPasswordUrl          = "";
    private string forgotPasswordverifyOTPUrl = "";
    private string resetPasswordUrl           = "";

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
        loginUrl =  ApiDataCall.Instance.baseUrl + "auth/login";
        signupUrl = ApiDataCall.Instance.baseUrl + "auth/signup";
        sendOtp =  ApiDataCall.Instance.baseUrl + "auth/sendOTP";
        forgotPasswordUrl = ApiDataCall.Instance.baseUrl + "auth/forgotPassword";
        forgotPasswordverifyOTPUrl = ApiDataCall.Instance.baseUrl + "auth/verifyOTP";
        resetPasswordUrl = ApiDataCall.Instance.baseUrl + "auth/resetPassword";

        emailInput.text = "t101@gmail.com";
        passwordInput.text = "12345678";

        isHide = true;
        isNewPasswordHide = true;
        isConfirmNewPasswordHide =    true;
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
    }

    public IEnumerator LoginRequest(string email, string password, string userType, string deviceType, string deviceToken)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("userType", userType);
        form.AddField("deviceType", deviceType);
        form.AddField("deviceToken", deviceToken);

        Debug.Log(loginUrl);

        if (!IsValidEmail(email))
        {
            DialogCanvas.Instance.ShowFailedDialog("Please enter valid email address");
        }
        else if(password.Length < 8)
        {
            DialogCanvas.Instance.ShowFailedDialog("Password should be 8 character long");
        }
        else
        {
            // Create request
            using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
            {
                yield return request.SendWebRequest();

                // Check response
                if (request.result == UnityWebRequest.Result.Success)
                {
                    ActivePanel(gameDetailsPanel.name);
                    emailInput.text = "";
                    passwordInput.text = "";

                    rootData = JsonUtility.FromJson<RootSign>(request.downloadHandler.text);

                    ApiDataCall.Instance.email = rootData.data.email;
                    ApiDataCall.Instance.userName = rootData.data.userName;
                    ApiDataCall.Instance.profile = rootData.data.profile;
                    ApiDataCall.Instance.ucode = rootData.data.ucode;
                    ApiDataCall.Instance.userType = rootData.data.userType;
                    ApiDataCall.Instance.totalPoint = rootData.data.totalPoint;
                    ApiDataCall.Instance.toDayPoint = rootData.data.toDayPoint;
                    ApiDataCall.Instance.token = rootData.data.token;
                    ApiDataCall.Instance.userId = rootData.data.userId;
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


        if (!IsValidEmail(email))
        {
            DialogCanvas.Instance.ShowFailedDialog("Please enter valid email address");
        }
        else
        {
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

    public void ResetPasswordButtonClick()
    {
        StartCoroutine(ForgotPasswordresetPasswordCheck(newPasswordInput.text, forgotPasswordInput.text));
    }

    public IEnumerator ForgotPasswordresetPasswordCheck(string newPassword, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("newPassword", newPassword);
        form.AddField("email", email);

        Debug.Log(" new pss " + newPassword);
        Debug.Log(" email " + email);

        if (newPasswordInput.text == "" || confirmNewPasswordInput.text == "")
        {
            DialogCanvas.Instance.ShowFailedDialog("\"password\" is not allowed to be empty");
        }
        else if(newPasswordInput.text.Length < 8 || confirmNewPasswordInput.text.Length < 8)
        {
            DialogCanvas.Instance.ShowFailedDialog("Password should be 8 character long");
        }
        else if(!newPasswordInput.text.Equals(confirmNewPasswordInput.text))
        {
            DialogCanvas.Instance.ShowFailedDialog("Password and confirm password do not match.");
        }
        else
        {
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
                    Debug.LogError("POST request failed!");
                    Debug.LogError("Error: " + request.error);
                    Debug.LogError("Response Code: " + request.responseCode);
                    Debug.LogError("Response Text: " + request.downloadHandler.text);
                }
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
        Debug.Log("api " + sendOtp);

        if(!IsValidEmail(email))
        {
            DialogCanvas.Instance.ShowFailedDialog("Please enter valid email address");
        }

        else if (imageFile == null || imageFile.Length == 0)
        {
            DialogCanvas.Instance.ShowFailedDialog("Please select the profile image");
        }

        else if(createAccountPasswordInput.text == "")
        {
            DialogCanvas.Instance.ShowFailedDialog("\"password\" is not allowed to be empty");
        }

        else if(createAccountPasswordInput.text.Length < 8)
        {
            DialogCanvas.Instance.ShowFailedDialog("Password should be 8 character long");
        }

        else if(!acceptToggle.isOn)
        {
            DialogCanvas.Instance.ShowFailedDialog("Please accept terms & conditions and privacy policy");
        }
        else
        {
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

                ApiDataCall.Instance.email = rootData.data.email;
                ApiDataCall.Instance.userName = rootData.data.userName;
                ApiDataCall.Instance.profile = rootData.data.profile;
                ApiDataCall.Instance.ucode = rootData.data.ucode;
                ApiDataCall.Instance.userType = rootData.data.userType;
                ApiDataCall.Instance.totalPoint = rootData.data.totalPoint;
                ApiDataCall.Instance.toDayPoint = rootData.data.toDayPoint;
                ApiDataCall.Instance.token =  rootData.data.token;
                ApiDataCall.Instance.userId = rootData.data.userId;
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status =  SimpleJSON.JSON.Parse(Json);
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
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
    public int toDayPoint;
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
        this.email    =    email;
        this.password = password;
    }
}