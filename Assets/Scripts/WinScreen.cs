using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
	public bool CanPressKeyToEndGame = false;

	public void AllowKeyPressToEndGame()
	{
		CanPressKeyToEndGame = true;
	}

	private void Update()
	{
		if (CanPressKeyToEndGame && Input.anyKeyDown)
		{
			GameManager.Instance.RestartGame();
		}
	}
}
