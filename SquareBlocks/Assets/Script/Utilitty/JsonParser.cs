using System.IO;
using UnityEngine;
using FullSerializer;
using System;
//This is a utility class
public sealed class JsonParser {

    //private const string path = @"c:\DroppApp\"
    private string path = Application.persistentDataPath;

    private string fileName = "AddData.json";
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
    private static readonly fsSerializer _serializer = new fsSerializer();

    // Eg: JsonParser.Serialize(typeof(Dictionary<string,string>), obj);
    public static string Serialize(Type type, object value)
    {
        // serialize the data
        fsData data;
        _serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

        // emit the data via JSON
        return fsJsonPrinter.CompressedJson(data);
    }

    // EG: JsonParser.Deserialize(typeof(Dictionary<string, string>), System.IO.File.ReadAllText(path));
    public static object Deserialize(Type type, string serializedState)
    {
        // step 1: parse the JSON data
        fsData data = fsJsonParser.Parse(serializedState);

        // step 2: deserialize the data
        object deserialized = null;
        _serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

        return deserialized;
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


