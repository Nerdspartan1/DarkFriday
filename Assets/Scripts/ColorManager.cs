using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Season
{
	Winter=0,
	Spring=1,
	Summer=2,
	Autumn=3
}

public class ColorManager : MonoBehaviour
{
	public static ColorManager Instance;

	public Color[] WinterColors;
	public Color[] SpringColors;
	public Color[] SummerColors;
	public Color[] AutumnColors;

	public int MinimumAmountOfCorrectClothes = 1;

	public Dictionary<ClothingType, int> AmountOfCorrectClothings = new Dictionary<ClothingType, int>(){
		{ClothingType.Jacket,0 },
		{ClothingType.Top, 0 },
		{ClothingType.Pants, 0},
		{ClothingType.Shoes, 0}
	};

	public Season CurrentSeason;

	private void Awake()
	{
		Instance = this;
		CurrentSeason = (Season)Random.Range(0, 3);
	}


	public Color GetRandomColor(ClothingType clothingType)
	{
		Season season;
		if (AmountOfCorrectClothings[clothingType] < MinimumAmountOfCorrectClothes)
			season = CurrentSeason;
		else
			season = (Season)Random.Range(0, 3);

		if(season == CurrentSeason) AmountOfCorrectClothings[clothingType]++;

		switch (season)
		{
			case Season.Winter:
				return WinterColors[Random.Range(0, WinterColors.Length)];
			case Season.Spring:
				return SpringColors[Random.Range(0, SpringColors.Length)];
			case Season.Summer:
				return SummerColors[Random.Range(0, SummerColors.Length)];
			case Season.Autumn:
				return AutumnColors[Random.Range(0, AutumnColors.Length)];
			default:
				throw new System.Exception("Unknown season");
		}
	}

	public static string ColorName(Color color)
	{
		if (color == Color.red)
			return "red";
		else if (color == Color.blue)
			return "blue";
		else if (color == Color.green)
			return "green";
		else if (color == new Color(1,1,0)) //yellow
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
}
