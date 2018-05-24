using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditPanel))]
public class CustomInspectorEditor : Editor {

	EditPanel editor_panel;

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
		if (GUILayout.Button("添加一个图片"  ,GUILayout.Height(30)))
		{
			editor_panel.AddOneTypeImage();
		}
		EditorGUILayout.Space();
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
}
