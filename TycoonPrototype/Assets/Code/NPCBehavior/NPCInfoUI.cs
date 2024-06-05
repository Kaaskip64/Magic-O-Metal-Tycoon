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
    }
}
