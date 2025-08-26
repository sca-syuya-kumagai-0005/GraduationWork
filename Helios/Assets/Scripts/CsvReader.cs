using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class CsvReader : MonoBehaviour
{
    protected List<string[]> Read(TextAsset csvData) //引数に入力したCSVファイルをリストに変換する関数
    {
        List<string[]> datas = new List<string[]>();
        StringReader reader = new StringReader(csvData.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            datas.Add(line.Split(','));
        }
        return datas;
    }
}
