using UnityEngine;
using TMPro;

public class MoveImageOnInput : MonoBehaviour
{
    public RectTransform image;
    public TMP_InputField inputField;
    private Vector3 originalPosition;
    private TouchScreenKeyboard keyboard;

    void Start()
    {
        originalPosition = image.localPosition;
        inputField.onSelect.AddListener(MoveImageUp);
        inputField.onDeselect.AddListener(ResetImagePosition);
    }

    void MoveImageUp(string text)
    {
        image.localPosition = originalPosition + new Vector3(0, 1000f, 0);
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    void Update()
    {
        if (keyboard != null && !keyboard.active)
        {
            ResetImagePosition("");
            keyboard = null;
        }
    }

    void ResetImagePosition(string text)
    {
        image.localPosition = originalPosition;
        inputField.DeactivateInputField();
    }

    void OnDestroy()
    {
        inputField.onSelect.RemoveListener(MoveImageUp);
        inputField.onDeselect.RemoveListener(ResetImagePosition);
    }
}