using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
		GameManager.Instance.StartGame();
    }

	public void Quit()
	{
		GameManager.Instance.QuitGame();
	}
}
