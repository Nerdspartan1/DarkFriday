using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject Game;
	public GameObject Menu;
	public GameObject Player;

	public void Awake()
	{
		Instance = this;
	}

    void Start()
    {
		Game.SetActive(false);
		Menu.SetActive(true);
		Player.SetActive(false);
    }

	public void StartGame()
	{
		Menu.SetActive(false);
		Game.SetActive(true);
		Player.SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}


}
