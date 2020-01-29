using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RestService
{
    public bool Running { get; private set; }

    public IEnumerator Post(string url, object obj)
    {
        Running = true;

        var formData = new WWWForm();
        formData.AddField("jsonObject", JsonConvert.SerializeObject(obj));

        var www = UnityWebRequest.Post(url, formData);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
            Debug.LogError(www.error);

        Debug.Log(www.downloadHandler.text);

        Running = false;
    }

    public IEnumerator Get<T>(string url, Response<T> obj, Action<T> onSuccess = null, Action<string> onError = null)
    {
        yield return Get<T>(url, (resp) => { obj.Result = resp; onSuccess?.Invoke(resp);});
    }

    public IEnumerator Get<T>(string url, Action<T> onSuccess, Action<string> onError = null)
    {
        Running = true;

        var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError) {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
            var obj = JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
            onSuccess?.Invoke(obj);
        }

        Running = false;
    }
}
