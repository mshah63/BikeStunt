using UnityEditor;

[CustomEditor(typeof(GN_SplashScreen))]
public class GN_SplashScreenEditor : Editor {

	string module = "Splash Screen";

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
