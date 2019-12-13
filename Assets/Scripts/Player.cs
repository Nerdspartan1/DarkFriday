using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[HideInInspector]
	public Camera FPSCamera;

	[Header("References")]
	public Text PickUpText;
	public GameObject FlashlightObject;
	private CharacterController _controller;
	private vp_PlayerInventory _inventory;

	public float MaxInteractDistance = 2f;

	
	public float MinFootstepSpeed = 2f;
	public float FootstepsFrequencyFactor;
	private float _timeBeforeNextStep = 0f;
	private bool _hasFlashlight = false;
	private Light _flashlight;

	public Clothing CarriedClothing;

    void Start()
    {
		FPSCamera = GetComponentInChildren<Camera>();
		PickUpText.gameObject.SetActive(false);
		_controller = GetComponent<CharacterController>();
		_inventory = GetComponent<vp_PlayerInventory>();
		_flashlight = FlashlightObject.GetComponentInChildren<Light>();
		_flashlight.enabled = false;
	}

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

		if (_hasFlashlight && Input.GetKeyDown(KeyCode.F)) ToggleFlashlight();


		/*if (_controller.velocity.magnitude >= 0.01f)
		{
			FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.playerFS);
		}
		else if (_controller.velocity.magnitude == 0)
		{
			Debug.Log("Player not moving");
		}*/

		var speed = _controller.velocity.magnitude;
		if (speed < MinFootstepSpeed)
		{
			_timeBeforeNextStep = 0.1F / (MinFootstepSpeed * FootstepsFrequencyFactor);
		}
		else
		{
			if (_timeBeforeNextStep <= 0f)
			{
				FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.playerFS);

				_timeBeforeNextStep = 1 / (speed * FootstepsFrequencyFactor);
			}
			_timeBeforeNextStep -= Time.deltaTime;
		}

	}

	public void GiveFlashlight()
	{
		_hasFlashlight = true;
		_flashlight.enabled = true;
		//sound : pick up flashlight
	}

	public void ToggleFlashlight()
	{
		if (_flashlight.enabled)
		{
			_flashlight.enabled = false;
			//sound : turn off flashlight
		}
		else
		{
			_flashlight.enabled = true;
			//sound : turn on flashlight
		}
	}
}
