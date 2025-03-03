using UnityEngine;
using TMPro;
using System.Collections;

public class KeyboardNavigation : MonoBehaviour
{
    [Header("TMP_InputField")]
    public TMP_InputField[] inputFields;
    private TouchScreenKeyboard keyboard;
    private TMP_InputField activeInputField;
    private int activeIndex = -1;

    void Start()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            int index = i;
            inputFields[index].onSelect.AddListener((text) => OpenKeyboard(inputFields[index], index));
        }
    }

    void OpenKeyboard(TMP_InputField inputField, int index)
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        activeInputField = inputField;
        activeIndex = index;
        StopAllCoroutines();
        StartCoroutine(WaitForKeyboardClose());
    }

    IEnumerator WaitForKeyboardClose()
    {
        while (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Visible)
        {
            yield return null;
        }

        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            MoveToNextInput(activeIndex);
        }
    }

    void MoveToNextInput(int index)
    {
        Debug.Log($"Done button pressed on input {index}");

        if (!string.IsNullOrEmpty(inputFields[index].text))
        {
            if (index < inputFields.Length - 1)
            {
                inputFields[index + 1].ActivateInputField();
                OpenKeyboard(inputFields[index + 1], index + 1);
            }
            else
            {
                Debug.Log("Last input field reached. Closing keyboard.");
                keyboard = null;
            }
        }
    }
}