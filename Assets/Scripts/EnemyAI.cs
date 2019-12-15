using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour

{
    public enum EnemyState
    {
        Patrol,
        AggressivePatrol,
        Cooldown,
        Search,
        Chase
    }

    public List<GameObject> SpawnPoints;
    //public static EnemyAI Instance;

    // Normal patroul parameters
    public float MinPatroulDistance = 5f;
    public float MaxPatroulDistance = 30f;
    public float MinWaitTime = 1f;
    public float MaxWaitTime = 5f;
    public float ChaseDuration = 10f;

    // Movement parameters to use when the player has 5 or more items. AI patroul-targets are set in proximity of the player
    public float MinAggressivePatroulDistance = 10f;
    public float MaxAggressivePatroulDistance = 35f;
    public float MinAggressivePatroulWaitTime = 1f;
    public float MaxAggressivePatroulWaitTime = 3f;
    public float AggressiveChaseDuration = 15f;

    // Searching Parameters are used when the player has aggroed the enemy
    public float MinSearchDistance = 4f;
    public float MaxSearchDistance = 8f;
    public float MinSearchWaitTime = 0f;
    public float MaxSearchWaitTime = 0.3f;
    public float SearchDuration = 10f;
    public float AggressiveSearchDuration = 15f;

    public float ChaseDelay = 1.5f;
    public float CoolDownDuration= 10f;

    public float NormalSpeed = 3.5f;
    public float ChaseSpeed = 6f;

    private GameObject _player;

    private NavMeshAgent navAgent;
    private bool waiting;
    public bool playerDetected;
    public bool playerHiding;
    private bool aggressiveMode = false;

    public EnemyState currentState;

    private float currentChaseDelay;
    private float currentChaseDuration;
    private float currentSearchDuration;

    private float currentMinWaitTime;
    private float currentMaxWaitTime;
    private float currentMinDistance;
    private float currentMaxDistance;
    private Vector3 currentOrigin;
    private Vector3 mostDistantPosition;

	private Animator _anim;


    void Start()
    {
		_anim = GetComponent<Animator>();
		_player = GameManager.Instance.Player;
        navAgent = GetComponent<NavMeshAgent>();
        SetState(EnemyState.Patrol);
        transform.position = MostDistantSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        // Check agression level
        UpdateAgressionLevel();

        // StateCheck Section
        switch (currentState)
        {
            case EnemyState.Patrol:
            case EnemyState.AggressivePatrol:
                if (playerDetected && !playerHiding)
                {
                    SetState(EnemyState.Chase);
                }
                break;
            case EnemyState.Cooldown:
                break;
            case EnemyState.Search:
                if (playerDetected && !playerHiding)
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
                // If player is undetected, don´t touch him, and after a while, stop chasing

                if (playerDetected)
                {
                    ResetChaseDuration();
                }
                else
                {
                    currentChaseDuration -= Time.deltaTime;
                    if (currentChaseDuration <= 0 || DistanceToPlayer(transform.position) <= 3)
                    {
                        SetState(EnemyState.Search);
                    }
                }
                break;
        }

        // Movement Section
        currentOrigin = _player.transform.position;

        switch (currentState)
        {
            case EnemyState.Patrol:
                if (DoMovement())
                {
                    currentOrigin = transform.position;
                    StartCoroutine("SetNewPatroulTarget", Random.Range(MinWaitTime, MaxWaitTime));
                }
                break;
            case EnemyState.Cooldown:
                break;
            case EnemyState.AggressivePatrol:
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

		_anim.SetFloat("speed", navAgent.velocity.magnitude/2);

    }

    // Checks if the aggression level has to be changed
    private void UpdateAgressionLevel()
    {
        aggressiveMode = GameManager.Instance.numberOfItemsPlaced >= 5;
        if (aggressiveMode && currentState == EnemyState.Patrol)
        {
            SetState(EnemyState.AggressivePatrol);
        }
        else if (!aggressiveMode && currentState == EnemyState.AggressivePatrol)
        {
            SetState(EnemyState.Patrol);
        }
    }

    // Change state of the enemy, adjust variables, etc.
    private void SetState(EnemyState state)
    {
        currentState = state;

        switch (state)
        {
            case EnemyState.Patrol:
                navAgent.speed = NormalSpeed;
                currentMinWaitTime = MinWaitTime;
                currentMaxWaitTime = MaxWaitTime;
                currentMinDistance = MinPatroulDistance;
                currentMaxDistance = MaxPatroulDistance;
                break;
            case EnemyState.AggressivePatrol:
                navAgent.speed = NormalSpeed;
                currentMinWaitTime = MinAggressivePatroulWaitTime;
                currentMaxWaitTime = MaxAggressivePatroulWaitTime;
                currentMinDistance = MinAggressivePatroulDistance;
                currentMaxDistance = MaxAggressivePatroulDistance;
                break;
            case EnemyState.Cooldown:
                navAgent.SetDestination(MostDistantSpawn());
                GameManager.Instance.EnemyCooldown(CoolDownDuration);
                return;
            case EnemyState.Search:
                navAgent.speed = ChaseSpeed;
                currentMinWaitTime = MinSearchWaitTime;
                currentMaxWaitTime = MaxSearchWaitTime;
                currentMinDistance = MinSearchDistance;
                currentMaxDistance = MaxSearchDistance;
                ResetSearchDuration();
                break;
            case EnemyState.Chase:
                if (waiting)
                {
                    StopCoroutine("SetNewPatroulTarget");
                    waiting = false;
                }
                navAgent.speed = ChaseSpeed;
                currentMinDistance = 0f;
                currentMaxDistance = 0.1f;
                currentChaseDelay = ChaseDelay;
                ResetChaseDuration();
                // add chase delay because chaseduration will be counted even if chase is delayed
                currentChaseDuration += ChaseDelay;

                // TODO Play aggro sound?

                break;
        }
    }

    // Sets correct chase duration
    private void ResetChaseDuration()
    {
        // Chase duration depends on aggressive level

        if (aggressiveMode)
        {
            currentChaseDuration = AggressiveChaseDuration;
        }
        else
        {
            currentChaseDuration = ChaseDuration;
        }
    }

    // Sets correct search duration
    private void ResetSearchDuration()
    {
        // Search duration depends on aggressive level

        if (aggressiveMode)
        {
            currentSearchDuration = AggressiveSearchDuration;
        }
        else
        {
            currentSearchDuration = SearchDuration;
        }
    }

    // Checks if a new movement destination is needed
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

    // Find a valid target
    private static Vector3 GetNewTarget(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    // Coroutine for all movements
    private IEnumerator SetNewPatroulTarget(float waitTime)
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        waiting = false;
        navAgent.SetDestination(GetNewTarget(currentOrigin, Random.Range(currentMinDistance, currentMaxDistance), -1));
    }

    // Find the spawnpoint which is most away from the player and return the position
    private Vector3 MostDistantSpawn()
    {
        float currentDistance = 0f;
        foreach (var spawnPoint in SpawnPoints)
        {
            var distance = DistanceToPlayer(spawnPoint.transform.position);
            if (distance > currentDistance)
            {
                mostDistantPosition = spawnPoint.transform.position;
                currentDistance = distance;
            }
        }
        return mostDistantPosition;
    }

    // Returns the distance to the player
    public float DistanceToPlayer(Vector3 position)
    {
        var distance = (position - _player.transform.position).magnitude;
        return distance;
    }

    // Respawns enemy and sets correct patroul state
    public void Respawn()
    {
        transform.position = MostDistantSpawn();
        SetState(EnemyState.Patrol);
		GetComponent<ColorRandomizer>().Randomize();
        UpdateAgressionLevel();
    }

    // Kill the player
    private void OnTriggerEnter(Collider other)
    {
        if (playerDetected && other.gameObject.CompareTag("Player"))
        {
			GameManager.Instance.GameOver();
        }
    }
}
