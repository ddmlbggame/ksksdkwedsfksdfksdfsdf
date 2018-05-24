using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameControl {
	public GameData game_data;

	private static GameControl _instance;

	public static Action RestartEvent;

	public static void HandleRestartEvent()
	{
		if (RestartEvent != null)
		{
			RestartEvent();
		}
	}

	public static Action FinishChallangeOneEvent;

	public static void HandleFinishChallangeOneEvent()
	{
		if (FinishChallangeOneEvent != null)
		{
			FinishChallangeOneEvent();
		}
	}
	public static Action FinishChallangeStop;

	public static void HandleFinishChallangeStop()
	{
		if (FinishChallangeStop != null)
		{
			FinishChallangeStop();
		}
	}


	public static GameControl Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameControl();
			}
			return _instance;
		}
	}

	public void Init(Transform parent)
	{
		FSoundManager.Init();
		game_data = new GameData();
		var obj = Resources.Load("UI/GamePlayPanel");
		if (obj != null)
		{
			GameObject.Instantiate(obj, UIManager.Instance.Game);
		}else
		{
			Debug.LogError("can't load UI/GamePlayPanel");
		}

	}

	public void PlayGame(GameType type , LevelDifficulty level_difficulty ,int level)
	{
		GameControl.Instance.game_data.currentGameLevel = level;
		GameControl.Instance.game_data.Current_Difficulty = level_difficulty;
		GameControl.Instance.game_data._current_game_type = type;
		if(type == GameType.challenge)
		{
			game_data.ResetChallangeData();
		}
		UIManager.Instance.PushShow(UIMain.Info,true);
		GameScene.Instance.SetGameStart();
	}


	public void DoGameOver()
	{
		game_data.isGamePlay = false;
		UnityEngine.Debug.Log("Finished");
		SDK.Instance.FinishLevel(string.Format("完成关卡{0}难度{1}模式{2}",GameControl.Instance.game_data.Current_Difficulty, GameControl.Instance.game_data.currentGameLevel, GameControl.Instance.game_data._current_game_type));
		if (GameControl.Instance.game_data._current_game_type == GameType.Custom)
		{
			FSoundManager.StopMusic();
			int level = GameData.GetPassedLevel(GameControl.Instance.game_data.Current_Difficulty);
			if(GameControl.Instance.game_data.currentGameLevel == level)
			{
				int max_level = GameControl.Instance.game_data.GetLevelDatas(GameControl.Instance.game_data.Current_Difficulty).Count;
				int passed = level;
				if (max_level > level)
				{
				    passed = level + 1;
				}
				GameData.SetPassedLevel(GameControl.Instance.game_data.Current_Difficulty, passed);
			}
			FSoundManager.PlaySound("Cheers");
			UIResult.result = true;
			UIManager.Instance.PushShow(UIResult.Info, false);
		}
		else
		{
			int max_level = GameData.GetChanllangeLevel();
			if (GameControl.Instance.game_data.ChallangePassedNumber > max_level)
			{
				GameData.SetChallangeLevel(GameControl.Instance.game_data.ChallangePassedNumber);
			}
			FSoundManager.PlaySound("Success");
			GameControl.Instance.game_data.ChallangePassedNumber++;
			GameControl.Instance.game_data.ChallangeRestTime = GameControl.Instance.game_data.ChallangeTime;
			HandleFinishChallangeStop();
			GameControl.Instance.game_data.isGamePlay = false;
			UIResult.result = true;
			UIManager.Instance.PushShow(UIResult.Info, false);

		}
		
	}

	public void ChallangeGameFinshed()
	{
		if (GameControl.Instance.game_data.ChallangePassedNumber > 0)
		{
			SDK.Instance.ReportScore(GameControl.Instance.game_data.ChallangePassedNumber);
		}
		SDK.Instance.FinishLevel("挑战模式结束");
		FSoundManager.StopMusic();
		FSoundManager.PlaySound("Success");
		GameControl.Instance.game_data.isGamePlay = false;
		UIResult.result = false;
		UIManager.Instance.PushShow(UIResult.Info, false);

	}

	public static string SetTimeFormat(int time)
	{
		int second = time % 60;
		int min = (time % 3600 - second) / 60;
		return string.Format("{0}:{1}", min.ToString("00"), second.ToString("00"));
	}

	//public results_time1 = 30;
	public static string BeatPeple(int time ,GameType type)
	{
		float random = UnityEngine.Random.Range(70, 98);
		return string.Format("超越了全球{0}%的玩家", random);
	}
}
