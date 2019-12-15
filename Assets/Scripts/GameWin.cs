using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MonoBehaviour
{
	public GameObject GameWinScreen;
	public bool _canPressKeyToEndGame = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameManager.Instance.Enemy.SetActive(false);
			GameWinScreen.SetActive(true);
			GameManager.Instance.Player.GetComponent<Player>().enabled = false;
			GameManager.Instance.Player.GetComponent<vp_Controller>().SetState("Freeze");
		}
	}

}
