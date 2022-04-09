using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private Camera mainCamera;
    public Color defaultBackgroundColor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Init()
    {
        mainCamera = Camera.main;
    }

    public void SetClearColor()
    {
        if (mainCamera.clearFlags == CameraClearFlags.SolidColor)
        {
            mainCamera.backgroundColor = defaultBackgroundColor;
        }
    }

    public void SetClearColor(Color c)
    {
        if (mainCamera.clearFlags == CameraClearFlags.SolidColor)
        {
            mainCamera.backgroundColor = c;
        }
    }
}
