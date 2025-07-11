using UnityEngine;

public class DeathCameraManager : MonoBehaviour
{
    public GameObject deathCamera;     // Camera to activate on death
    public GameObject playerCamera;    // Reference to the player's main camera

    public void SwitchToDeathCamera()
{
    if (playerCamera != null)
        playerCamera.SetActive(false);

    if (deathCamera != null)
        deathCamera.SetActive(true);
}

}
