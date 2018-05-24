using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationLanguage))]
public class LocalizationEditor : Editor
{
	private SerializedObject serialized_object;
	private SerializedProperty localization_languages;
	private SerializedProperty language_type;
	private LocalizationLanguage text;
	//private int language_count;
	private string[] language_names = { "中文", "英文" };
	private void OnEnable()
	{
		text = (LocalizationLanguage)target;
		serialized_object = new SerializedObject(target);
		language_type = serialized_object.FindProperty("language_type");
		localization_languages = serialized_object.FindProperty("localization_languages");
		if (localization_languages != null)
		{
			//language_count = localization_languages.arraySize;
			if (localization_languages.isArray && localization_languages.arraySize == 0)
			{
				localization_languages.arraySize = 2;
			}
		}
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.BeginVertical();
		//language_count = EditorGUILayout.IntField("语言种类", language_count);
		if (localization_languages.isArray)
		{
			for (int i = 0; i < localization_languages.arraySize; i++)
			{
				EditorGUILayout.BeginHorizontal();
				if (i == 0)
				{
					EditorGUILayout.LabelField("中文", GUILayout.Width(40));
				}
				else
				{
					EditorGUILayout.LabelField("英文", GUILayout.Width(40));
				}
				SerializedProperty text = localization_languages.GetArrayElementAtIndex(i);
				text.stringValue = EditorGUILayout.TextArea(text.stringValue, GUILayout.ExpandHeight(true));
				//EditorGUILayout.PropertyField(localization_languages.GetArrayElementAtIndex(i),new GUIContent("中卫"), true, GUILayout.ExpandHeight(true));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("");
			}
			//EditorGUILayout.PropertyField(localization_languages.GetArrayElementAtIndex(0), true);
		}
		////当Inspector 面板发生变化时保存数据
		//if (GUI.changed)
		//{
		//	EditorUtility.SetDirty(target);
		//}
		serialized_object.ApplyModifiedProperties();
		if (GUI.changed)
		{
			text.SetText();
		}
		EditorGUILayout.EndVertical();
	}

	[MenuItem("GameObject/UI/多语言组件", false, -1)]
	public static void CreateText()
	{
		Transform select = Selection.activeTransform;
		GameObject text = new GameObject("Text");
		localizationText localization_text = text.AddComponent<localizationText>();
		text.transform.SetParent(select);
		text.transform.localPosition = Vector2.zero;
		var rect = text.GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(160, 30);
		localization_text.text = "输入内容";
		//MonoBehaviour.Instantiate();
	}
}
