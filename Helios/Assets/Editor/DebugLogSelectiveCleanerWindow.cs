using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DebugLogSelectiveCleanerWindow : EditorWindow
{
    private Vector2 scroll;

    // Script path -> info
    private Dictionary<string, ScriptInfo> scripts = new Dictionary<string, ScriptInfo>();

    private static readonly Regex debugLogRegex =
        new Regex(
            @"^\s*Debug\.Log(Error|Warning)?\s*\(.*?\)\s*;\s*$",
            RegexOptions.Multiline);

    private class ScriptInfo
    {
        public string path;
        public bool selected;
    }

    [MenuItem("Tools/Debug Log Selective Cleaner")]
    static void Open()
    {
        GetWindow<DebugLogSelectiveCleanerWindow>("Debug Log Cleaner");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Debug.Log を含む Script 一覧", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "チェックした Script のみ Debug.Log / Warning / Error を削除します。\n" +
            "Undoは効きません。実行前に必ずコミットしてください。",
            MessageType.Warning);

        EditorGUILayout.Space();

        if (GUILayout.Button("Search"))
        {
            Search();
        }

        EditorGUILayout.Space();

        DrawList();

        EditorGUILayout.Space();

        GUI.enabled = scripts.Values.Any(s => s.selected);
        if (GUILayout.Button("Remove Selected Debug.Log"))
        {
            if (EditorUtility.DisplayDialog(
                "確認",
                "選択した Script から Debug.Log を削除します。\n本当に実行しますか？",
                "実行",
                "キャンセル"))
            {
                RemoveSelected();
            }
        }
        GUI.enabled = true;
    }

    void Search()
    {
        scripts.Clear();

        var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.scene.isLoaded);

        foreach (var go in gameObjects)
        {
            foreach (var mb in go.GetComponents<MonoBehaviour>())
            {
                if (mb == null) continue;

                var monoScript = MonoScript.FromMonoBehaviour(mb);
                if (monoScript == null) continue;

                var path = AssetDatabase.GetAssetPath(monoScript);
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) continue;

                if (scripts.ContainsKey(path)) continue;

                var text = File.ReadAllText(path);
                if (debugLogRegex.IsMatch(text))
                {
                    scripts.Add(path, new ScriptInfo
                    {
                        path = path,
                        selected = false
                    });
                }
            }
        }
    }

    void DrawList()
    {
        if (scripts.Count == 0)
        {
            EditorGUILayout.HelpBox("Debug.Log を含む Script はありません", MessageType.Info);
            return;
        }

        EditorGUILayout.LabelField($"ヒット数: {scripts.Count}");

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var info in scripts.Values)
        {
            EditorGUILayout.BeginHorizontal();

            info.selected = EditorGUILayout.Toggle(info.selected, GUILayout.Width(20));

            EditorGUILayout.LabelField(Path.GetFileName(info.path), GUILayout.Width(250));

            if (GUILayout.Button("Open", GUILayout.Width(60)))
            {
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(info.path);
                AssetDatabase.OpenAsset(script);
            }

            EditorGUILayout.LabelField(info.path);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    void RemoveSelected()
    {
        var targets = scripts.Values.Where(s => s.selected).ToList();

        foreach (var info in targets)
        {
            var text = File.ReadAllText(info.path);
            var newText = debugLogRegex.Replace(text, "");
            File.WriteAllText(info.path, newText);
        }

        AssetDatabase.Refresh();

        Debug.Log($"Debug.Log 削除完了 : {targets.Count} scripts");

        scripts.Clear();
    }
}
