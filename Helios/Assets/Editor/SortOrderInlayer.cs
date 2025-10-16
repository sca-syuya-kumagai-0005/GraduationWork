using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Reflection;

public class SortOrderInLayer : EditorWindow
{ 
      /// <summary>
      /// �ΏۃR���|�[�l���g�����܂Ƃ߂�\����
      /// </summary>
    private struct SortingOrderInformation
    {
        public GameObject gameObject;   // �R���|�[�l���g�����Ă���GameObject
        public Component component;     // sortingOrder�����R���|�[�l���g
        public MemberInfo member;       // sortingOrder�̔ԍ�
    }

    
    // �ΏۃR���|�[�l���g�̃��X�g
    private List<SortingOrderInformation> componentList = new List<SortingOrderInformation>();
    private List<SortingOrderInformation> tmpComponentList = new List<SortingOrderInformation>();

    // ���ёւ��\�ȃ��X�g �Ȃ񂩂���g����Window����[SerializeField]�݂����ɔz��̏��Ԃ�ύX�ł���
    private ReorderableList reorderableList;

    private Vector2 scrollPos;//�X�N���[���o�[�̈ʒu

    /// <summary>
    /// ���j���[�ɓo�^���ăE�B���h�E��\��
    /// </summary>
    [MenuItem("Tools/OrderInLayer Editor")]
    public static void OpenWindow()
    {
        GetWindow<SortOrderInLayer>("Order in Layer Editor");
    }

    /// <summary>
    /// �E�B���h�E���L�������ꂽ���ɌĂ΂��BReorderableList�̏��������s���B
    /// </summary>
    private void OnEnable()
    {
        // ReorderableList���������A�v�f��componentList���Q��
        reorderableList = new ReorderableList(componentList, typeof(SortingOrderInformation), true, true, false, false);

        // ���X�g�̃w�b�_�[�̕`�揈��
        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Components with sortingOrder property");
        };

        // ���X�g���̊e�v�f�̕`�揈��
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (index < 0 || index >= componentList.Count) return;

            var compInfo = componentList[index];

            float labelWidth = 250f;
            float fieldWidth = rect.width - labelWidth;

            // GameObject���ƃR���|�[�l���g�������ɕ\��
            string label = $"{compInfo.gameObject.name} ({compInfo.component.GetType().Name})";
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight), label);

            // sortingOrder(int)�̒l���E�ɕ\���E�ҏW
            int currentValue = (int)GetMemberValue(compInfo.component, compInfo.member);
            int newValue = EditorGUI.IntField(new Rect(rect.x + labelWidth, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight), currentValue);
            if (newValue != currentValue)
            {
                SetMemberValue(compInfo.component, compInfo.member, newValue);
                EditorUtility.SetDirty(compInfo.component); // �ύX���G�f�B�^�ɓ`����
            }
        };
    }

    /// <summary>
    /// �E�B���h�E����GUI��`��B�V�[������E�A�Z�b�g����E��������̎擾�{�^���ƕ��ёւ����X�g�A
    /// ������orderInLayer(=sortingOrder)�����X�g���ňꊇ�ݒ肷��{�^����\��
    /// </summary>
    private void OnGUI()
    {

        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("�V�[������擾"))
        {
            componentList.Clear();
            FindComponentsInScene();
            reorderableList.list = componentList;
        }

        if (GUILayout.Button("�A�Z�b�g����擾"))
        {
            componentList.Clear();
            FindComponentsInAssets();
            reorderableList.list = componentList;
        }

        if (GUILayout.Button("�V�[���ƃA�Z�b�g��������擾"))
        {
            componentList.Clear();
            FindComponentsInScene();
            FindComponentsInAssets();
            reorderableList.list = componentList;
        }

      
        if (GUILayout.Button("OrderinLayer(sortingOrder)���ɕ��ёւ���"))
        {
            SortListByOrderInLayer();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("���X�g���� OrderinLayer (sortingOrder) ���ꊇ�ݒ�"))
        {
            ApplySortingOrderByList();
        }



        //�X�N���[���o�[
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(0));
        if (reorderableList != null)
        {
            reorderableList.DoLayoutList();
        }
        EditorGUILayout.EndScrollView();
    }

    #region �V�[�����̑ΏۃR���|�[�l���g�擾

    /// <summary>
    /// ���݃A�N�e�B�u�ȃV�[���̃��[�g�I�u�W�F�N�g���猟�����J�n
    /// �q���܂߂�sortingOrder�����R���|�[�l���g���擾
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
    /// GameObject�̊K�w���ċA�I�ɒT�����A�ΏۃR���|�[�l���g(����̏ꍇ��OrderinLayer(sortingOrder))������΃��X�g�ɒǉ�
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

    #region �A�Z�b�g���̑ΏۃR���|�[�l���g�擾

    /// <summary>
    /// Project����GameObject�A�Z�b�g�i�v���n�u�Ȃǁj�����ׂČ������A
    /// sortingOrder�����R���|�[�l���g�𒊏o����
    /// </summary>
    private void FindComponentsInAssets()
    {
        // GameObject�^�̃A�Z�b�g��GUID�ꗗ���擾
        string[] guids = AssetDatabase.FindAssets("t:GameObject");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null) continue;

            // �A�Z�b�g�̒��̑S�R���|�[�l���g���`�F�b�N���ă��X�g�ɒǉ�
            TraverseComponentsInAsset(prefab);
        }

        Repaint();
    }

    /// <summary>
    /// Assets����GameObject���������A����ɂ��Ă���R���|�[�l���g����sortingOrder�������̂�T�����X�g�ɒǉ�
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

    #region �ΏۃR���|�[�l���g����ƍ\���̍쐬

    /// <summary>
    /// �R���|�[�l���g��sortingOrder�Ƃ������O�̃v���p�e�B�܂��̓t�B�[���h�����邩���ׂ�B
    /// ���������SortingOrderComponent�\���̂�Ԃ��B�Ȃ����null�B
    /// </summary>
    /// <param name="comp">���ׂ�R���|�[�l���g</param>
    /// <returns>�Y������ꍇ�͍\���́A�Ȃ����null</returns>
    private SortingOrderInformation? CreateSortingOrderComponent(Component comp)
    {
        var type = comp.GetType();

        // sortingOrder�v���p�e�B��T���i�ǂݏ����\��public�v���p�e�B�j
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

        // sortingOrder�t�B�[���h��T��
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

        // ������Ȃ����null��Ԃ�
        return null;
    }

    /// <summary>
    /// GameObject�̑S�R���|�[�l���g�𒲂ׂāAsortingOrder�������̂�componentList�ɒǉ�����
    /// </summary>
    /// <param name="go">���ׂ�GameObject</param>
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

    #region Reflection���g�����v���p�e�B�E�t�B�[���h�̒l���샆�[�e�B���e�B

    /// <summary>
    /// �v���p�e�B���t�B�[���h���𔻒肵�Ēl���擾����
    /// </summary>
    private object GetMemberValue(Component comp, MemberInfo member)
    {
        if (member is PropertyInfo p) return p.GetValue(comp);
        if (member is FieldInfo f) return f.GetValue(comp);
        return null;
    }

    /// <summary>
    /// �v���p�e�B���t�B�[���h���𔻒肵�Ēl��ݒ肷��
    /// </summary>
    private void SetMemberValue(Component comp, MemberInfo member, object value)
    {
        if (member is PropertyInfo p) p.SetValue(comp, value);
        else if (member is FieldInfo f) f.SetValue(comp, value);
    }

    #endregion

    #region ���X�g����sortingOrder���ꊇ�ݒ�

    /// <summary>
    /// ���X�g�̕��я��ɍ��킹��sortingOrder�̒l��A�ԂŐݒ肷��B
    /// ���X�g�̃C���f�b�N�X0���珇��0,1,2,...�ƂȂ�B
    /// </summary>
    private void ApplySortingOrderByList()
    {
        for (int i = 0; i < componentList.Count; i++)
        {
            var compInfo = componentList[i];
            SetMemberValue(compInfo.component, compInfo.member, i);
            EditorUtility.SetDirty(compInfo.component);
        }

        Debug.Log("OrderinLayer(sortingOrder)�����X�g�̏��Ɉꊇ�ݒ肵�܂����B");
    }
    #endregion
    
    #region OrderinLayer���ɕ��ёւ���
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
