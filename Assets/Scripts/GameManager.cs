using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject Game;
	public GameObject Menu;
	public GameObject Player;
    public GameObject Enemy;

	public Transform Phase1Lighting;
	public Transform Phase2Lighting;

	public GameObject PickableFlashlight1;
	public GameObject PickableFlashlight2;

	public GameObject GameOverScreen;

	public int numberOfItemsPlaced;

    FMOD.Studio.EventInstance menuEvent;
    public FMOD.Studio.EventInstance musicEvent;

	public void Awake()
	{
		Instance = this;
	}

    void Start()
    {
		Cursor.visible = true;
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
        menuEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		musicEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.music);
		musicEvent.start();
        
	}

    void Update()
    {
        //musicEvent.setParameterByName("Items", numberOfItemsPlaced);
        //Debug.Log(numberOfItemsPlaced);
    }

	public void QuitGame()
	{
		Application.Quit();
	}

	public void GameOver()
	{
		Player.GetComponent<vp_FPInput>().enabled = false;
		Enemy.SetActive(false);
		GameOverScreen.SetActive(true);
		FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.playerDeath);
		musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		musicEvent.release();
	}

	public void RestartGame()
	{
		Player.GetComponent<vp_FPInput>().MouseCursorForced = true;
		Player.GetComponent<vp_FPInput>().enabled = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

	public void TransitiontoPhase2()
	{
		musicEvent.setParameterByName("TRX Part 2", 1f);
	}

	public void StartPhase2()
	{
		Phase1Lighting.gameObject.SetActive(false);
		Phase2Lighting.gameObject.SetActive(true);
		Enemy.SetActive(true);
		Enemy.GetComponent<EnemyAI>().Respawn();

		Player.GetComponent<Player>().InteractableMask |= (1 << LayerMask.NameToLayer("InteractablePhase2"));

		musicEvent.setParameterByName("TRX Part 2", 0f);


		if (!Player.GetComponent<Player>().HasFlashlight)
		{
			PickableFlashlight1.SetActive(false);
			PickableFlashlight2.SetActive(true);
		}
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
		Enemy.SetActive(true);
		Enemy.GetComponent<EnemyAI>().Respawn();
	}
}
