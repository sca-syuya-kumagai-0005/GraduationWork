using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class RandomMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static List<string> table{ get; private set; }//ランダム挙動のテーブル
    [SerializeField]List<string> tableReadOnly = new List<string>();
    enum Command
    {
        STRAIGHT=0,
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
}
