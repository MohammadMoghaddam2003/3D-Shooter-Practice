using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera CameraFPS, CameraTPS, GunCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CameraTPS.enabled = !CameraTPS.enabled;
            CameraFPS.enabled = !CameraFPS.enabled;
            GunCamera.enabled = !GunCamera.enabled;
        }
    }
}
