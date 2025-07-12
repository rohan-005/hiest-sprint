using UnityEngine;
using TMPro; // ✅ TMP namespace

public class ItemBarManager : MonoBehaviour
{
    [Header("Item Bar Settings")]
    public UnityEngine.UI.Image itemBarFill;
    public int totalItems = 5;

    [Header("Timer Settings")]
    public float timeLimit = 30f;
    public TMP_Text timerText; // ✅ Use TMP_Text instead of legacy Text

    private float currentFill = 0f;
    private int collectedItems = 0;
    private float timer;
    private bool gameEnded = false;

    private HealthComponent playerHealth;

    void Start()
    {
        totalItems = Mathf.Max(1, totalItems);
        collectedItems = 0;
        currentFill = 0f;
        timer = timeLimit;

        UpdateBar();
        UpdateTimerDisplay();

        // 🔎 Find the player's HealthComponent
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthComponent>();
        }
    }

    [System.Obsolete]
    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timer <= 0f)
        {
            timer = 0f;
            if (currentFill < 1f)
            {
                TriggerPlayerDeath();
            }
        }
    }

    public void AddOneItem()
    {
        if (gameEnded) return;

        collectedItems++;
        currentFill = (float)collectedItems / totalItems;
        UpdateBar();

        if (currentFill >= 1f)
        {
            WinGame();
        }
    }

    private void UpdateBar()
    {
        if (itemBarFill != null)
        {
            itemBarFill.fillAmount = currentFill;
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timer).ToString() + "s";
        }
    }

    private void WinGame()
    {
        gameEnded = true;
        Debug.Log("✅ You win! Filled the bar in time.");
        // Optional: Reward logic here
    }

    [System.Obsolete]
    private void TriggerPlayerDeath()
    {
        gameEnded = true;
        Debug.Log("❌ Time’s up. You didn’t collect enough — you die.");

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(playerHealth.maxHealth); // Deal fatal damage
        }
        else
        {
            Debug.LogWarning("⚠️ No HealthComponent found on player.");
        }
    }
}
