using System;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private enum DataAddress
    {
        DAY=0,
        HOUSEDSINNER,
        MEMORY,
        MAX
    }
    private string[] data = new string[(int)DataAddress.MAX];
    private int days =1;
    const int adjustment = 48;
    public int Days {  get { return days; } set { days = value; } }
    private const int maxSinners = 31;
    private bool[] housedSinners = new bool[maxSinners];
    public bool[] HousedSinner { get { return housedSinners; }set { housedSinners = value; } }

    private bool[] sinnerMemory = new bool[maxSinners];
    public bool[] Memory { get { return sinnerMemory; } set { sinnerMemory = value; } }
    private void Start()
    {
        Load();
    }
    private static string SavePath => System.IO.Path.Combine(Application.persistentDataPath, "SaveData.txt");
    private void Update()
    {
        //if(Input.GetMouseButtonDown(2))
        //Save();
    }
    public void Save()
    {
        Debug.Log("Saved");
        if (File.Exists(SavePath))
        {
            using (File.Create(SavePath)) { }
        }
        StreamWriter streamWriter = new StreamWriter(SavePath, false);
        data[(int)DataAddress.DAY] = days.ToString();
        data[(int)DataAddress.HOUSEDSINNER] = null;
        data[(int)DataAddress.MEMORY] = null;
        for (int i = 0; i < housedSinners.Length; i++)
        {
            data[(int)DataAddress.HOUSEDSINNER] += Convert.ToInt32(housedSinners[i]);
            data[(int)DataAddress.MEMORY] += Convert.ToInt32(sinnerMemory[i]);
        }
        string str = string.Join("\n", data);
        streamWriter.WriteLine(str);
        streamWriter.Flush();
        streamWriter.Close();
    }
    public void Load()
    {
        StreamReader streamReader = new StreamReader(SavePath, true);
        int lineCount = 0;
        const int DataEnd = -1;
        while (streamReader.Peek() != DataEnd)
        {
            data[lineCount] = streamReader.ReadLine();
            lineCount++;
        }

        streamReader.Close();
        days = int.Parse(data[(int)DataAddress.DAY]);
        string housed = data[(int)DataAddress.HOUSEDSINNER];
        string memory = data[(int)DataAddress.MEMORY];
        for (int i = 0; i < data[(int)DataAddress.HOUSEDSINNER].Length; i++)
        {
            housedSinners[i] = Convert.ToBoolean(housed[i] - adjustment);
            //sinnerMemory[i] = Convert.ToBoolean(memory[i] - adjustment);
        }     
    }
}
