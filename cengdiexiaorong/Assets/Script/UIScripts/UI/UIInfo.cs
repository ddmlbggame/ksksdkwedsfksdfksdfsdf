using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UIInfo  {

	public  UIType Type;

	public UIHierarchyType UI_Hierarchy_Type;

	public string Path;

	public UIInfo(UIType type , UIHierarchyType ui_hierarchy_type ,string path)
	{
		this.Type = type;
		this.UI_Hierarchy_Type = ui_hierarchy_type;
		this.Path = path;
	}
}

public enum UIType
{
	Main,
	Login,
	Buy,
	Pause,
	Finish,
	Level,
	Result,
}

public enum UIHierarchyType
{
	Normal,
	Dialog,
	Top,
}