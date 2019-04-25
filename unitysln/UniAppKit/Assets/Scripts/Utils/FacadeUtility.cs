using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MVCS;

public class FacadeUtility
{
	public static void RegisterWindowTools(Transform _parent)
	{
		GameObject goWindowTools = Resources.Load<GameObject>("ui/tootWindowTools");
        GameObject cloneWindowTools =GameObject.Instantiate(goWindowTools);
		Transform tsWindowTools = cloneWindowTools.transform;
		tsWindowTools.SetParent(_parent);
		tsWindowTools.position = Vector3.zero;
		tsWindowTools.rotation = Quaternion.identity;
		tsWindowTools.localScale = Vector3.one;
		
        UIFacade facade = cloneWindowTools.GetComponent<UIFacade>();
        facade.Register();
	}

	
}
