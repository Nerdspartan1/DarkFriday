using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[HideInInspector]
	public Camera FPSCamera;
	public Text PickUpText;

	public float MaxInteractDistance = 2f;

	private CharacterController _controller;
	public float MinSpeed = 2f;
	public float FrequencyFactor;
	private float _timeBeforeNextStep = 0f;

	public Clothing CarriedClothing;

    // Start is called before the first frame update
    void Start()
    {
		FPSCamera = GetComponentInChildren<Camera>();
		PickUpText.gameObject.SetActive(false);
		_controller = GetComponent<CharacterController>();
	}

    // Update is called once per frame
    void Update()
    {
		Ray ray = new Ray(FPSCamera.transform.position, FPSCamera.transform.forward);
		
		if (Physics.Raycast(ray, out RaycastHit hit, MaxInteractDistance) && hit.collider.CompareTag("Interactable"))
		{
			var pickable = hit.collider.GetComponent<Pickable>();
			PickUpText.gameObject.SetActive(true);
			PickUpText.text = $"Press E to pick up {pickable.ItemName}";
			pickable.Highlight();
			if (Input.GetKeyDown(KeyCode.E))
			{
				pickable.PickUp();
			}
		}
		else
		{
			PickUpText.gameObject.SetActive(false);
		}

		/*if (_controller.velocity.magnitude >= 0.01f)
		{
			FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.playerFS);
		}
		else if (_controller.velocity.magnitude == 0)
		{
			Debug.Log("Player not moving");
		}*/

		var speed = _controller.velocity.magnitude;
		if (speed < MinSpeed)
		{
			_timeBeforeNextStep = 0.1F / (MinSpeed * FrequencyFactor);
		}
		else
		{
			if (_timeBeforeNextStep <= 0f)
			{
				FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.playerFS);

				_timeBeforeNextStep = 1 / (speed * FrequencyFactor);
			}
			_timeBeforeNextStep -= Time.deltaTime;
		}

	}
}
