using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MockReply
{
	public int code = 0;
	public string message = "";

	public JSONClass data = new JSONClass();

	public string ToJSON()
	{
		JSONClass root = new JSONClass();
		root.Add("code", new JSONData(code));
		root.Add("messgae", new JSONData(code));
		root.Add("data", data);
		return root.ToJSON(0);
	}
}
