using UnityEngine;
using UnityEditor;
using System.Collections;

public class GN_Editor {

	static string version = "1.0";
	static Texture2D Logo;

	public static void GetLogo () {
		Logo = Resources.Load ("gamerNetwork", typeof(Texture2D)) as Texture2D;
	}

	public static void DefineGUIStyle (string module) { 

		GUIStyle myStyle = GUI.skin.GetStyle ("box");
		myStyle.padding = new RectOffset (2, 2, 1, 1);

		var guiTitleStyle = new GUIStyle (GUI.skin.label);
		guiTitleStyle.normal.textColor = Color.white;
		guiTitleStyle.fontSize = 14;
		guiTitleStyle.alignment = TextAnchor.MiddleCenter;

		var guiMessageStyle = new GUIStyle (GUI.skin.label);
		guiMessageStyle.wordWrap = true;
		guiMessageStyle.fontSize = 11;
		guiMessageStyle.normal.textColor = Color.white;
		guiMessageStyle.alignment = TextAnchor.MiddleCenter;

		var guiMessageStyle1 = new GUIStyle (GUI.skin.label);
		guiMessageStyle1.wordWrap = true;
		guiMessageStyle1.fontSize = 9;
		guiMessageStyle1.normal.textColor = Color.grey;
		guiMessageStyle1.alignment = TextAnchor.MiddleCenter;

		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical ("box");
		GUILayout.Box (Logo, guiTitleStyle);
		EditorGUILayout.LabelField ("Gamer Network Framework v" + version, guiMessageStyle);
		EditorGUILayout.LabelField ("Component : " + module, guiMessageStyle1);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
	}
}
