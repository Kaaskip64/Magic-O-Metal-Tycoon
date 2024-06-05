using UnityEngine;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

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

    [SerializeField]
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

    public void UpdateNPCLimit()
    {
        int tempAmount = 0;
        foreach(var item in BuildingSystem.currentInstance.stages)
        {
            tempAmount += item.currentStagePlaylist.Count - 1;
        }
        maxNPCAmount = tempAmount * 10;
    }

    private void FixedUpdate()
    {
        print(currentNPCAmount);
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
            intervalCount = npcSpawnInterval;
            PlayerProperties.Instance.MoneyChange(+admissionPrice);
        }
        else
        {
            intervalCount -= Time.fixedDeltaTime;
        }
        
    }

    // Method to add an NPC to the list
    public void RegisterNPC(Guest npc)
    {
        npcList.Add(npc);
        currentNPCAmount++;
    }

    // Method to remove an NPC from the list
    public void UnregisterNPC(Guest npc)
    {
        npcList.Remove(npc);
        currentNPCAmount--;
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

