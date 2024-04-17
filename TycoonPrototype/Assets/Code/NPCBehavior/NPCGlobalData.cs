using UnityEngine;

public class NPCGlobalData : MonoBehaviour
{
    // 单例实例
    public static NPCGlobalData Instance { get; private set; }

    [Header("Destinations")]
    public Transform stage;
    public Transform toliet;
    public Transform buergerKing;
    public Transform beerStand;

    [Header("BaseMeter")]
    public float initialHungryMeter;
    public float initialThristMeter;
    public float initialUregencyMeter;
    public float initialSatisfaction;

    [Header("ChangeRate")]
    public float hungryChangeRate;
    public float thirstChangeRate;
    public float urgencyChangeRate;
    public float satisfactionChangeRate;

    [Header("RestoreTime")]
    public float eatTime;
    public float drinkTime;
    public float peeTime;
    // 防止其他脚本通过实例化来创建新的实例
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

}
