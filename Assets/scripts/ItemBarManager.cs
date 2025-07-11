using UnityEngine;
using UnityEngine.UI;

public class ItemBarManager : MonoBehaviour
{
    public Image itemBarFill;           // Assign in Inspector
    private float currentFill = 0f;     // Starts empty

    public void AddToBar(float amount)
    {
        currentFill += amount;
        currentFill = Mathf.Clamp01(currentFill);
        UpdateBar();

        // Optional: check if bar is full
        if (currentFill >= 1f)
        {
            Debug.Log("Item bar is full!");
            // Trigger reward or effect here
        }
    }

    private void UpdateBar()
    {
        if (itemBarFill != null)
        {
            itemBarFill.fillAmount = currentFill;
        }
    }
}
