using UnityEngine;
using System.Collections.Generic;

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
    // List to store all NPCs
    private List<Guest> npcList = new List<Guest>();

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

    // Method to add an NPC to the list
    public void RegisterNPC(Guest npc)
    {
        npcList.Add(npc);
    }

    // Method to remove an NPC from the list
    public void UnregisterNPC(Guest npc)
    {
        npcList.Remove(npc);
    }

    // Method to calculate average NPC status
    public void CalculateAverageNPCStatus(out float averageHungry, out float averageThirst, out float averageUrgency, out float averageSatisfaction)
    {
        if (npcList.Count == 0)
        {
            // If no NPCs, set default values
            averageHungry = 0f;
            averageThirst = 0f;
            averageUrgency = 0f;
            averageSatisfaction = 0f;
            return;
        }

        // Calculate averages
        float totalHungry = 0f;
        float totalThirst = 0f;
        float totalUrgency = 0f;
        float totalSatisfaction = 0f;

        foreach (Guest npc in npcList)
        {
            totalHungry += npc.hungryMeter;
            totalThirst += npc.thristMeter;
            totalUrgency += npc.urgencyMeter;
            totalSatisfaction += npc.satisfaction;
        }

        averageHungry = totalHungry / npcList.Count;
        averageThirst = totalThirst / npcList.Count;
        averageUrgency = totalUrgency / npcList.Count;
        averageSatisfaction = totalSatisfaction / npcList.Count;
    }
}

