using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SortOrderInLayer : EditorWindow
{
    private static List<SpriteRenderer> sortObjects=new List<SpriteRenderer>();
    private static string myString = "Hellow";
    private Canvas can=new Canvas();
    private static UnityEngine.Object[] withOrderInLayer = new UnityEngine.Object[2] { new Canvas(),new SpriteRenderer()};
    [MenuItem("Window/My Window")]
    static void Window()
    {
        SortOrderInLayer window = (SortOrderInLayer)EditorWindow.GetWindow(typeof(SortOrderInLayer));
        window.Show();
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [InitializeOnLoadMethod]
    private static void GetObjects()
    {
        for(int i = 0;i<withOrderInLayer.Length;i++)
        {
            SpriteRenderer[] s = FindObjectsOfType<SpriteRenderer>();
        }

    }
    private void OnGUI()
    {
         GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        
        myString = EditorGUILayout.TextField("Text Field", sortObjects[0].name);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
