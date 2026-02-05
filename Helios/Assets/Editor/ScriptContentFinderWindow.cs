using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class ScriptContentFinderWindow : EditorWindow
{
    private string searchText = "";
    private Vector2 scroll;
    private Dictionary<MonoBehaviour, string> cache = new Dictionary<MonoBehaviour, string>();

    [MenuItem("Tools/Script Content Finder")]
    static void Open()
    {
        GetWindow<ScriptContentFinderWindow>("Script Content Finder");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Scriptì‡ÇÃï∂éöóÒåüçı", EditorStyles.boldLabel);

        searchText = EditorGUILayout.TextField("Search Text", searchText);

        if (GUILayout.Button("åüçı"))
        {
            cache.Clear();
            Repaint();
        }

        EditorGUILayout.Space();

        if (string.IsNullOrEmpty(searchText))
        {
            EditorGUILayout.HelpBox("åüçıÇ∑ÇÈï∂éöóÒÇì¸óÕÇµÇƒÇ≠ÇæÇ≥Ç¢", MessageType.Info);
            return;
        }

        var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.scene.isLoaded);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var go in gameObjects)
        {
            foreach (var mb in go.GetComponents<MonoBehaviour>())
            {
                if (mb == null) continue;

                string scriptText;

                if (!cache.TryGetValue(mb, out scriptText))
                {
                    var script = MonoScript.FromMonoBehaviour(mb);
                    if (script == null) continue;

                    var path = AssetDatabase.GetAssetPath(script);
                    if (string.IsNullOrEmpty(path) || !File.Exists(path)) continue;

                    scriptText = File.ReadAllText(path);
                    cache[mb] = scriptText;
                }

                if (scriptText.Contains(searchText))
                {
                    DrawResult(go, mb);
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawResult(GameObject go, MonoBehaviour mb)
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(go.name, GUILayout.Width(200)))
        {
            Selection.activeObject = go;
            EditorGUIUtility.PingObject(go);
        }

        EditorGUILayout.LabelField(mb.GetType().Name, GUILayout.Width(200));
        EditorGUILayout.LabelField(go.scene.name);

        EditorGUILayout.EndHorizontal();
    }
}
