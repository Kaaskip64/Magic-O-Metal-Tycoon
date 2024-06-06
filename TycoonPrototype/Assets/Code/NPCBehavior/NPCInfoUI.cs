using UnityEngine;
using UnityEngine.UI;

public class NPCInfoUI : MonoBehaviour
{
    public Slider hungrySlider;
    public Slider thirstSlider;
    public Slider urgencySlider;
    public Slider satisfactionSlider;

    private void LateUpdate()
    {
        UpdateAverageNPCStatus();
    }

    private void UpdateAverageNPCStatus()
    {
        float averageHungry, averageThirst, averageUrgency, averageSatisfaction;

        // Calculate average NPC status using NPCManager
        NPCManager.Instance.CalculateAverageNPCStatus(out averageHungry, out averageThirst, out averageUrgency, out averageSatisfaction);

        // Update slider values
        hungrySlider.value = Mathf.Clamp(averageHungry / 100f, 0f, 1f);
        thirstSlider.value = Mathf.Clamp(averageThirst / 100f, 0f, 1f);
        urgencySlider.value = Mathf.Clamp(averageUrgency / 100f, 0f, 1f);
        satisfactionSlider.value = Mathf.Clamp(averageSatisfaction / 100f, 0f, 1f);
<<<<<<< HEAD
=======

        // Update fill colors based on slider values
        UpdateFillColor(hungryFill, hungrySlider.value);
        UpdateFillColor(thirstFill, thirstSlider.value);
        UpdateFillColor(urgencyFill, urgencySlider.value);
        UpdateFillColor(satisfactionFill, satisfactionSlider.value);
    }

    private void UpdateFillColor(Image fill, float value)
    {
        // HSV: H=0 (red), S=1 (full saturation), V varies from 0 to 1
        float h = 0f; // Hue for red
        float s = 1f; // Full saturation
        float v = value; // Value based on the slider value
        fill.color = Color.HSVToRGB(h, s, v);
>>>>>>> UUDev
    }
}

