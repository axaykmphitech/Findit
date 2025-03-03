using UnityEngine;
using TMPro;

public class OTPInputManager : MonoBehaviour
{
    [Header("OTP Input Fields")]
    public TMP_InputField[] otpFields;

    private int currentFieldIndex = -1;

    private TouchScreenKeyboard keyboard;

    public static OTPInputManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        for (int i = 0; i < otpFields.Length; i++)
        {
            int index = i;
            otpFields[i].characterLimit = 1;
            otpFields[i].onValueChanged.AddListener(delegate { OnInputValueChanged(index); });
            otpFields[i].onSubmit.AddListener(delegate { OnInputSubmit(index); });
            otpFields[i].interactable = (i == 0);
        }
    }

    public void OnInputValueChanged(int index)
    {
        if (otpFields[index].text.Length > 0)
        {
            if (index < otpFields.Length - 1)
            {
                otpFields[index + 1].interactable = true;
                FocusOnNextField(index + 1);
            }
        }
        else if (index > 0)
        {
            otpFields[index].interactable = false;
            FocusOnPreviousField(index - 1);
        }
    }

    public void FocusOnNextField(int index)
    {
        //otpFields[index].Select();
        otpFields[index].ActivateInputField();
        //if (keyboard == null || !keyboard.active)
        //{
        //    if (currentFieldIndex == -1)
        //    {
        //        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        //        Debug.Log("Keyboard Open " + otpFields[index]);
        //    }
        //}
        currentFieldIndex = index;
        Debug.Log(currentFieldIndex + " currentFieldIndex");
    }

    public void FocusOnPreviousField(int index)
    {
        Debug.Log("otpFields[index] " + otpFields[index].name + " " + " index" + " " + index);
        otpFields[index].Select();
        currentFieldIndex = index;
        Debug.Log("Clear");
    }

    public void OnInputSubmit(int index)
    {
        if (index == otpFields.Length - 1)
        {
            string otp = GetOTP();
            Debug.Log("Entered OTP: " + otp);
        }
    }

    public string GetOTP()
    {
        string otp = string.Empty;
        foreach (var field in otpFields)
        {
            otp += field.text;
        }
        return otp;
    }
}