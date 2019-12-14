using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Pickable
{
	[TextArea(4,10)]
	public string Text;

	public override void PickUp()
	{
		NoteManager.Instance.ReadNote(this);
	}

	public void SetClothingType(ClothingType clothingType)
	{
		Dictionary<Season, Color> colors = new Dictionary<Season, Color>();
		colors.Add(Season.Winter, ColorManager.Instance.GetRandomSeasonColor(Season.Winter));
		colors.Add(Season.Spring, ColorManager.Instance.GetRandomSeasonColor(Season.Spring));
		colors.Add(Season.Summer, ColorManager.Instance.GetRandomSeasonColor(Season.Summer));
		colors.Add(Season.Autumn, ColorManager.Instance.GetRandomSeasonColor(Season.Autumn));

		colors[ColorManager.Instance.CurrentSeason] = ColorManager.Instance.CorrectColor[clothingType];

		switch (clothingType)
		{
			case ClothingType.Jacket:
				Text = "Hey Tommy, you have to set the jackets on the mannequins today. " +
					"Please get the colors right!\n";
				break;
			case ClothingType.Pants:
				Text = "Tommy, stop messing up the pants colors! " +
					"For the last time, for the pants it's like this:\n";
				break;
			case ClothingType.Top:
				Text = "Tommy, I need you to put the T-shirts of the right seasonal color " +
					"on the mannequins. Remember the scheme :\n";
				break;
			case ClothingType.Shoes:
				Text = "Sally, the intern keeps mixing up the shoes seasonal colors on " +
					"the mannequins. Could you take care of it? The scheme is :\n";
				break;
		}

		Text += $"Winter : {ColorManager.ColorName(colors[Season.Winter])}\n" +
					$"Spring : {ColorManager.ColorName(colors[Season.Spring])}\n" +
					$"Summer : {ColorManager.ColorName(colors[Season.Summer])}\n" +
					$"Autumn : {ColorManager.ColorName(colors[Season.Autumn])}\n";

	}

	public void SetSeasonText(Season season)
	{
		string seasonString = "";
		switch (season)
		{
			case Season.Winter: seasonString = "WINTER"; break;
			case Season.Spring: seasonString = "SPRING"; break;
			case Season.Summer: seasonString = "SUMMER"; break;
			case Season.Autumn: seasonString = "AUTUMN"; break;
		}
		Text = "Note for Tommy : today you need to dress the mannequins " +
			$"accordingly to the {seasonString} season. There is a very " +
			$"specific and precise color pattern that you have to respect when " +
			$"dressing the mannequins. They and I will get very angry " +
			$"if you mess up the colors, alright ?\n" +
			$"There are some notes lying around explaining the color scheme " +
			$"that you have to follow for EACH clothing article : top, pants, shoes and jacket.\n" +
			$"Just look around a bit to find them and PLEASE DON'T MESS UP LIKE USUAL";
	}
}

