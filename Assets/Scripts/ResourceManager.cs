using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ResourceManager
{
    public string CachePath;
    public string UnsentPath;

    public const string UnsentFolder = "Unsent";
    public const string CacheResourceFolder = "Resources";

    public void Initialize()
    {
        CachePath = $"{Application.persistentDataPath}/{CacheResourceFolder}";
        UnsentPath = $"{Application.persistentDataPath}/{UnsentFolder}";

        if (!Directory.Exists(CachePath)) Directory.CreateDirectory(CachePath);
        if (!Directory.Exists(UnsentPath)) Directory.CreateDirectory(UnsentPath);
    }

    public void CacheResource(object obj, string fileName)
    {
        Cache(obj, BuildResourcePath(fileName));
    }

    public void CacheUnsent(object obj, string fileName)
    {
        Cache(obj, BuildUnsentPath(fileName));
    }

    public void Cache(object obj, string path)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        Cache(json, path);
    }

    public void Cache(string json, string path)
    {
        File.WriteAllText(path, json);
    }

    public bool HasCache(string path)
    {
        return File.Exists(path);
    }

    public T Get<T>(string fileName)
    {
        var json = File.ReadAllText(BuildResourcePath(fileName));
        return JsonConvert.DeserializeObject<T>(json);
    }

    public string BuildUnsentPath(string fileName) => $"{UnsentPath}/{fileName}";
    public string BuildResourcePath(string fileName) => $"{CachePath}/{fileName}";
}
