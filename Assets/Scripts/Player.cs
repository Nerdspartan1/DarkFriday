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

    // Start is called before the first frame update
    void Start()
    {
		FPSCamera = GetComponentInChildren<Camera>();
		PickUpText.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		Ray ray = new Ray(FPSCamera.transform.position, FPSCamera.transform.forward);
		
		if (Physics.Raycast(ray, out RaycastHit hit, MaxInteractDistance) && hit.collider.CompareTag("Interactable"))
		{
			PickUpText.gameObject.SetActive(true);
			var pickable = hit.collider.GetComponent<Pickable>();
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
	}
}
