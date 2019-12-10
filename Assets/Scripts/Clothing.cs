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
			Material.color = value;
			UpdateName();
		}
	}

	public ClothingType ClothingType;

	protected override void Start()
	{
		base.Start();
		UpdateName();
	}

	public void UpdateName()
	{
		ItemName = ColorName(Material.color) + ClothingName(ClothingType);
	}

	public static string ColorName(Color color)
	{
		if (color == Color.red)
			return "red";
		else if (color == Color.blue)
			return "blue";
		else if (color == Color.green)
			return "green";
		else if (color == Color.yellow)
			return "yellow";
		else if (color == Color.magenta)
			return "magenta";
		else if (color == Color.cyan)
			return "cyan";
		else if (color == Color.black)
			return "black";
		else if (color == Color.white)
			return "white";
		else
			return "";
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
