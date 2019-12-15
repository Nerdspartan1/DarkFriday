using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mannequin : Pickable
{
	// Top=0,
	// Jacket=1,
	// Pants=2,
	// Shoes=3
	[Tooltip("Top=0,Jacket=1,Pants=2,Shoes=3")]
	public Renderer[] Renderers;

	private bool[] _hasClothing = new bool[4];

	private Material[] _material = new Material[4];

	private Animator _anim;

	public UnityEvent OnEntirelyDressed;

    void Start()
    {
		_anim = GetComponent<Animator>();
		_anim.SetInteger("pose", Random.Range(0, 4));

		PickUpLine = "dress mannequin";
		//reference and clone materials
		for(int i = 0; i < 4; ++i)
		{
			_material[i] = new Material(Renderers[i].material);
			Renderers[i].material = _material[i];
			_hasClothing[i] = Renderers[i].gameObject.activeInHierarchy;
		}
	}

	public override void PickUp()
	{
		var player = GameManager.Instance.Player.GetComponent<Player>();
		if (player.CarriedClothing)
		{
			var clothingType = player.CarriedClothing.ClothingType;
			if (!_hasClothing[(int)clothingType])
			{
				if(player.CarriedClothing.Color == ColorManager.Instance.CorrectColor[clothingType])
				{
					DressUp(player.CarriedClothing);
					player.RemoveClothingFromInventory();
				}
				else
				{
					//wrong color
				}
			}
			else
			{
				//already has this clothing type
			}

		}
	}

	public void DressUp(Clothing clothing)
	{
		_hasClothing[(int)clothing.ClothingType] = true;
		_material[(int)clothing.ClothingType].color = clothing.Color;
		Renderers[(int)clothing.ClothingType].gameObject.SetActive(true);

		GameManager.Instance.numberOfItemsPlaced++;
		//dynamically change music

		bool fullyDressed = true;
		foreach (bool b in _hasClothing)
			fullyDressed &= b;
		if (fullyDressed) OnEntirelyDressed.Invoke();
	}

}
