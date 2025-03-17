using UnityEngine;
using TMPro;

public class DialogCanvas : MonoBehaviour
{
    public static DialogCanvas Instance;

    public GameObject failedDialog;
    public GameObject loadingDialog;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowFailedDialog(string message)
    {
        failedDialog.SetActive(true);
        failedDialog.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = message;
    }

    public void CloseFailedDialog()
    {
        failedDialog.SetActive(false);
    }
}
