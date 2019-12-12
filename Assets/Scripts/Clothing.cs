using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothingType
{
	Top,
	Jacket,
	Pants,
	Shoes
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

	protected override void Start()
	{
		base.Start();
		var col = ColorManager.Instance.GetRandomColor(ClothingType);
		Debug.Log(col);
		Color = col;
	}

	public void UpdateName()
	{
		ItemName = ColorManager.ColorName(Material.color) + " " + ClothingName(ClothingType);
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
