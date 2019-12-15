using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMannequin : MonoBehaviour
{
	private Animator _anim;

	void Start()
	{
		_anim = GetComponent<Animator>();
		_anim.SetInteger("pose", Random.Range(0, 4));
	}
}
