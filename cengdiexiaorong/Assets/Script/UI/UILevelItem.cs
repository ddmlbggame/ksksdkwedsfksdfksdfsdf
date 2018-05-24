using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILevelItem : MonoBehaviour {

	public Text Level;

	public GameObject Lock;

	private LevelData level_data;

	public void OnEnable()
	{
		EventTriggerClick.Get(this.gameObject).onClick = this._OnClick;
	}

	private void _OnClick(GameObject obj)
	{
		if(this.level_data!=null && GameData.GetPassedLevel(level_data.Level_Difficulty)>= level_data.CurrentLevel)
		{
			GameControl.Instance.PlayGame( GameType.Custom, this.level_data.Level_Difficulty, this.level_data.CurrentLevel);
			SDK.Instance.StartLevel(string.Format("进入关卡{0}难度{1}类型{2}",this.level_data.CurrentLevel, this.level_data.Level_Difficulty,GameType.Custom));
		}
	}

	public void Init(LevelData data)
	{
		this.level_data = data;
		if (data != null)
		{
			this.Level.text = data.CurrentLevel.ToString();
			int passed_level = GameData.GetPassedLevel(data.Level_Difficulty);
			this.Lock.SetActive(passed_level< data.CurrentLevel);
		}
	}

	public static UILevelItem Create(Transform parent)
	{
		var obj = Resources.Load("UI/UILevelItem");
		GameObject item = GameObject.Instantiate(obj) as GameObject;
		item.transform.SetParent(parent);
		item.transform.localPosition = Vector3.zero;
		item.transform.localScale = Vector3.one;
		item.transform.localRotation = Quaternion.identity;
		return item.GetComponent<UILevelItem>();
	}
}
