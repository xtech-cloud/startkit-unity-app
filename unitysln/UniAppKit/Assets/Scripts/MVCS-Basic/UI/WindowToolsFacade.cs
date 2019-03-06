using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

[System.Serializable]
public class WindowToolsUI
{
	public GameObject root;
	public Button btnMinimize;
	public Button btnClose;
}

public class WindowToolsFacade :  UIFacade
{
	public WindowToolsUI uiWindowTools;
}
