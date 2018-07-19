using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UILevelTableViewCell : TableViewCell
{
	protected UILevelItem m_UILevelItem = null;

	public UILevelItem _UILevelItem
	{
		get
		{
			return m_UILevelItem;
		}
		set
		{
			m_UILevelItem = value;
			node = m_UILevelItem.gameObject;
		}
	}
}

public class UILevelGridView : TableViewDataSource, TableCellDelegate
{
	public UIGridView m_gridView = null;

	private List<LevelData> datas;

	private LevelData m_SelectedHero;

	public int numberOfCellsInTableView(UIGridView table)
	{
		if (datas != null)
		{
			return datas.Count;
		}
		return 0;
	}

	public TableViewCell tableCellAtIndex(UIGridView table, TableViewCell cell, int idx)
	{
		UILevelTableViewCell itemCell = cell as UILevelTableViewCell;
		UILevelItem m_UILevelItem = null;
		if (itemCell == null)
		{
			itemCell = new UILevelTableViewCell();
			m_UILevelItem = UILevelItem.Create();
			itemCell._UILevelItem = m_UILevelItem;
			FlushItem(m_UILevelItem, idx);
			EventTriggerClick.Get(m_UILevelItem.gameObject).onClick = (o) =>
			{
				SelectItem(table, m_UILevelItem, itemCell.Idx);
			};
		}
		else
		{
			m_UILevelItem = itemCell._UILevelItem;
			FlushItem(m_UILevelItem, idx);
		}
		return itemCell;
	}
	static public UILevelGridView Create(GameObject objRoot)
	{
		UILevelGridView _view = new UILevelGridView();
		_view.Init(objRoot);
		return _view;
	}

	public void Init(GameObject objRoot)
	{
		m_gridView = objRoot.gameObject.GetComponent<UIGridView>();
		if (m_gridView == null)
		{
			m_gridView = objRoot.gameObject.AddComponent<UIGridView>();
		}
		m_gridView.Init();
		m_gridView.DataSource = this;
		m_gridView.ViewCellDelegate = this;
	}
	public void reloadData(List<LevelData> data)
	{
		datas = data;
		m_gridView.reloadData();
	}

	public void FlushItem(UILevelItem uiItem, int index)
	{
		if (datas != null)
		{
			uiItem.Init(datas[index]);

		}
	}

	public void SelectItem(UIGridView table, UILevelItem selectItem, int Idx)
	{
		if (datas != null)
		{
			m_SelectedHero = selectItem.level_data;
			if (m_SelectedHero != null && GameData.GetPassedLevel(m_SelectedHero.Level_Difficulty) >= m_SelectedHero.CurrentLevel)
			{
				GameControl.Instance.PlayGame(GameType.Custom, m_SelectedHero.Level_Difficulty, m_SelectedHero.CurrentLevel);
				SDK.Instance.StartLevel(string.Format("进入关卡{0}难度{1}类型{2}", m_SelectedHero.CurrentLevel, m_SelectedHero.Level_Difficulty, GameType.Custom));
			}
		}
	}

	public void setContentOffsetToTop()
	{
		m_gridView.setContentOffsetToTop();
	}

	public TableViewCell cellAtIndex(int index)
	{
		return m_gridView.cellAtIndex(index);
	}

	public void tableCellWillRecycle(UIGridView table, TableViewCell cell)
	{
		//if (m_UIHero != null)
		//{
		//	UILevelTableViewCell itemCell = cell as UILevelTableViewCell;
		//	m_UIHero.tableCellWillRecycle(table, itemCell._UILevelItem);
		//}
	}
}
