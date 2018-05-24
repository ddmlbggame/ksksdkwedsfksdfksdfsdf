using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UIManager {

	private static UIManager _instance;

	public static UIManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new UIManager();
			}
			return _instance;
		}
	}
	public const string UIRootPath = "UI/CharactorTran/Prefab/";

	private GameObject _canvas;

	public Canvas canvas;

	public Transform Dialog;

	public Transform Normal;

	public Transform Top;

	public Transform Game;

	private Stack<UIBase> ui_queue = new Stack<UIBase>();

	private Dictionary<UIInfo, UIBase> ui_dictionary = new Dictionary<UIInfo, UIBase>();

	private UIBase _current_ui = null;

	public void _Init(Transform parent)
	{
		var canvas = Resources.Load("UI/UICanvas");
		this._canvas = GameObject.Instantiate(canvas) as GameObject;
		this.canvas = this._canvas.GetComponent<Canvas>();
		this._canvas.transform.SetParent(parent);
		this.Dialog = this._canvas.transform.Find("Dialog").transform;
		this.Normal = this._canvas.transform.Find("Normal").transform;
		this.Top = this._canvas.transform.Find("Top").transform;
		this.Game = this._canvas.transform.Find("Game").transform;
	}

	public void PushShow(UIInfo info , bool is_push = false)
	{
		UIBase ui = null;
		Transform parent = null;
		switch (info.UI_Hierarchy_Type)
		{
			case UIHierarchyType.Normal:
				parent = this.Normal.transform;
				break;
			case UIHierarchyType.Dialog:
				parent = this.Dialog.transform;
				break;
			case UIHierarchyType.Top:
				parent = this.Top.transform;
				break;
		}
		if (this.ui_dictionary.ContainsKey(info))
		{
			ui = this.ui_dictionary[info];
		}
		else
		{
			var ui_obj = Resources.Load(UIManager.UIRootPath +info.Path);
			if (ui_obj != null)
			{
				GameObject ui_game_obj = GameObject.Instantiate(ui_obj , parent) as GameObject;
				ui = ui_game_obj.GetComponent<UIBase>();
				this.ui_dictionary.Add(info, ui);
			}
			else
			{
				Debug.LogError("can't load ui path =" + info.Path);
			}
	
		}

		if (is_push)
		{
			UIBase peek_ui = null;
			if (this.ui_queue.Count > 0)
			{
				peek_ui = this.ui_queue.Peek();
				this._Hide(peek_ui);
			}
			ui_queue.Push(ui);
		}
		this._current_ui = ui;
		this._Show(ui);
	}

	public void PopShow()
	{
		if (this.ui_queue.Count > 0)
		{
			UIBase ui = this.ui_queue.Pop();
			_Hide(ui);
			if (this.ui_queue.Count > 0)
			{
				UIBase peek_ui = this.ui_queue.Peek();
				this._Show(peek_ui);
			}else
			{
				Debug.Log("current ui stack count = 0");
			}
		}
		else
		{
			Debug.LogError("current ui stack count = 0");
		}
	}

	public void Hide(UIInfo info)
	{
		UIBase ui = null;
		if(this.ui_dictionary.TryGetValue(info ,out ui))
		{
			_Hide(ui);
		}
	}

	private void _Hide(UIBase uibase)
	{
		uibase.Hide();
		//this._current_ui = this.ui_active.Pop();
	}

	private void _Show(UIBase uibase)
	{
		uibase.Show();
		this._current_ui = uibase;
		//this.ui_active.Push(uibase);
	}
}
