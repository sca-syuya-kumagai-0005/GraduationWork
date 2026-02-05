using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class SearchComponent: EditorWindow
{
    private string componentName = "";
    private Vector2 scroll;

    [MenuItem("Tools/Component Finder")]
    static void Open()
    {
        GetWindow<SearchComponent>("Component Finder");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("コンポーネント名で検索", EditorStyles.boldLabel);

        componentName = EditorGUILayout.TextField("Component Name", componentName);

        if (GUILayout.Button("検索"))
        {
            // ボタン押下時にRepaintするだけ（検索はOnGUI内で動的）
            Repaint();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("検索結果", EditorStyles.boldLabel);

        if (string.IsNullOrEmpty(componentName))
        {
            EditorGUILayout.HelpBox("コンポーネント名を入力してください", MessageType.Info);
            return;
        }

        // 全GameObject取得（非アクティブ含む）
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.scene.isLoaded);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var go in allObjects)
        {
            var components = go.GetComponents<Component>();

            foreach (var comp in components)
            {
                if (comp == null) continue;

                if (comp.GetType().Name.Equals(componentName, StringComparison.Ordinal))
                {
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(go.name, GUILayout.ExpandWidth(true)))
                    {
                        Selection.activeObject = go;
                        EditorGUIUtility.PingObject(go);
                    }

                    EditorGUILayout.LabelField(go.scene.name, GUILayout.Width(150));

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
