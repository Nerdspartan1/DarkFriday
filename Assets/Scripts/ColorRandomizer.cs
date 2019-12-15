using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
	public Renderer[] Renderers;

	private Material[] _materials;

	private void Awake()
	{
		_materials = new Material[Renderers.Length];
		//reference and clone materials
		for (int i = 0; i < _materials.Length; ++i)
		{
			_materials[i] = new Material(Renderers[i].material);
			Renderers[i].material = _materials[i];
		}
	}

	void Start()
    {
		Randomize();
	}

	public void Randomize()
	{
		for (int i = 0; i < _materials.Length; ++i)
		{
			_materials[i].color = ColorManager.Instance.GetRandomColor();
		}
	}

}
