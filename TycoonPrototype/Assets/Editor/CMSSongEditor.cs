using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.TestTools;


public class CMSSongEditor : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    private List<BandListingData> Songs;
    
    [MenuItem("Window/UI Toolkit/CMSSongEditor")]
    public static void ShowExample()
    {
        CMSSongEditor wnd = GetWindow<CMSSongEditor>();
        wnd.titleContent = new GUIContent("CMSSongEditor");
    }
    
    
    void OnGUI()
    {
        GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField ("Text Field", myString);
        
        groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle ("Toggle", myBool);
        myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup ();
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        Button button = new Button();
        button.name = "CreateSong";
        button.text = "Create Song";
        root.Add(button);
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);
    }
}