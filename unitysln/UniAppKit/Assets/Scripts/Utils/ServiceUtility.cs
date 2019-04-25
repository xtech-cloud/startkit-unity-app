using System;
using System.Text;
using System.Collections;
using UnityEngine;
using SimpleJSON;
using XTC.MVCS;
using XTC.Logger;

public class ServiceUtility
{
    public delegate void DataHandler(JSONClass _data);

    public static void HandleReply(string _reply, Model.Status _status, DataHandler _dataHandler)
    {
        try
        {
            JSONClass root = JSON.Parse(_reply).AsObject;
            _status.code = root["code"].AsInt;
            _status.message = root["message"].Value;

            if (0 == _status.code)
            {
                if(null != _dataHandler)
                    _dataHandler(root["data"].AsObject);
            }
            else
            {
                Log.Warning("ServiceUtility.HandleReply", "{0} : {1}", _status.code, _status.message);
            }
        }
        catch (System.Exception e)
        {
            Log.Warning("ServiceUtility.HandleReply", "{0}", e.Message);
            _status.code = -1;
            _status.message = e.Message;
        }
    }
}
