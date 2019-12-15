using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject Game;
	public GameObject Menu;
	public GameObject Player;
    public GameObject Enemy;

    public int numberOfItemsPlaced;

    FMOD.Studio.EventInstance menuEvent;

	public void Awake()
	{
		Instance = this;
	}

    void Start()
    {
		Game.SetActive(false);
		Menu.SetActive(true);
		Player.SetActive(false);
        Enemy.SetActive(false);

        menuEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.menu);
        menuEvent.start();
        

    }

	public void StartGame()
	{
		Menu.SetActive(false);
		Game.SetActive(true);
		Player.SetActive(true);
        menuEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        // TODO Activate Enemy in Part 1
        ActivateEnemy();
        
	}

	public void QuitGame()
	{
		Application.Quit();
	}

    public void EnemyCooldown(float cooldownTime)
    {
        StartCoroutine("StartEnemyCooldown", cooldownTime);
    }
    
    public IEnumerator StartEnemyCooldown(float cooldownTime)
    {
        // Give some time to walk away;
        yield return new WaitForSeconds(5);
        Enemy.SetActive(false);
        yield return new WaitForSeconds(cooldownTime);
        ActivateEnemy();
    }

    private void ActivateEnemy()
    {
        Enemy.SetActive(true);
        Enemy.GetComponent<EnemyAI>().Respawn();
    }
}
