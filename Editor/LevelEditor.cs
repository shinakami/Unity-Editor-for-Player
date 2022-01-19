using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelEditor : EditorWindow
{
    private SerializedObject _serializedObject;
    private SerializedProperty _serializedProperty;
    private LevelAttributes[] _attributes;
    private string _selectedPropertyPach;
    private string _selectedProperty;

    [MenuItem("Window/Level Editor")]


    static void Init()
    {
        LevelEditor LevelEditor = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        LevelEditor.Show();
    }

    void OnGUI()
    {
        _attributes = GetAllInstances<LevelAttributes>();
        _serializedObject = new SerializedObject(_attributes[0]);
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSliderBar(_attributes);

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (_selectedProperty != null)
        {
            for (int i = 0; i < _attributes.Length; i++)
            {
                if (_attributes[i].LevelName == _selectedProperty)
                {
                    _serializedObject = new SerializedObject(_attributes[i]);
                    _serializedProperty = _serializedObject.GetIterator();
                    _serializedProperty.NextVisible(true);
                    DrawProperties(_serializedProperty);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("select an item from the list");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
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


    protected void DrawProperties(SerializedProperty p)
    {

        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);

        }
    }

    protected void DrawSliderBar(LevelAttributes[] prop)
    {
        foreach (LevelAttributes p in prop)
        {
            if (GUILayout.Button(p.LevelName))
            {
                _selectedPropertyPach = p.LevelName;
            }
        }

        if (!string.IsNullOrEmpty(_selectedPropertyPach))
        {
            _selectedProperty = _selectedPropertyPach;
        }

        if (GUILayout.Button("new levelAttributes"))
        {
            LevelAttributes newAttributes = ScriptableObject.CreateInstance<LevelAttributes>();
            CreateLevelWindow newLevelWindow = GetWindow<CreateLevelWindow>("New LevelAttributes");
            newLevelWindow.NewAttributes = newAttributes;
        }
    }



    protected void Apply()
    {
        _serializedObject.ApplyModifiedProperties();
    }
}
