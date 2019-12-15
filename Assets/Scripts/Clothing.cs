using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingType
{
	Top=0,
	Jacket=1,
	Pants=2,
	Shoes=3
}

public class Clothing : Pickable
{
	public Color Color {
		get => Material.color;
		set
		{
			Material.SetColor("_Color", value);
			UpdateName();
		}
	}

	public ClothingType ClothingType;

	public void UpdateName()
	{
		ItemName = ColorManager.Instance.ColorName(Material.color) + " " + ClothingName(ClothingType);
	}

	public override void PickUp()
	{
		GameManager.Instance.Player.GetComponent<Player>().TakeClothing(this);

		base.PickUp();
		Debug.Log("picked up");
	}

	public static string ClothingName(ClothingType type)
	{
		switch (type)
		{
			case ClothingType.Top:
				return "top";
			case ClothingType.Pants:
				return "pants";
			case ClothingType.Shoes:
				return "shoes";
			case ClothingType.Jacket:
				return "jacket";
			default:
				throw new System.Exception("Unkown clothing type");
		}
	}
}
