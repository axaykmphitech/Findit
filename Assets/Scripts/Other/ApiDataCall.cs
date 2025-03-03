using UnityEngine;

public class ApiDataCall : MonoBehaviour
{
    public string userName;
    public string email;
    public string profile;
    public int totalPoint;
    public string token;

    public static ApiDataCall Instnce;

    public void Awake()
    {
        if (Instnce == null)
        {
            Instnce = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}