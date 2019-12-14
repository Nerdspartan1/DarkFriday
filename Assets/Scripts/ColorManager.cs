using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

	public Transform Clothings;

	public Color[] WinterColors;
	public Color[] SpringColors;
	public Color[] SummerColors;
	public Color[] AutumnColors;

	public int MinimumAmountOfCorrectClothes = 1;

	public Dictionary<ClothingType, Color> CorrectColor = new Dictionary<ClothingType, Color>();

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
		CorrectColor.Add(ClothingType.Jacket, GetRandomSeasonColor(CurrentSeason));
		CorrectColor.Add(ClothingType.Top, GetRandomSeasonColor(CurrentSeason));
		CorrectColor.Add(ClothingType.Pants, GetRandomSeasonColor(CurrentSeason));
		CorrectColor.Add(ClothingType.Shoes, GetRandomSeasonColor(CurrentSeason));
	}

	public void Start()
	{
		PlaceColoredClothes();
	}

	public Color[] GetSeasonColors(Season season)
	{
		switch (season)
		{
			case Season.Winter:
				return WinterColors;
			case Season.Spring:
				return SpringColors;
			case Season.Summer:
				return SummerColors;
			case Season.Autumn:
				return AutumnColors;
			default:
				throw new System.Exception("Unknown season");
		}
	}

	public Color GetRandomSeasonColor(Season season)
	{
		var colors = GetSeasonColors(season);
		return colors[Random.Range(0, colors.Length)];
	}

	public Color GetRandomColor()
	{
		var season = (Season)Random.Range(0, 3);
		return GetRandomSeasonColor(season);
	}

	public void PlaceColoredClothes()
	{
		List<Clothing> clothings = Clothings.GetComponentsInChildren<Clothing>().ToList();
		clothings.RandomizeList();
		foreach(var clothing in clothings)
		{
			if(AmountOfCorrectClothings[clothing.ClothingType] < MinimumAmountOfCorrectClothes)
			{
				clothing.Color = CorrectColor[clothing.ClothingType];
				AmountOfCorrectClothings[clothing.ClothingType]++;
			}
			else
			{
				clothing.Color = GetRandomColor();
			}
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
