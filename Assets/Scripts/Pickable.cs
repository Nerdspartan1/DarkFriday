using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
	private Material _highlightMaterial;
	private float _highlightDecaySpeed = 2f;
	private float _currentHighlightIntensity = 0f;

	public float OutlineWidth = 0.01f;
	public UnityEvent OnPickUp;

    // Start is called before the first frame update
    void Start()
    {
		var renderer = GetComponent<Renderer>();
		_highlightMaterial = new Material(renderer.material);
		renderer.material = _highlightMaterial;
    }

	public void Highlight()
	{
		_currentHighlightIntensity = 1f;
	}

	// Update is called once per frame
	void Update()
	{
		_highlightMaterial.SetFloat("_Outline", _currentHighlightIntensity * OutlineWidth);
		if (_currentHighlightIntensity > 0)
			_currentHighlightIntensity -= _highlightDecaySpeed * Time.deltaTime;
		else
			_currentHighlightIntensity = 0f;
	}

	public void PickUp()
	{
		OnPickUp.Invoke();
		gameObject.SetActive(false);
	}
}
