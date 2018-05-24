using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelData {

	public int CurrentLevel;

	public LevelDifficulty Level_Difficulty;

	public List<ImageData> ImageDatas;

}

public struct ImageData
{
	public ImageType ImageType;

	public Vector2 ImagePosition;

	public Vector2 OperationalImagePosition;
}

public enum LevelDifficulty
{
	Simple,
	Normal,
	Hard,
	Abnormal,
}