using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using XTC.MVCS;

public class ModelUtility
{
    public static string NewUUID()
    {
        string guid = Guid.NewGuid().ToString();
        return ToUUID(guid);
    }

    public static string ToUUID(string _text)
    {
        MD5 md5 = MD5.Create();
        byte[] byteOld = Encoding.UTF8.GetBytes(_text);
        byte[] byteNew = md5.ComputeHash(byteOld);
        StringBuilder sb = new StringBuilder();
        foreach (byte b in byteNew)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    public static string NewUtcNow()
    {
        return System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

    
}

public static class ModelExtend
{
    public static void MergeBase(this Model.Status _this, Model.Status _status)
    {
        _this.code = _status.code;
        _this.message = _status.message;
    }
}
