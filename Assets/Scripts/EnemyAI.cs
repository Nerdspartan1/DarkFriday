using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour

{
    public enum EnemyState
    {
        Patroul,
        AggressivePatroul,
        Cooldown,
        Search,
        Chase
    }

    public List<GameObject> SpawnPoints;

    // Normal patroul parameters
    public float MinPatroulDistance = 5f;
    public float MaxPatroulDistance = 30f;
    public float MinWaitTime = 1f;
    public float MaxWaitTime = 5f;
    public float ChaseDuration = 2f;

    // Movement parameters to use when the player has 5 or more items. AI patroul-targets are set in proximity of the player
    public float MinAggressivePatroulDistance = 8f;
    public float MaxAggressivePatroulDistance = 20f;
    public float MinAggressivePatroulWaitTime = 1f;
    public float MaxAggressivePatroulWaitTime = 3f;
    public float AggressiveChaseDuration = 4f;

    // Searching Parameters are used when the player has aggroed the enemy
    public float MinSearchDistance = 2f;
    public float MaxSearchDistance = 4f;
    public float MinSearchWaitTime = 0f;
    public float MaxSearchWaitTime = 1f;
    public float SearchDuration = 3f;

    // Detection parameters
    public float DetectionRange = 10f;
    public float FlashlightDetectionRange = 25f;
    public float DetectionAngle = 90f;

    public float ChaseDelay = 1.5f;
    public float CoolDownDuration= 10f;

    public float NormalSpeed = 3.5f;
    public float ChaseSpeed = 6f;
    
    public GameObject Player;
    public GameManager GameManager;
    
    private NavMeshAgent navAgent;
    private bool waiting;
    public bool playerDetected;
    private bool playerWasDetected;
    private bool agressiveMode = false;
    
    public EnemyState currentState;

    private float currentChaseDelay;
    private float currentChaseDuration;
    private float currentSearchDuration;
    private float currentCooldownDuration;

    private float currentMinWaitTime;
    private float currentMaxWaitTime;
    private float currentMinDistance;
    private float currentMaxDistance;
    private Vector3 currentOrigin;
    private Vector3 mostDistantPosition;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        SetState(EnemyState.Patroul);
    }

    // Update is called once per frame
    void Update()
    {
        // Check agression level
        agressiveMode = GameManager.numberOfItemsPlaced >= 5;
        
        // StateCheck
        switch (currentState)
        {
            case EnemyState.Patroul:
            case EnemyState.AggressivePatroul:
                if (playerDetected)
                {
                    SetState(EnemyState.Chase);
                }
                break;
            case EnemyState.Cooldown:
                break;
            case EnemyState.Search:
                if (playerDetected)
                {
                    SetState(EnemyState.Chase);
                }
                else
                {
                    // Only search for a while
                    currentSearchDuration -= Time.deltaTime;
                    if (currentSearchDuration <= 0)
                    {
                        SetState(EnemyState.Cooldown);
                    }
                }
                break;
            case EnemyState.Chase:
                // If player is undetected for a while, stop chasing
                if (playerDetected)
                {
                    ResetChaseDuration();
                }
                else
                {
                    currentChaseDuration -= Time.deltaTime;
                    if (currentChaseDuration <= 0)
                    {
                        if (agressiveMode)
                        {
                            SetState(EnemyState.Search);
                        }
                        else
                        {
                            SetState(EnemyState.Cooldown);
                        }
                    }
                }
                break;
            default:
                break;
        }

        // Movement
        currentOrigin = Player.transform.position;

        switch (currentState)
        {
            case EnemyState.Patroul:
                if (DoMovement())
                {
                    currentOrigin = transform.position;
                    StartCoroutine("SetNewPatroulTarget", Random.Range(MinWaitTime, MaxWaitTime));
                }
                break;
            case EnemyState.Cooldown:
                break;
            case EnemyState.AggressivePatroul:
                if (DoMovement())
                {
                    StartCoroutine("SetNewPatroulTarget", Random.Range(MinAggressivePatroulWaitTime, MaxAggressivePatroulWaitTime));
                }

                break;
            case EnemyState.Search:              
                if (DoMovement())
                {
                    StartCoroutine("SetNewPatroulTarget", Random.Range(MinSearchWaitTime, MaxSearchWaitTime));
                }
                break;
            case EnemyState.Chase:
                
                // Wait before actually chasing the player
                if (currentChaseDelay > 0)
                {
                    currentChaseDelay -= Time.deltaTime;
                }

                if (DoMovement() && currentChaseDelay <= 0)
                {
                    StartCoroutine("SetNewPatroulTarget", 0.1f);
                }
                break;
        }


    }

    private void SetState(EnemyState state)
    {
        currentState = state;
        
        switch (state)
        {
            case EnemyState.Patroul:
                navAgent.speed = NormalSpeed;
                currentMinWaitTime = MinWaitTime;
                currentMaxWaitTime = MaxWaitTime;
                currentMinDistance = MinPatroulDistance;
                currentMaxDistance = MaxPatroulDistance;
                break;
            case EnemyState.AggressivePatroul:
                navAgent.speed = NormalSpeed;
                currentMinWaitTime = MinAggressivePatroulWaitTime;
                currentMaxWaitTime = MaxAggressivePatroulWaitTime;
                currentMinDistance = MinAggressivePatroulDistance;
                currentMaxDistance = MaxAggressivePatroulDistance;
                break;
            case EnemyState.Cooldown:
                if (agressiveMode)
                {
                    state = EnemyState.AggressivePatroul;
                }
                else
                {
                    state = EnemyState.Patroul;
                }
                GameManager.StartEnemyCooldown(CoolDownDuration);
                return;
            case EnemyState.Search:
                navAgent.speed = ChaseSpeed;
                currentMinWaitTime = MinSearchWaitTime;
                currentMaxWaitTime = MaxSearchWaitTime;
                currentMinDistance = MinSearchDistance;
                currentMaxDistance = MaxSearchDistance;
                
                break;
            case EnemyState.Chase:
                navAgent.speed = ChaseSpeed;
                currentMinDistance = 0f;
                currentMaxDistance = 0.1f;
                currentChaseDelay = ChaseDelay;
                ResetChaseDuration();
                // add chase delay because chaseduration will be counted even if chase is delayed
                currentChaseDuration += ChaseDelay;

                // TODO Play aggro sound

                break;
        }
    }

    private void ResetChaseDuration()
    {
        // Chase duration depends on aggressive level
        
        if (agressiveMode)
        {
            currentChaseDuration = AggressiveChaseDuration;
        }
        else
        {
            currentChaseDuration = ChaseDuration;
        }
    }

    private bool DoMovement()
    {
        if (currentState == EnemyState.Chase && !waiting)
        {
            return true;
        }
        else if (navAgent.velocity.magnitude == 0 && !waiting)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static Vector3 GetNewTarget(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    private IEnumerator SetNewPatroulTarget(float waitTime)
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        waiting = false;
        navAgent.SetDestination(GetNewTarget(currentOrigin, Random.Range(currentMinDistance, currentMaxDistance), -1));
    }

    public void SetRandomPosition()
    {
        float currentDistance = 0f;
        foreach (var spawnPoint in SpawnPoints)
        {
            if ((spawnPoint.transform.position - Player.transform.position).magnitude > currentDistance)
            {
                mostDistantPosition = spawnPoint.transform.position;
            }
        }
        transform.position = mostDistantPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerDetected && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("You are dead now...");
        }
    }
}
