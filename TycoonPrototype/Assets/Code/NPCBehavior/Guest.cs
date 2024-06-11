using Pathfinding;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UI;

public class Guest : NPC_FSM
{
    #region Public Members
    [Header("States")]
    public BreakState BreakState;
    public CheerState cheerState;
    public RestoreState restoreState;
    public LeaveParkState leaveParkState;
    public IdleState idleState;

    [Header("Movement Parameter")]
    public float maxSpeed;

    [Header("NPC Stats")]
    public float hungryMeter;
    public float thristMeter;
    public float urgencyMeter;
    public float satisfaction;
/*
    [Header("Notifications")]
    public GameObject urgencyNotificationPrefab;*/
    #endregion

    #region Properties
    private AIDestinationSetter destinationSetter;
    public AIDestinationSetter DestinationSetter
    {
        get { return destinationSetter; }
    }

    private AIPath aIPath;
    public AIPath AIPath
    {
        get { return aIPath; }
    }

    private Animator animator;
    public Animator Animator
    {
        get { return animator; }
    }

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
    }

    public bool isExtraNPC;

    private Rigidbody2D rb;

    public bool isCheering;
/*    private GameObject currentNotification;

    // References to the images within the canvas
    private Image[] notificationImages;*/
    #endregion

    #region Awake
    private void Awake()
    {
        RefrencenInit();
    }

    private void RefrencenInit()
    {
        //pathfinding component
        destinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();

        //Init states
        cheerState = new CheerState();
        BreakState = new BreakState();
        restoreState = new RestoreState();
        leaveParkState = new();
        idleState = new IdleState();

        //Physics
        rb = GetComponent<Rigidbody2D>();

        //Animator
        animator = GetComponent<Animator>();

        //Sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Start
    private void InstanceInit()
    {
        //NPC value init
        hungryMeter = NPCManager.Instance.initialHungryMeter + Random.Range(-NPCManager.Instance.hungryMeterLeftOffset, NPCManager.Instance.hungryMeterRightOffset);
        thristMeter = NPCManager.Instance.initialThristMeter + Random.Range(-NPCManager.Instance.thristMeterLeftOffset, NPCManager.Instance.thristMeterrightOffset);
        urgencyMeter = NPCManager.Instance.initialUregencyMeter + Random.Range(-NPCManager.Instance.uregencyMeterLeftOffset, NPCManager.Instance.uregencyMeterRightOffset);
        satisfaction = NPCManager.Instance.initialSatisfaction + Random.Range(-NPCManager.Instance.satisfactionLeftOffset, NPCManager.Instance.satisfactionRightOffset);
    }
/*
    private void NotificationInit()
    {
        currentNotification = Instantiate(urgencyNotificationPrefab, transform.position, Quaternion.identity);
        currentNotification.transform.SetParent(transform);
        notificationImages = currentNotification.GetComponentsInChildren<Image>();
        print(notificationImages);
    
        // Deactivate all images initially
        foreach (var image in notificationImages)
        {
            image.enabled = false;
        }
    }*/

    private void StatesSetup()
    {
        //Initialize state
        SwitchState(idleState);
    }

    protected override void Start()
    {
        InstanceInit();

        /*        NotificationInit();*/

        NPCManager.Instance.RegisterNPC(this);

        StatesSetup();

        
    }
    #endregion

    #region Update
    protected override void Update()
    {
        base.Update();
        SpriteFlip();
        AnimatorSetter();
    }

    private void AnimatorSetter()
    {
        if(isCheering)
        {
            return;
        }
        if (destinationSetter.target != null && !aIPath.reachedDestination)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void SpriteFlip()
    {
        if (destinationSetter.target != null && destinationSetter.target.position.x - transform.position.x <= 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

    }
    #endregion

    #region FixedUpdate
    private void PathfindingSetting()
    {
        if(aIPath.maxSpeed!=maxSpeed)
        {
            aIPath.maxSpeed = maxSpeed;
        }
    }

    private void FixedUpdate()
    {
        base.FixUpdate();
        PathfindingSetting(); 
    }
    #endregion

    #region Utilities
    public void GoToTarget(Transform destination)
    {
        AIPath.isStopped = false;
        destinationSetter.target = destination;
    }

    private void OnDestroy()
    {
        NPCManager.Instance.UnregisterNPC(this);
    }

    public void StopPathFinding()
    {
        destinationSetter.target = null;
        AIPath.isStopped = true;
    }
    #endregion

    /*#region Notifications
    public void ShowNotification(int index)
    {
        if (currentNotification != null && index >= 0 && index < notificationImages.Length)
        {
            // Deactivate all images
*//*            foreach (var image in notificationImages)
            {
                image.enabled = false;
            }*//*

            // Activate the specific image
            notificationImages[index].enabled = true;

            // Start coroutine to hide the notification after a delay
            StartCoroutine(HideNotificationAfterTime(3f)); // Display for 3 seconds
        }
    }

    private IEnumerator HideNotificationAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideNotification();
    }

    // Method to hide all notifications
    public void HideNotification()
    {
        if (currentNotification != null)
        {
            foreach (var image in notificationImages)
            {
                image.enabled = false;
            }
        }
    }
    #endregion*/
}
