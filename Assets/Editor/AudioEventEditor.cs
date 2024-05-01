using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioEvent))]
public class AudioEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Scriptableobject audio", EditorStyles.boldLabel);

        AudioEvent myScript = (AudioEvent)target;

        if (GUILayout.Button("Play"))
        {
            myScript.PlayFromEditor();
        }
        if (GUILayout.Button("Stop"))
        {
            myScript.StopFromEditor();
        }
        DrawDefaultInspector();
    }
}