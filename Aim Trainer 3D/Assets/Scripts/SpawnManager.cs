using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The total number of targets to spawn per play session")]
    private int targetSpawnNumbers;

    [SerializeField]
    [Tooltip("The maximum number of targets that the gun range can have before new targets can be spawned")]
    private int maxNumberOfTargetAliveAllow;
    private int currentNumberOfTargetAlive = 0;

    [SerializeField]
    [Tooltip("The wait time before a new target is spawned")] 
    private float timeBetweenSpawn;

    [SerializeField] private TextMeshProUGUI targetSpawnNumbersText;

    [SerializeField] private UnityEvent OnTargetSpawnStarted;
    [SerializeField] private UnityEvent OnTargetSpawnStopped;

    [SerializeField] [Tooltip("The list of target spawners - objects that responsible for spawning the target at their positions")]
    private List<TargetSpawner> targetSpawners = new List<TargetSpawner>();

    private bool hasStartedSpawningTarget = false;
    private float timeSinceLastSpawn;
    private int currentTargetSpawnedNum = 0;
    private int currentListElement = 0;
    private bool isCurrentTargetSpawnerSpawningMovingTargets = false;
    private ShootToActivate[] startButtons;

    private void Awake()
    {
        startButtons = FindObjectsOfType<ShootToActivate>();
    }

    private void OnEnable()
    {
        Target.OnTargetDestroyed += DecreaseNumberOfTargetsAlive;
        Target.OnTargetHitByPlayer += SetTimeSinceLastSpawnAfterPlayerDestroyedATarget;
    }

    private void OnDisable()
    {
        Target.OnTargetDestroyed -= DecreaseNumberOfTargetsAlive;
        Target.OnTargetHitByPlayer -= SetTimeSinceLastSpawnAfterPlayerDestroyedATarget;
    }

    private void Start()
    {
        timeSinceLastSpawn = timeBetweenSpawn;
    }

    private void Update()
    {
        int spawnNumText = targetSpawnNumbers - currentTargetSpawnedNum;
        if (targetSpawnNumbersText != null) targetSpawnNumbersText.text = "Targets Remaining: " + spawnNumText.ToString();

        if (!hasStartedSpawningTarget) return;

        if(maxNumberOfTargetAliveAllow != 0 && currentNumberOfTargetAlive >= maxNumberOfTargetAliveAllow)
        {
            timeSinceLastSpawn = 0f;
            return;
        }

        if(timeSinceLastSpawn >= timeBetweenSpawn)
        {
            SelectTargetSpawnerToSpawn();

            timeSinceLastSpawn = 0f;

            if (currentTargetSpawnedNum == targetSpawnNumbers) StopSpawningAndReset();

            return;
        }

        timeSinceLastSpawn += Time.deltaTime;
    }

    private void SelectTargetSpawnerToSpawn()
    {
        if (currentListElement == targetSpawners.Count)
        {
            ShuffleTargetSpawnerList();
            currentListElement = 0;
        }

        TargetSpawner targetSpawnerSelected = targetSpawners[currentListElement];
        isCurrentTargetSpawnerSpawningMovingTargets = targetSpawnerSelected.IsSpawningMovingTargets();

        currentListElement++;

        targetSpawnerSelected.SpawnTarget();
        currentTargetSpawnedNum++;
        currentNumberOfTargetAlive++;
    }

    private void ShuffleTargetSpawnerList()
    {
        System.Random rand = new System.Random();
        int count = targetSpawners.Count;
        while(count > 1)
        {
            count--;
            int i = rand.Next(count + 1);
            TargetSpawner value = targetSpawners[i];
            targetSpawners[i] = targetSpawners[count];
            targetSpawners[count] = value;
        }
    }

    private void StopSpawningAndReset()
    {
        hasStartedSpawningTarget = false;
        if (AccuracyTracker.accuracyTrackerInstance != null) AccuracyTracker.accuracyTrackerInstance.SetShotStartRegisterationStatus(false);
        timeSinceLastSpawn = timeBetweenSpawn;
    }

    //StartSpawning() is called when ShootToActivate UI event is invoked after player has shot the Start button
    public void StartSpawning()
    {
        if (hasStartedSpawningTarget) return;

        currentTargetSpawnedNum = 0;
        currentNumberOfTargetAlive = 0;

        if (targetSpawners == null || targetSpawners.Count == 0)
        {
            Debug.LogError("There is no target spawners in target spawner list of spawn manager: " + name + ". Spawning will not work!");
            return;
        }

        ShuffleTargetSpawnerList();
        currentListElement = 0;

        //if (targetSpawnNumbersText != null) targetSpawnNumbersText.text = "Targets Remaining: " + targetSpawnNumbers.ToString();
        hasStartedSpawningTarget = true;

        if (AccuracyTracker.accuracyTrackerInstance != null) AccuracyTracker.accuracyTrackerInstance.SetShotStartRegisterationStatus(true);

        TemporaryDisableNonActivatedStartButtons(true);

        OnTargetSpawnStarted?.Invoke();
    }

    public void DecreaseNumberOfTargetsAlive()
    {
        currentNumberOfTargetAlive--;
        if (currentTargetSpawnedNum == targetSpawnNumbers)//targets remaining = 0 (all targets spawned and spawn manager stopped spawning)
        {
            OnTargetSpawnStopped?.Invoke();
            TemporaryDisableNonActivatedStartButtons(false);
        }
    }

    public void SetTimeSinceLastSpawnAfterPlayerDestroyedATarget(int i)
    {
        if(!isCurrentTargetSpawnerSpawningMovingTargets) timeSinceLastSpawn = timeBetweenSpawn - 0.3f;
    }

    public void TemporaryDisableNonActivatedStartButtons(bool disabled)
    {
        if (startButtons == null || startButtons.Length == 0) return;

        if (disabled)
        {
            for(int i = 0; i < startButtons.Length; i++)
            {
                if (startButtons[i].HasActivated()) continue;
                startButtons[i].TemporaryDisable(true);
            }
            return;
        }

        for (int i = 0; i < startButtons.Length; i++)
        {
            startButtons[i].TemporaryDisable(false);
        }

    }
}
