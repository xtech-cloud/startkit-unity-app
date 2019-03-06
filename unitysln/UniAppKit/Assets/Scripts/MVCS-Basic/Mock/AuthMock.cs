using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.Types;
using XTC.MVCS;

public class AuthMock 
{
	public static IEnumerator Processor(string _url, string _method, Dictionary<string, Any> _params, Service.OnReplyCallback _onReply, Service.OnErrorCallback _onError, Service.Options _options)
    {
        yield return new WaitForEndOfFrame();

        if (_url.EndsWith("/login"))
        {
            if (_params["username"].AsString.Equals("admin") && _params["password"].AsString.Equals("admin"))
                _onReply("ok");
            else
                _onError("errcode:1");
            yield break;
        }

        _onError("");
    }
}
