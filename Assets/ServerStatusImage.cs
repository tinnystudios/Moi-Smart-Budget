using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerStatusImage : MonoBehaviour, IDataBind<Server>
{
    public Color Offline = Color.red;
    public Color Online = Color.green;

    public Image Image;
    private Server _server;

    private void Update()
    {
        Image.color = _server.Online ? Online : Offline;
    }

    public void Bind(Server data)
    {
        _server = data;
    }
}
