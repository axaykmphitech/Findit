using UnityEngine;
using UnityEngine.UI;

public class Picture : MonoBehaviour
{
    public Image profileImage;
    public Image buttonIcon;
    public Sprite camaraSprite;
    public Sprite pencilSprite;

    private void Update()
    {
        if(profileImage.sprite.name == "Dummy")
        {
            buttonIcon.sprite = camaraSprite;
        }
        else
        {
            buttonIcon.sprite = pencilSprite;
        }
    }
}
