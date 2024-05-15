using UnityEngine;

public class NPCManager : MonoBehaviour
{
    
    public static NPCManager Instance { get; private set; }

    [Header("Destinations")]
    public Transform stage;
    //public Transform toilet;
    //public Transform buergerKing;
    //public Transform beerStand;

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
    public float admissionPrice = 50f;

    [Header("SpawnProperties")]
    public int maxNPCAmount;
    public float npcSpawnInterval;
    public Transform[] spawnPositions;

    [Header("LeaveProperties")]
    public float NPCHesitateTime;

    private int currentNPCAmount;
    private float intervalCount;
    private void Awake()
    {

        Instance = this;


    }

    private void Start()
    {
        intervalCount = npcSpawnInterval;
    }

    private void FixedUpdate()
    {
        //print(intervalCount);
        if(currentNPCAmount < maxNPCAmount)
        {
            SpawnNPC();
        }
        
    }

    public void SpawnNPC()
    {
        if(intervalCount<0)
        {
            Instantiate(npcPrefab, spawnPositions[Random.Range(0, spawnPositions.Length - 1)].position, Quaternion.identity);
            currentNPCAmount++;
            intervalCount = npcSpawnInterval;
            PlayerProperties.Instance.MoneyChange(+admissionPrice);
        }
        else
        {
            intervalCount -= Time.fixedDeltaTime;
        }
        
    }

    public void ReduceNPC()
    {
        currentNPCAmount--;
    }

}
