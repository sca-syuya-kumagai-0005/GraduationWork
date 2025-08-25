using UnityEngine;
using System.Collections.Generic;
public class MapCreator : CsvReader
{
    [SerializeField] TextAsset mapData;
    [SerializeField] GameObject[] mapObjects;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapCreate(List<string[]> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                mapObjects[(int.Parse(data[i][j])];
            }
        }

    }
}