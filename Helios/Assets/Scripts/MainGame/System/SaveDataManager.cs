using System;
using System.IO;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private enum DataAddress
    {
        DAY=0,
        HOUSEDSINNER,
        MAX
    }
    private string[] data = new string[(int)DataAddress.MAX];
    private int days = 1;
    const int housei = 48;
    public int Days {  get { return days; } set { days = value; } }
    private const int maxSinners = 31;
    private bool[] housedSinners = new bool[maxSinners];
    public bool[] HousedSinner { get { return housedSinners; }set { housedSinners = value; } }
    private void Start()
    {
        Load();
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(2))
        //Save();
    }
    public void Save()
    {
        string path = @"\Assets/SaveData.txt";
        if (File.Exists(Application.dataPath+ path))
        {
            using (File.Create(path)) { }
        }
        StreamWriter streamWriter = new StreamWriter("./Assets/SaveData.txt", false);
        data[(int)DataAddress.DAY] = days.ToString();
        data[(int)DataAddress.HOUSEDSINNER] = null;
        for (int i = 0; i < housedSinners.Length; i++) 
            data[(int)DataAddress.HOUSEDSINNER] += Convert.ToInt32(housedSinners[i]);
        string str = string.Join("\n", data);
        streamWriter.WriteLine(str);
        streamWriter.Flush();
        streamWriter.Close();
    }
    public void Load()
    {
        StreamReader streamReader = new StreamReader("./Assets/SaveData.txt", true);
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
        for (int i = 0; i < data[(int)DataAddress.HOUSEDSINNER].Length; i++)
        {
            housedSinners[i] = Convert.ToBoolean((int)housed[i]-housei);
        }
          
    }
}
