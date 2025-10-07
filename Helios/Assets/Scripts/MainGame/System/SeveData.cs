using System.IO;
using UnityEngine;

public class SeveData : MonoBehaviour
{
    private StreamWriter streamWriter;
    private void Start()
    {
        Save();
    }
    public void Save()
    {
        streamWriter = new StreamWriter("./Assets/SaveData.txt", true);
        streamWriter.WriteLine("aaa");
        streamWriter.Flush();
        streamWriter.Close();
    }
    public void Load()
    {

    }
}
