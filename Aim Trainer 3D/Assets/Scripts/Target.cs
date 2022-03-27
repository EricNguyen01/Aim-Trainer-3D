using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [field: Header("Target Movement Config")]
    [field: SerializeField] public bool isStatic { get; set; } = true;

    [SerializeField] private bool hasRandomMoveDirection = true;
    private enum MoveDirection { MoveLeft, MoveRight }
    [SerializeField] private MoveDirection moveDirection = MoveDirection.MoveLeft;

    [SerializeField] private float minMoveDistance;
    [SerializeField] private float maxMoveDistance;
    private float moveDistance;

    [SerializeField] private float targetMinMoveSpeed = 1f;
    [SerializeField] private float targetMaxMoveSpeed = 2f;
    private float moveSpeed;

    [Header("Target Other Stats")]
    [SerializeField] private float timeAlive;
    [SerializeField] private int targetDestroyedScore;

    private Vector3 startingPos;
    private Vector3 endPos;

    private Collider targetCollider;
    private float currentTimeAlive = 0f;
    private bool movingTowardEndPos = true;

    public static event System.Action<int> OnTargetHitByPlayer;
    public static event System.Action OnTargetDestroyed;

    private void OnEnable()
    {
        TargetSpawner.OnTargetSpawnerSpawnedNewTarget += DestroyStaticTargetOnNewTargetSpawned;
    }

    private void OnDisable()
    {
        TargetSpawner.OnTargetSpawnerSpawnedNewTarget -= DestroyStaticTargetOnNewTargetSpawned;
    }

    // Start is called before the first frame update
    private void Start()
    {
        startingPos = transform.localPosition;

        targetCollider = GetComponent<Collider>();
        if (targetCollider == null) Debug.LogWarning("Target: " + name + " doesn't have a collider attached to it!");

        if (!isStatic) targetDestroyedScore *= 2;

        SetRandomMoveDirOnSpawn();
        SetMovementConfigsOnSpawn();
    }

    private void Update()
    {
        if (!isStatic)
        {
            ProcessTargetMovement();
        }

        currentTimeAlive += Time.deltaTime;

        if(currentTimeAlive >= timeAlive)
        {
            DestroyTarget();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log("Bullet Hit: " + name);
            OnTargetHitByPlayer?.Invoke(targetDestroyedScore);
            DestroyTarget();
        }
    }

    public void DestroyTarget()
    {
        OnTargetDestroyed?.Invoke();
        Destroy(gameObject);
    }

    private void ProcessTargetMovement()
    {
        if(movingTowardEndPos)
        {
            float distFromDest = Vector3.Distance(transform.localPosition, endPos);

            if(endPos.x - transform.localPosition.x <= 0f) transform.position -= transform.right * moveSpeed * Time.deltaTime;
            else transform.position += transform.right * moveSpeed * Time.deltaTime;

            if (distFromDest <= 0.13f)
            {
                movingTowardEndPos = false;
            }
        }
        if(!movingTowardEndPos)
        {
            float distFromDest = Vector3.Distance(transform.localPosition, startingPos);

            if (startingPos.x - transform.localPosition.x <= 0f) transform.position -= transform.right * moveSpeed * Time.deltaTime;
            else transform.position += transform.right * moveSpeed * Time.deltaTime;

            if (distFromDest <= 0.13f)
            {
                movingTowardEndPos = true;
            }
        }
    }

    private void SetRandomMoveDirOnSpawn()
    {
        if (!hasRandomMoveDirection) return;

        int i = Random.Range(0, 2);

        switch (i)
        {
            case 0:
                moveDirection = MoveDirection.MoveLeft;
                break;
            case 1:
                moveDirection = MoveDirection.MoveRight;
                break;
        }
    }

    private void SetMovementConfigsOnSpawn()
    {
        if (maxMoveDistance < minMoveDistance) maxMoveDistance = minMoveDistance;
        moveDistance = Random.Range(minMoveDistance, maxMoveDistance);
        
        if(moveDirection == MoveDirection.MoveLeft)
        {
            endPos = new Vector3(transform.localPosition.x - moveDistance, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            endPos = new Vector3(transform.localPosition.x + moveDistance, transform.localPosition.y, transform.localPosition.z);
        }

        if (targetMaxMoveSpeed < targetMinMoveSpeed) targetMaxMoveSpeed = targetMinMoveSpeed;
        moveSpeed = Random.Range(targetMinMoveSpeed, targetMaxMoveSpeed);
    }

    public void DestroyStaticTargetOnNewTargetSpawned()
    {
        if (isStatic)
        {
            DestroyTarget();
        }
    }
}
