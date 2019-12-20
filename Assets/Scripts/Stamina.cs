using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
	private vp_Activity _run;
	private CharacterController _controller;
	private vp_Controller _fpsController;
	private vp_FPInput _fpsInput;

	public RectTransform StaminaBar;
	private Image _barImage;
	private float _staminaBarInitialWidth;
	private float _nextTimeBarDisappear = 0f;

	[Range(0, 1f)]
	public float CurrentStamina = 1f;

	[Range(0, 1f)]
	public float MinimumStaminaRequiredToStartRunningOrHiding = 0.1f;

	public float RunDepletionRate = 0.05f;
	public float HideDepletionRate = 0.05f;
	public float RegenerationRate = 0.05f;

	public float RegenerationDelay = 1.5f;
	private float _resumeRenegerationTime = 0f;

	private bool _isRunning = false;
	public bool IsHiding = false;

	FMOD.Studio.EventInstance hiddenEvent;

	private void Start()
	{
		_staminaBarInitialWidth = StaminaBar.rect.width;
		_barImage = StaminaBar.GetComponentInChildren<Image>();
		var color = _barImage.color;
		color.a = 0f;
		_barImage.color = color;
		_controller = GetComponent<CharacterController>();
		_fpsController = GetComponent<vp_Controller>();
		_fpsInput = GetComponent<vp_FPInput>();
		_run = GetComponent<vp_FPPlayerEventHandler>().Run;
		_run.StartConditions += CanRun;
		_run.StopCallbacks += StopRunning;
		_run.StartCallbacks += StartRunning;

		hiddenEvent = FMODUnity.RuntimeManager.CreateInstance(SoundManager.sm.playerHide);
	}

	public bool CanRun()
	{
		return CurrentStamina > MinimumStaminaRequiredToStartRunningOrHiding;
	}

	public void StartRunning()
	{
		_isRunning = true;
	}

	public void StopRunning()
	{
		_isRunning = false;
		_resumeRenegerationTime = Time.time + RegenerationDelay;
	}

	public void StartHiding()
	{
		IsHiding = true;
		_fpsController.SetState("Freeze", true);
		_fpsInput.SetState("Freeze", true);
		FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Hiding", 1f);
		hiddenEvent.start();
	}

	public void StopHiding()
	{
		IsHiding = false;
		_fpsController.SetState("Freeze", false);
		_fpsInput.SetState("Freeze", false);
		FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Hiding", 0f);

		_resumeRenegerationTime = Time.time + RegenerationDelay;
	}

	// Update is called once per frame
	void Update()
    {

		//GLOBAL PARAMETER HIDING TRIGGER
		if (CurrentStamina > MinimumStaminaRequiredToStartRunningOrHiding &&
			Input.GetKeyDown(KeyCode.Space)) //start hiding
			StartHiding();
		else if (Input.GetKeyUp(KeyCode.Space))
			StopHiding();

		if (IsHiding)
		{
			CurrentStamina -= HideDepletionRate * Time.deltaTime;
			if (CurrentStamina < 0f)
			{
				CurrentStamina = 0f;
				StopHiding();
			}
		}
		else if (_isRunning)
		{
			if (_controller.velocity.sqrMagnitude > 0.1f)
			{
				CurrentStamina -= RunDepletionRate * Time.deltaTime;
				if (CurrentStamina < 0f)
				{
					CurrentStamina = 0f;
					_run.Stop();
				}
			}
		}
		else
		{
			if(Time.time > _resumeRenegerationTime)
			{
				if (CurrentStamina < 1f)
					CurrentStamina += RegenerationRate * Time.deltaTime;
				else if (CurrentStamina > 1f)
				{
					CurrentStamina = 1f;
					_nextTimeBarDisappear = Time.time + 1f;
				}
					
			}
		}


		UpdateStaminaBar();
	}

	public void UpdateStaminaBar()
	{
		if(CurrentStamina == 1f && Time.time > _nextTimeBarDisappear)
		{
			var color = _barImage.color;
			color.a = Mathf.Lerp(color.a, 0f, 0.1f);
			_barImage.color = color;
		}
		else if(CurrentStamina < 1f && _barImage.color.a < 0.99f)
		{
			var color = _barImage.color;
			color.a = Mathf.Lerp(color.a, 1f, 0.05f);
			_barImage.color = color;
		}

		Vector2 size = StaminaBar.sizeDelta;
		size.x = CurrentStamina * _staminaBarInitialWidth;
		StaminaBar.sizeDelta = size;
	}
}
