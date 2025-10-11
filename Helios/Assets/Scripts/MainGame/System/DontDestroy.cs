using UnityEngine;

public class DontDestroy:MonoBehaviour 
{
    [SerializeField]
    private GameObject[] singletonObjects;
    private void Awake()
    {
        for (int i = 0; i < singletonObjects.Length; i++)
        {
            if (GameObject.Find(singletonObjects[i].name) == null)
            {
                GameObject go = Instantiate(singletonObjects[i]);
                go.name= singletonObjects[i].name;
                DontDestroyOnLoad(go);
            }
        }
    }
}
