using System;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private enum DataAddress
    {
        DAY=0,
        STAYSINNERS,
        MAX
    }
    private string[] data = new string[(int)DataAddress.MAX];
    private int days = 1;
    public int Days {  get { return days; } set { days = value; } }
    private const int maxSinners = 31;
    private bool[] stayedSinners = new bool[maxSinners];
    public bool[] StayedSinner { get { return stayedSinners; }set { stayedSinners = value; } }
    public void Save()
    {
        string path = @"\Assets/SaveData.txt";
        if (File.Exists(Application.dataPath+ path))
        {
            using (File.Create(path)) { }
        }
        StreamWriter streamWriter = new StreamWriter("./Assets/SaveData.txt", false);
        data[(int)DataAddress.DAY] = days.ToString();
        for (int i = 0; i < stayedSinners.Length; i++) 
            data[(int)DataAddress.STAYSINNERS] += Convert.ToInt32(stayedSinners[i]);
        string str = string.Join("\n", data);
        streamWriter.WriteLine(str);
        streamWriter.Flush();
        streamWriter.Close();
    }
    public void Load()
    {
        StreamReader streamReader = new StreamReader("./Assets/SaveData.txt");
        int lineCount = 0;
        const int DataEnd = -1;
        while (streamReader.Peek() != DataEnd) data[lineCount] = streamReader.ReadLine();
        streamReader.Close();
        days = int.Parse(data[(int)DataAddress.DAY]);
        for (int i = 0; i < data[(int)DataAddress.STAYSINNERS].Length; i++)
            stayedSinners[i] = Convert.ToBoolean(data[i]);
    }
}
