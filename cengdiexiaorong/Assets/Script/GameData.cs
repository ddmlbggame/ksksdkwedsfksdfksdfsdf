using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData {

	// 简单难度
	private Dictionary<int , LevelData> simple_level_datas;

	private Dictionary<int, LevelData> normal_level_datas;

	private Dictionary<int, LevelData> hard_level_datas;

	private Dictionary<int, LevelData> _abnormal_level_datas;

	public bool doing_show_tip = false;

	public int currentGameLevel;

	public LevelDifficulty Current_Difficulty = LevelDifficulty.Simple;

	public GameType _current_game_type;

	public int ChallangeTime = 11;

	public bool isGamePlay = false;
	// 挑战模式完成的关卡数
	public int ChallangePassedNumber = 0;

	public int ChallangeRestTime = 0;

	public bool canshowtip = false;

	public void ResetChallangeData()
	{
		ChallangeRestTime = ChallangeTime;
		ChallangePassedNumber = 0;
	}

	public static string[] CurrentPassedLevel = {
		Enum.GetName(typeof(LevelDifficulty), LevelDifficulty.Simple),
		Enum.GetName(typeof(LevelDifficulty), LevelDifficulty.Normal),
		Enum.GetName(typeof(LevelDifficulty), LevelDifficulty.Hard),
		Enum.GetName(typeof(LevelDifficulty), LevelDifficulty.Abnormal) };

	private static int[] CurrentPassedSimpleLevel = new int[4];

	public void Init()
	{
		this.simple_level_datas = new Dictionary<int, LevelData>();
		this.normal_level_datas = new Dictionary<int, LevelData>();
		this.hard_level_datas = new Dictionary<int, LevelData>();
		this._abnormal_level_datas = new Dictionary<int, LevelData>();
		_Init();
	}

	private string[] _InitLevel()
	{
		TextAsset map = Resources.Load("gamedatas/level") as TextAsset;
		string mapText = map.text;
		string[] lines = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
		return lines;
	}

	private void _Init()
	{

		//List<string> list = new List<string>();
		//list.Add("1|0|0,0,0,20,30;1,0,0,20,30;2,0,0,20,30;7,0,0,20,30");
		//list.Add("2|0;0,-100,100,-160,160;0,80,-140,140,160;3,0,0,-160,-140;7,0,0,140,-140");
		var levels = _InitLevel();
		for (int i = 0; i < levels.Length; i++)
		{
			//Debug.LogError(levels[i].ToString());
			//if (string.IsNullOrEmpty(levels[i].ToString()))
			//{
			//	continue;
			//}
			char[] split = { ',',';' ,'|'};
			string[] array = levels[i].ToString().Split(split);
			LevelData level_data = new LevelData();
			level_data.CurrentLevel = int.Parse(array[0]);
			level_data.Level_Difficulty = (LevelDifficulty) int.Parse(array[1]);
			level_data.ImageDatas = new List<ImageData>();
			for (int j = 2; j < array.Length; j += 5)
			{
				ImageData image_data = new ImageData();
				image_data.ImageType = (ImageType)int.Parse(array[j]);
				image_data.ImagePosition = new Vector3(float.Parse(array[j + 1]), float.Parse(array[j + 2]), 0f);
				image_data.OperationalImagePosition = new Vector3(float.Parse(array[j + 3]), float.Parse(array[j + 4]));
				level_data.ImageDatas.Add(image_data);
			}
			switch (level_data.Level_Difficulty)
			{
				case LevelDifficulty.Simple:
					if (!simple_level_datas.ContainsKey(level_data.CurrentLevel))
					{
						simple_level_datas.Add(level_data.CurrentLevel, level_data);
					}else
					{
						Debug.LogError("关卡重复添加 level="+level_data.CurrentLevel);
					}
					
					break;
				case LevelDifficulty.Normal:
					normal_level_datas.Add(level_data.CurrentLevel, level_data);
					break;
				case LevelDifficulty.Hard:
					hard_level_datas.Add(level_data.CurrentLevel, level_data);
					break;
				case LevelDifficulty.Abnormal:
					_abnormal_level_datas.Add(level_data.CurrentLevel, level_data);
					break;
			}
		}
	}

	public LevelData GetLevelData(int level , LevelDifficulty level_difficult)
	{
		Dictionary<int, LevelData> datas = null;
		switch (level_difficult)
		{
			case LevelDifficulty.Simple:
				datas = simple_level_datas;
				break;
			case LevelDifficulty.Normal:
				datas = normal_level_datas;
				break;
			case LevelDifficulty.Hard:
				datas = hard_level_datas;
				break;
			case LevelDifficulty.Abnormal:
				datas = _abnormal_level_datas;
				break;
		}
		if (datas.ContainsKey(level))
		{
			return datas[level];
		}
		return null;
	}

	public Dictionary<int, LevelData> GetLevelDatas(LevelDifficulty level_difficult)
	{
		Dictionary<int, LevelData> datas = null;
		switch (level_difficult)
		{
			case LevelDifficulty.Simple:
				datas = simple_level_datas;
				break;
			case LevelDifficulty.Normal:
				datas = normal_level_datas;
				break;
			case LevelDifficulty.Hard:
				datas = hard_level_datas;
				break;
			case LevelDifficulty.Abnormal:
				datas = _abnormal_level_datas;
				break;
		}
		return datas;
	}


	public static int GetPassedLevel(LevelDifficulty level_difficulty)
	{
		int passed_level =  PlayerPrefs.GetInt(Enum.GetName(typeof(LevelDifficulty),level_difficulty));
		if (passed_level <= 0)
		{
			return 1;
		}
		if (CurrentPassedSimpleLevel[(int)level_difficulty] <= 0)
		{
			CurrentPassedSimpleLevel[(int)level_difficulty] = passed_level;
		}else
		{
			return CurrentPassedSimpleLevel[(int)level_difficulty];
		}
		
		return passed_level;
	}
	public static void SetPassedLevel(LevelDifficulty level_difficulty ,int level)
	{
	    CurrentPassedSimpleLevel[(int)level_difficulty] = level;
		PlayerPrefs.SetInt(Enum.GetName(typeof(LevelDifficulty), level_difficulty), level);
	}

	public static void SetChallangeLevel(int level)
	{
		PlayerPrefs.SetInt("chanllange", level);
	}
	public static int GetChanllangeLevel()
	{
		int passed_level = PlayerPrefs.GetInt("chanllange");
		
		return passed_level;
	}

	public static void SetPursedRemoveAds(bool pursed)
	{
		PlayerPrefs.SetInt("removeads", pursed?1:0);
	}
	public static bool GetPursedRemoveAds()
	{
		int pursed = PlayerPrefs.GetInt("removeads");

		return pursed==1;
	}


	public void SetRandomLevel()
	{
		GameControl.Instance.game_data.currentGameLevel = UnityEngine.Random.Range(1, 7);
		GameControl.Instance.game_data.Current_Difficulty = LevelDifficulty.Simple;
	}

	public static int GetRecord(LevelDifficulty level_difficulty ,int level )
	{
		string name = Enum.GetName(typeof(LevelDifficulty), level_difficulty) + level;
		int record = PlayerPrefs.GetInt(name);
		return record;
	}

	public static void SetRecord(LevelDifficulty level_difficulty, int level,int time)
	{
		string name = Enum.GetName(typeof(LevelDifficulty), level_difficulty) + level;
		PlayerPrefs.SetInt(name, time);
	}
}

public enum GameType
{
	Custom,
	challenge,
}
