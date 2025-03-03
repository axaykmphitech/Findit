using UnityEngine;

public class CameraFitSprite : MonoBehaviour
{
    [Header("Sprite & Image")]
    public SpriteRenderer targetSprite;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        FitCameraToSprite();
    }

    void FitCameraToSprite()
    {
        if (targetSprite == null || cam == null) return;

        Bounds spriteBounds = targetSprite.bounds;

        float spriteHeight = spriteBounds.size.y;
        float spriteWidth = spriteBounds.size.x;

        float screenRatio = (float)Screen.width / Screen.height;
        float spriteRatio = spriteWidth / spriteHeight;

        if (screenRatio >= spriteRatio)
        {
            cam.orthographicSize = spriteHeight / 2;
        }
        else
        {
            cam.orthographicSize = (spriteWidth / 2) / screenRatio;
        }
    }
}