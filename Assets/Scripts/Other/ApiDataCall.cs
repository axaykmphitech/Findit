using UnityEngine;

public class ApiDataCall : MonoBehaviour
{
    public string baseUrl = "http://hexanetwork.in:4006/api/";
    //public string baseUrl = "https://s9c0vkj4-4006.inc1.devtunnels.ms/api/";

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
    public string userId;

    [Header("Current Level Details")]
    public string title;
    public string photo;
    public string hint;
    public string xPos;
    public string yPos;
    public string xScale;
    public string yScale;
    public string id;

    public static ApiDataCall Instance;

    public void Awake()
    {
        Debug.Log("awake");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}