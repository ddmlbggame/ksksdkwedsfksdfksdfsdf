using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditPanel))]
public class CustomInspectorEditor : Editor {

	EditPanel editor_panel;

	public int selGridInt =-1;
	Vector2 scrollTarget;
	private void OnEnable()
	{
		editor_panel = (EditPanel)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("初始化" ,GUILayout.Height(30)))
		{
			editor_panel.OnInit();
		}
		EditorGUILayout.Space();
		if (GUILayout.Button("暂存当前数据", GUILayout.Height(30)))
		{
			editor_panel.OnSaveData();
		}
		EditorGUILayout.Space();
		if (GUILayout.Button("保存到文件", GUILayout.Height(30)))
		{
			editor_panel.OnSave();
		}

		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		string[] names = Enum.GetNames(typeof(ImageType));
		scrollTarget = GUILayout.BeginScrollView(scrollTarget);
		selGridInt = GUILayout.SelectionGrid(selGridInt, editor_panel.texture, 3);
		//selGridInt = GUILayout.SelectionGrid(selGridInt, names, 2);
		GUILayout.EndScrollView();

		//for (int i = 0; i < names.Length; i++)
		//{
		//	EditorGUILayout.BeginHorizontal();
		//	EditorGUILayout.EndHorizontal();
		//	if (GUILayout.Button(names[i], GUILayout.Height(30), GUILayout.Width(150)))
		//	{
		//		ClickAddImage((ImageType)i);
		//	}

		//}
		if (GUILayout.Button("添加图片", GUILayout.Height(30)))
		{
			Debug.Log(selGridInt);
			editor_panel.AddOneTypeImage((ImageType)selGridInt);
		}
		EditorGUILayout.Space();
		if (editor_panel.images != null)
		{
			foreach (var item in editor_panel.images)
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("选中该图片", GUILayout.Height(25), GUILayout.Width(100)))
				{
					Selection.activeGameObject = item.Value.gameObject;
				}
				if (GUILayout.Button("删除", GUILayout.Height(25), GUILayout.Width(50)))
				{
					editor_panel.RemoveSelectedImage(item.Value);
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
			}
		}	

		EditorGUILayout.EndVertical();
	}

	private  void ClickAddImage(ImageType iamge_type)
	{
		editor_panel.AddOneTypeImage(iamge_type);
	}
}
