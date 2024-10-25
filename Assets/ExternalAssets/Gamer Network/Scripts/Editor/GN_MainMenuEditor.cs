using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GN_MainMenu))]
public class GN_MainMenuEditor : Editor {

    string module = "Main Menu";

    void Awake() {
	    GN_Editor.GetLogo();
    }

    public override void OnInspectorGUI() {

	    GN_Editor.DefineGUIStyle(module);

        EditorGUILayout.BeginVertical("box");
        DrawDefaultInspector();
        EditorGUILayout.EndHorizontal();
    }
}

public class ResetSaveData{
	[MenuItem("Window/GN - Gamer Network Framework/Reset Save Data %#r")]
	private static void ResetSave (){				
		Reset ();
	}

	[MenuItem("Window/GN - Gamer Network Framework/Open Save File %#o")]
	private static void OpenSave (){
		Application.OpenURL (Application.persistentDataPath);
	}

	public static void Reset(){
		GN_SaveLoad.DeleteProgress();
		EditorUtility.DisplayDialog("GN  - Gamer Network Framework",
			"Save data reset successfull !", 
			"Ok");
	}
}
