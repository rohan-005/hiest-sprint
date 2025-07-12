using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs; // Assign the same prefabs as in the selector

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Instantiate(characterPrefabs[selectedIndex], transform.position, Quaternion.identity);
    }
}
