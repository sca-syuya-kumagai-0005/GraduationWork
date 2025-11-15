using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Reflection;

public class SortOrderInLayer : EditorWindow
{ 
      /// <summary>
      /// 対象コンポーネント情報をまとめる構造体
      /// </summary>
    private struct SortingOrderInformation
    {
        public GameObject gameObject;   // コンポーネントがついているGameObject
        public Component component;     // sortingOrderを持つコンポーネント
        public MemberInfo member;       // sortingOrderの番号
    }

    
    // 対象コンポーネントのリスト
    private List<SortingOrderInformation> componentList = new List<SortingOrderInformation>();

    // 並び替え可能なリスト なんかこれ使うとWindow内で[SerializeField]みたいに配列の順番を変更できる
    private ReorderableList reorderableList;

    private Vector2 scrollPos;//スクロールバーの位置

    /// <summary>
    /// メニューに登録してウィンドウを表示
    /// </summary>
    [MenuItem("Tools/OrderInLayer Editor")]
    public static void OpenWindow()
    {
        GetWindow<SortOrderInLayer>("Order in Layer Editor");
    }

    /// <summary>
    /// ウィンドウが有効化された時に呼ばれる。ReorderableListの初期化を行う。
    /// </summary>
    private void OnEnable()
    {
        // ReorderableListを初期化、要素はcomponentListを参照
        reorderableList = new ReorderableList(componentList, typeof(SortingOrderInformation), true, true, false, false);

        // リストのヘッダーの描画処理
        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Components with sortingOrder property");
        };

        // リスト内の各要素の描画処理
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (index < 0 || index >= componentList.Count) return;

            var compInfo = componentList[index];

            float labelWidth = 250f;
            float fieldWidth = rect.width - labelWidth;

            // GameObject名とコンポーネント名を左に表示
            string label = $"{compInfo.gameObject.name} ({compInfo.component.GetType().Name})";
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight), label);

            // sortingOrder(int)の値を右に表示・編集
            int currentValue = (int)GetMemberValue(compInfo.component, compInfo.member);
            int newValue = EditorGUI.IntField(new Rect(rect.x + labelWidth, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight), currentValue);
            if (newValue != currentValue)
            {
                SetMemberValue(compInfo.component, compInfo.member, newValue);
                EditorUtility.SetDirty(compInfo.component); // 変更をエディタに伝える
            }
        };
    }

    /// <summary>
    /// ウィンドウ内のGUIを描画。シーンから・アセットから・両方からの取得ボタンと並び替えリスト、
    /// そしてorderInLayer(=sortingOrder)をリスト順で一括設定するボタンを表示
    /// </summary>
    private void OnGUI()
    {

        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("シーンから取得"))
        {
            componentList.Clear();
            FindComponentsInScene();
            reorderableList.list = componentList;
        }

        if (GUILayout.Button("アセットから取得"))
        {
            componentList.Clear();
            FindComponentsInAssets();
            reorderableList.list = componentList;
        }

        if (GUILayout.Button("シーンとアセット両方から取得"))
        {
            componentList.Clear();
            FindComponentsInScene();
            FindComponentsInAssets();
            reorderableList.list = componentList;
        }

      
        if (GUILayout.Button("OrderinLayer(sortingOrder)順に並び替える"))
        {
            SortListByOrderInLayer();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("リスト順に OrderinLayer (sortingOrder) を一括設定"))
        {
            ApplySortingOrderByList();
        }



        //スクロールバー
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(0));
        if (reorderableList != null)
        {
            reorderableList.DoLayoutList();
        }
        EditorGUILayout.EndScrollView();
    }

    #region シーン内の対象コンポーネント取得

    /// <summary>
    /// 現在アクティブなシーンのルートオブジェクトから検索を開始
    /// 子も含めてsortingOrderを持つコンポーネントを取得
    /// </summary>
    private void FindComponentsInScene()
    {
        var scene = SceneManager.GetActiveScene();

        foreach (var rootGo in scene.GetRootGameObjects())
        {
            TraverseGameObjectHierarchy(rootGo);
        }

        Repaint();
    }

    /// <summary>
    /// GameObjectの階層を再帰的に探索し、対象コンポーネント(今回の場合はOrderinLayer(sortingOrder))があればリストに追加
    /// </summary>
    private void TraverseGameObjectHierarchy(GameObject go)
    {
        AddIfHasSortingOrder(go);

        foreach (Transform child in go.transform)
        {
            TraverseGameObjectHierarchy(child.gameObject);
        }
    }

    #endregion

    #region アセット内の対象コンポーネント取得

    /// <summary>
    /// Project内のGameObjectアセット（プレハブなど）をすべて検索し、
    /// sortingOrderを持つコンポーネントを抽出する
    /// </summary>
    private void FindComponentsInAssets()
    {
        // GameObject型のアセットのGUID一覧を取得
        string[] guids = AssetDatabase.FindAssets("t:GameObject");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null) continue;

            // アセットの中の全コンポーネントをチェックしてリストに追加
            TraverseComponentsInAsset(prefab);
        }

        Repaint();
    }

    /// <summary>
    /// AssetsからGameObjectを検索し、それについているコンポーネントからsortingOrderを持つものを探しリストに追加
    /// </summary>
    private void TraverseComponentsInAsset(GameObject prefab)
    {
        Component[] components = prefab.GetComponentsInChildren<Component>(true);
        foreach (var comp in components)
        {
            if (comp == null) continue;

            var compInfo = CreateSortingOrderComponent(comp);
            if (compInfo != null)
            {
                componentList.Add(compInfo.Value);
            }
        }
    }

    #endregion

    #region 対象コンポーネント判定と構造体作成

    /// <summary>
    /// コンポーネントにsortingOrderという名前のプロパティまたはフィールドがあるか調べる。
    /// もしあればSortingOrderComponent構造体を返す。なければnull。
    /// </summary>
    /// <param name="comp">調べるコンポーネント</param>
    /// <returns>該当する場合は構造体、なければnull</returns>
    private SortingOrderInformation? CreateSortingOrderComponent(Component comp)
    {
        var type = comp.GetType();

        // sortingOrderプロパティを探す（読み書き可能なpublicプロパティ）
        var prop = type.GetProperty("sortingOrder", BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.PropertyType == typeof(int) && prop.CanRead && prop.CanWrite)
        {
            return new SortingOrderInformation
            {
                gameObject = comp.gameObject,
                component = comp,
                member = prop
            };
        }

        // sortingOrderフィールドを探す
        var field = type.GetField("sortingOrder", BindingFlags.Public | BindingFlags.Instance);
        if (field != null && field.FieldType == typeof(int))
        {
            return new SortingOrderInformation
            {
                gameObject = comp.gameObject,
                component = comp,
                member = field
            };
        }

        // 見つからなければnullを返す
        return null;
    }

    /// <summary>
    /// GameObjectの全コンポーネントを調べて、sortingOrderを持つものをcomponentListに追加する
    /// </summary>
    /// <param name="go">調べるGameObject</param>
    private void AddIfHasSortingOrder(GameObject go)
    {
        Component[] components = go.GetComponents<Component>();

        foreach (var comp in components)
        {
            if (comp == null) continue;

            var compInfo = CreateSortingOrderComponent(comp);
            if (compInfo != null)
            {
                componentList.Add(compInfo.Value);
            }
        }
    }

    #endregion

    #region Reflectionを使ったプロパティ・フィールドの値操作ユーティリティ

    /// <summary>
    /// プロパティかフィールドかを判定して値を取得する
    /// </summary>
    private object GetMemberValue(Component comp, MemberInfo member)
    {
        if (member is PropertyInfo p) return p.GetValue(comp);
        if (member is FieldInfo f) return f.GetValue(comp);
        return null;
    }

    /// <summary>
    /// プロパティかフィールドかを判定して値を設定する
    /// </summary>
    private void SetMemberValue(Component comp, MemberInfo member, object value)
    {
        if (member is PropertyInfo p) p.SetValue(comp, value);
        else if (member is FieldInfo f) f.SetValue(comp, value);
    }

    #endregion

    #region リスト順にsortingOrderを一括設定

    /// <summary>
    /// リストの並び順に合わせてsortingOrderの値を連番で設定する。
    /// リストのインデックス0から順に0,1,2,...となる。
    /// </summary>
    private void ApplySortingOrderByList()
    {
        for (int i = 0; i < componentList.Count; i++)
        {
            var compInfo = componentList[i];
            SetMemberValue(compInfo.component, compInfo.member, i);
            EditorUtility.SetDirty(compInfo.component);
        }

        Debug.Log("OrderinLayer(sortingOrder)をリストの順に一括設定しました。");
    }
    #endregion
    
    #region OrderinLayer順に並び替える
    /// <summary>
    /// 
    /// </summary>
    private void SortListByOrderInLayer()
    {
        componentList.Sort((a, b) =>
        {
            int orderA = (int)GetMemberValue(a.component, a.member);
            int orderB = (int)GetMemberValue(b.component, b.member);
            return orderA.CompareTo(orderB);
        });
    }
    #endregion
};
