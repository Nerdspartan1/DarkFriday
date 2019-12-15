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
	public Text CarriedClothingText;
	private CharacterController _controller;
	private vp_PlayerInventory _inventory;

	public float MaxInteractDistance = 2f;

	
	public float MinFootstepSpeed = 2f;
	public float FootstepsFrequencyFactor;
	private float _timeBeforeNextStep = 0f;
	private bool _hasFlashlight = false;
	private Light _flashlight;

	private Clothing _carriedClothing;
	public Clothing CarriedClothing { get => _carriedClothing; }

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

		if (NoteManager.Instance.NoteReader.activeInHierarchy && Input.GetKeyDown(KeyCode.E)) NoteManager.Instance.StopReadingNote();
		else if (!NoteManager.Instance.NoteReader.activeInHierarchy && Physics.Raycast(ray, out RaycastHit hit, MaxInteractDistance) && hit.collider.CompareTag("Interactable"))
		{
			var pickable = hit.collider.GetComponent<Pickable>();
			PickUpText.gameObject.SetActive(true);
			PickUpText.text = $"Press E to {pickable.PickUpLine} {pickable.ItemName}";
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

	public void TakeClothing(Clothing clothing)
	{
		if (_carriedClothing != null)
		{
			_carriedClothing.transform.SetPositionAndRotation(clothing.transform.position, clothing.transform.rotation);
			_carriedClothing.gameObject.SetActive(true);
		}
		clothing.gameObject.SetActive(false);
		_carriedClothing = clothing;

		UpdateCarryingText();

		FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.pickCloth);
	}

	public void RemoveClothingFromInventory()
	{
		_carriedClothing = null;
		UpdateCarryingText();
	}

	public void UpdateCarryingText()
	{
		if (_carriedClothing)
		{
			CarriedClothingText.text = _carriedClothing.ItemName;
			CarriedClothingText.color = _carriedClothing.Color;
			if (CarriedClothingText.color == Color.black) CarriedClothingText.color = Color.gray;
		}
		else
		{
			CarriedClothingText.text = "nothing";
			CarriedClothingText.color = Color.white;
		}
	}

	public void ToggleFlashlight()
	{
		if (_flashlight.enabled)
		{
			_flashlight.enabled = false;
			FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.flashlightButton);
		}
		else
		{
			_flashlight.enabled = true;
			FMODUnity.RuntimeManager.PlayOneShot(SoundManager.sm.flashlightButton);
		}
	}
}
