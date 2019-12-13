using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public EnemyAI EnemyAI;
    public KeyCode hidingKey;
    public GameObject EnemyEyes;
    public GameObject PlayerEyes;

    private bool playerInRange = false;
    private bool playerMoved = false;
    private bool mouseMoved = false;
    private bool hiding = false;

    private Vector3 lastMousePos;
    private Vector3 lastPlayerPos;

    void Start()
    {
        lastPlayerPos = GameManager.Instance.Player.transform.position;
        lastMousePos = Input.mousePosition;
    }

    void FixedUpdate()
    {
        mouseMoved = Input.mousePosition != lastMousePos;
        lastMousePos = Input.mousePosition;

        playerMoved = GameManager.Instance.Player.transform.position != lastPlayerPos;
        lastPlayerPos = GameManager.Instance.Player.transform.position;

        hiding = Input.GetKey(hidingKey);

        // Player can only hide when not in line of sight & out of range and cannot be detected when hiding
        if (EnemyAI.playerHiding)
        {
            EnemyAI.playerDetected = false;
        }
        else
        {
            EnemyAI.playerDetected = playerInRange && PlayerInLineOfSight();
        }
        EnemyAI.playerHiding = !mouseMoved && !playerMoved && hiding && !EnemyAI.playerDetected;
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
