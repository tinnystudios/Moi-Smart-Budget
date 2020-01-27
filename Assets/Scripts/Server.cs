using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    // Post an expense
    public IEnumerator PostExpense()
    {
        var url = "http://192.168.1.85/AddExpenses.php";
        var formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        var www = UnityWebRequest.Post(url, formData);
        www.chunkedTransfer = false;////ADD THIS LINE
        yield return www.SendWebRequest();
    }
}
