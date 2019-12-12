using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
	protected Material Material;
	private float _highlightDecaySpeed = 2f;
	private float _currentHighlightIntensity = 0f;

	public float OutlineWidth = 0.01f;

	[HideInInspector]
	public string ItemName;

    // Start is called before the first frame update
    protected virtual void Start()
    {
		tag = "Interactable";
		var renderer = GetComponent<Renderer>();
		Material = new Material(renderer.material);
		Material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
		Material.SetColor("_OutlineColor", Color.yellow);
		renderer.material = Material;
    }

	public void Highlight()
	{
		_currentHighlightIntensity = 1f;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		Material.SetFloat("_Outline", _currentHighlightIntensity * OutlineWidth);
		if (_currentHighlightIntensity > 0)
			_currentHighlightIntensity -= _highlightDecaySpeed * Time.deltaTime;
		else
			_currentHighlightIntensity = 0f;
	}

	public virtual void PickUp()
	{
	}
}
