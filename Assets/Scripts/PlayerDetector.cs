using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
	private Stamina PlayerStamina;
    public EnemyAI EnemyAI;
    public KeyCode hidingKey;
    public GameObject EnemyEyes;
    public GameObject PlayerEyes;

    public float UndetectDelay = 1.5f;

    private bool playerInRange = false;
    private bool playerMoved = false;
    private bool mouseMoved = false;
    private bool hiding = false;

    private bool canHide = false;
    private bool wasHiding = false;
    public float currentUndetectDelay;

    private Vector3 lastMousePos;
    private Vector3 lastPlayerPos;

    void Start()
    {
		PlayerStamina = GameManager.Instance.Player.GetComponent<Stamina>();
        lastPlayerPos = GameManager.Instance.Player.transform.position;
        lastMousePos = Input.mousePosition;
    }

    void FixedUpdate()
    {
        mouseMoved = Input.mousePosition != lastMousePos;
        lastMousePos = Input.mousePosition;

        playerMoved = GameManager.Instance.Player.transform.position != lastPlayerPos;
        lastPlayerPos = GameManager.Instance.Player.transform.position;

        hiding = PlayerStamina.IsHiding;

        // Check if hiding is possible


        EnemyAI.playerDetected = playerInRange && PlayerInLineOfSight();

        if (EnemyAI.playerDetected)
        {
            currentUndetectDelay = UndetectDelay;
        }

        if (currentUndetectDelay > 0)
        {
            EnemyAI.playerDetected = true;
            currentUndetectDelay -= Time.deltaTime;
            if (currentUndetectDelay < 0)
            {
                currentUndetectDelay = 0;
            }
        }

        if (!EnemyAI.playerDetected || wasHiding)
        {
            canHide = true;
        }
        else
        {
            canHide = false;
        }

        EnemyAI.playerHiding = !mouseMoved && !playerMoved && hiding && canHide;

        if (EnemyAI.playerHiding)
        {
            EnemyAI.playerDetected = false;
        }

        wasHiding = EnemyAI.playerHiding;
    }

    public void PlayerHiddenSound()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Hide and breath");
        }
        else if (Input.GetKeyUp("space"))
        {
            Debug.Log("not hidden");
        }
        EnemyAI.playerHiding = !mouseMoved && !playerMoved && hiding && !EnemyAI.playerDetected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private bool PlayerInLineOfSight()
    {
        Vector3 start = EnemyEyes.gameObject.transform.position;
        Vector3 target = PlayerEyes.gameObject.transform.position;
        Vector3 direction = target - start;

        float distance = (start - target).magnitude;

        RaycastHit sighttest;
        if (Physics.Raycast(start, direction, out sighttest, distance))
        {
            if (sighttest.collider.tag == "Player")
            {
                Debug.DrawRay(start, direction * distance, Color.red, 0.5f, false);
                return true;
            }
        }
        return false;
    }
}
