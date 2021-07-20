using System.IO;
using UnityEngine;
//This is a utility class
public sealed class JsonParser {

    private string path = Application.persistentDataPath;

    private string fileName = "Data.json";
    private string extention = ".json";
    private static JsonParser instance = null;

    private JsonParser() {
    }

    public static JsonParser Instance {
        get{
            if(instance == null) {
                instance = new JsonParser();
            }
            return instance;
        }
    }


    public void WriteJsonData(string _data, string _FileName)
    {
        Directory.CreateDirectory(path);
        var s = _FileName + extention;
        string exPath = Path.Combine(path, s);
        File.WriteAllText(exPath, _data);
       // Debug.Log(exPath);
    }

    public void WriteJsonData(System.Object _data) {
        Directory.CreateDirectory(path);
        string exPath = Path.Combine(path, fileName);
        File.WriteAllText(exPath, JsonUtility.ToJson(_data));
        Debug.Log(exPath);
    }

    public void WriteJsonData(System.Object _data,string _FileName) {
        Directory.CreateDirectory(path);
        var s = _FileName + extention;
        string exPath = Path.Combine(path, s);
        File.WriteAllText(exPath, JsonUtility.ToJson(_data));
        Debug.Log(exPath);
    }

    public void WriteJsonData(System.Object _data, string _FileName,string _extention) {
        Directory.CreateDirectory(path);
        var s = _FileName + _extention;
        string exPath = Path.Combine(path, s);
        File.WriteAllText(exPath, JsonUtility.ToJson(_data));
        Debug.Log(exPath);
    }

    public string GetDefaultPath()
    {
        return path;
    }
    public string GetDefaultFullPath()
    {
        return path + fileName;
    }

}//JsonParser


