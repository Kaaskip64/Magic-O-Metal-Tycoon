using UnityEngine;

public class NPCManager : MonoBehaviour
{
    
    public static NPCManager Instance { get; private set; }

    [Header("Destinations")]
    public Transform stage;
    public Transform toilet;
    public Transform buergerKing;
    public Transform beerStand;

    [Header("BaseMeter")]
    public float initialHungryMeter;
    public float initialThristMeter;
    public float initialUregencyMeter;
    public float initialSatisfaction;

    [Header("MeterThreshold")]
    public float hungryMeterThreshold;
    public float thristMeterThreshold;
    public float uregencyMeterThreshold;
    public float satisfactionThreshold;

    [Header("ChangeRate")]
    public float hungryChangeRate;
    public float thirstChangeRate;
    public float urgencyChangeRate;
    public float satisfactionChangeRate;

    [Header("RestoreTime")]
    public float eatTime;
    public float drinkTime;
    public float peeTime;

    [Header("ServiceCost")]
    public float burgerPrice;
    public float beerPrice;
    public float toiletPrice;

    [Header("NPC Object")]
    public GameObject npcPrefab;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnNPC(Transform spawnPosition)
    {
        Instantiate(npcPrefab, spawnPosition);
    }


}
