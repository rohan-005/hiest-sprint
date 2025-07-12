using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    [Header("Character Settings")]
    public GameObject[] characterPrefabs;
    public Transform characterHolder;

    [Header("UI Text")]
    public TMP_Text nameText;
    public TMP_Text storyText;

    [Header("Panels")]
    public GameObject controlsPanel;
    public GameObject aboutUsPanel;

    [Header("Buttons")]
    public Button confirmButton;

    private GameObject currentCharacter;
    private int currentIndex = 0;

    void Start()
    {
        ShowCharacter(currentIndex);
        controlsPanel.SetActive(false);
        aboutUsPanel.SetActive(false);
    }

    // Show character based on index
    public void ShowCharacter(int index)
    {
        if (currentCharacter != null)
            Destroy(currentCharacter);

        currentCharacter = Instantiate(characterPrefabs[index], characterHolder.position, Quaternion.identity, characterHolder);

        CharacterInfo info = currentCharacter.GetComponent<CharacterInfo>();
        if (info != null && info.data != null)
        {
            nameText.text = info.data.characterName;
            storyText.text = info.data.funnyStory;
        }
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % characterPrefabs.Length;
        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + characterPrefabs.Length) % characterPrefabs.Length;
        ShowCharacter(currentIndex);
    }

    public void ConfirmSelection()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);
        SceneManager.LoadScene("lvl1");
    }

    // --- NEW METHODS ---

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void ShowAboutUs()
    {
        aboutUsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
    }

    public void CloseAboutUs()
    {
        aboutUsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
