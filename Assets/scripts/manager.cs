using UnityEngine;

public class DeathCameraManager : MonoBehaviour
{
    public GameObject deathCamera;  // Camera to enable on death

    public void ActivateDeathCamera()
    {
        if (deathCamera != null)
            deathCamera.SetActive(true);
    }
}
