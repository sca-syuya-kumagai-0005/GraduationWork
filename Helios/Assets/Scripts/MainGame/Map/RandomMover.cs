using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteAlways]
public class RandomMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static List<string> table{ get; private set; }//ランダム挙動のテーブル
    [SerializeField]List<string> tableReadOnly = new List<string>();//テーブル確認用の変数
    enum Command
    {
        STRAIGHT,
        RIGHT,
        LEFT,
        BACK,
        SIZE,
    }
    const int TABLE_SIZE = 1000;

    private void Start()
    {
        table = new List<string>();
        for(int i = 0;i<TABLE_SIZE;i++)
        {
            int com = Random.Range(0, (int)Command.SIZE);
            Command a = (Command)com;
            table.Add(a.ToString());
            
    }
        tableReadOnly = table;
        Debug.Log(tableReadOnly.Count);
    }

    public IEnumerator RandomMove(GameObject moveObj,Vector3 endPosition,float time)
    {
        Vector3 dir = ((endPosition) - moveObj.transform.position).normalized;
        Vector3 lastDirction = dir;
        while (lastDirction == dir)
        {
            lastDirction = dir;
            Vector3 vec = lastDirction * Time.deltaTime;
            if (dir.x == 1)
            {
                moveObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            if (dir.x == -1)
            {
                moveObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            if (dir.y == 1)
            {
                moveObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
            if (dir.y == -1)
            {
                moveObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            
            moveObj.transform.position += vec / time;
            dir = ((endPosition) - moveObj.transform.position).normalized;
            yield return null;
        }
        moveObj.transform.position = endPosition;
    }

}
