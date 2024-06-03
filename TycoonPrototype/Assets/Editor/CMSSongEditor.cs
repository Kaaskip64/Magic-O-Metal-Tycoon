using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine.TestTools;

public class CMSSongEditor : EditorWindow
{
    private const string _helpText = "Cannot find 'Band listings' anywhere in the scene, call support!";
    private static Rect _helpRect = new Rect(0f, 0f, 400f, 100f);

    private static Vector2 _windowMinSize = Vector2.one * 500f;

    private bool _isActive;

    SerializedObject _objectSerialized = null;

    ReorderableList _listReordarable = null;

    private BandListings _bandListings;
    
    private Vector2 _scrollPos;
    
    [MenuItem("Window/UI Toolkit/CMSSongEditor")]
    public static void ShowExample()
    {
        CMSSongEditor wnd = GetWindow<CMSSongEditor>(Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll"));
        wnd.titleContent = new GUIContent("CMSSongEditor");
    }

    private void OnEnable()
    {
        _bandListings = FindObjectOfType<BandListings>(true);

        if (_bandListings)
        {
            _objectSerialized = new SerializedObject(_bandListings);

            SerializedProperty bandData = _objectSerialized.FindProperty("BandData");
            
            _listReordarable = new ReorderableList(_objectSerialized, bandData, true,
                true, true, true);

            _listReordarable.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "BandListingData");
            _listReordarable.drawElementCallback = (Rect rect, int index, bool _isActive, bool isFocused) =>
            {
                rect.y += 2f;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUIContent objectLabel = new GUIContent($"BandListingData {index}");

                EditorGUI.PropertyField(rect, _listReordarable.serializedProperty.GetArrayElementAtIndex(index), objectLabel);
            };
        }
    }
    

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        if (_objectSerialized == null)
        {
            EditorGUI.HelpBox(_helpRect, _helpText, MessageType.Warning);
        }
        else if (_objectSerialized != null)
        {
            _objectSerialized.Update();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,GUILayout.Width(position.width), GUILayout.Height(position.height - 20));
            GUILayout.Label ("", GUILayout.Width ( position.width - 40 ), GUILayout.Height ( _listReordarable.GetHeight() ) );
            //Debug.Log(_scrollPos);
            _listReordarable.DoList(new Rect(0, 0, position.width - 20, _listReordarable.GetHeight()));
            
            EditorGUILayout.EndScrollView();

            _objectSerialized.ApplyModifiedProperties();
        }

        GUILayout.Space(30f);
        GUILayout.Space(10f);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30f);
        EditorGUILayout.EndHorizontal();
    }

    public static void Horizontal(Action action, GUIStyle style)
    {
        GUILayout.BeginHorizontal(style);
        try
        {
            action();
        }
        finally
        {
            GUILayout.EndHorizontal();
        }
    }

    public void CreateGUI()
    {
       
    }
}