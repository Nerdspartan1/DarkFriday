using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public EnemyAI EnemyAI;
    public KeyCode hidingKey;
    
    private bool playerInRange = false;
    private bool playerMoved = false;
    private bool mouseMoved = false;
    private bool hiding = false;

    private Vector3 lastMousePos;
    private Vector3 lastPlayerPos;
    
    void Start()
    {
        lastPlayerPos = EnemyAI.Player.gameObject.transform.position;
        lastMousePos = Input.mousePosition;
    }

    void FixedUpdate()
    {
        mouseMoved = Input.mousePosition != lastMousePos;
        lastMousePos = Input.mousePosition;

        playerMoved = EnemyAI.Player.transform.position != lastPlayerPos;
        lastPlayerPos = EnemyAI.Player.gameObject.transform.position;

        hiding = Input.GetKey(hidingKey);

        EnemyAI.playerDetected = false;
        if (playerInRange && PlayerInLineOfSight())
        {
            if(mouseMoved || playerMoved || !hiding)
            {
                EnemyAI.playerDetected = true;
            }
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
        Vector3 start = EnemyAI.gameObject.transform.position;
        Vector3 target = EnemyAI.Player.gameObject.transform.position;
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
