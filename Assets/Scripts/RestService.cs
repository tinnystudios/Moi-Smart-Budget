using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RestService
{
    public IEnumerator Post(string url, object obj)
    {
        var formData = new WWWForm();
        formData.AddField("jsonObject", JsonConvert.SerializeObject(obj));

        var www = UnityWebRequest.Post(url, formData);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
            Debug.LogError(www.error);

        Debug.Log(www.downloadHandler.text);
    }

    public IEnumerator Get<T>(string url, Response<T> obj, Action<string> OnError = null)
    {
        yield return Get<T>(url, (resp) => obj.Result = resp);
    }

    public IEnumerator Get<T>(string url, Action<T> OnSuccess, Action<string> OnError = null)
    {
        var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError) {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
            var obj = JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
            OnSuccess?.Invoke(obj);
        }
    }
}
