using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CreateLevelWindow : EditorWindow
{
    private SerializedObject _serializedObject;
    private SerializedProperty _serializedProperty;
    private LevelAttributes[] _attributes;

    public LevelAttributes NewAttributes;
    private void OnGUI()
    {

        _serializedObject = new SerializedObject(NewAttributes);
        _serializedProperty = _serializedObject.GetIterator();
        _serializedProperty.NextVisible(true);
        DrawProperties(_serializedProperty);
        if (GUILayout.Button("save"))
        {
            _attributes = GetAllInstances<LevelAttributes>();
            if (NewAttributes.LevelName == null)
            {
                NewAttributes.LevelName = "Level" + (_attributes.Length + 1);
            }
            AssetDatabase.CreateAsset(NewAttributes, "Assets/Scripts/Level ScriptableObject/" + NewAttributes.LevelName + " Attributes" + ".asset"); // Àx¦s¸ô®|
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }

        Apply();
    }

    protected void DrawProperties(SerializedProperty p)
    {

        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);

        }
    }


    public static T[] GetAllInstances<T>() where T : LevelAttributes
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;

    }

    protected void Apply()
    {
        _serializedObject.ApplyModifiedProperties();
    }
}
