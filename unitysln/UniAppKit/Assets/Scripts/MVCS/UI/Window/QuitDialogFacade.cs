using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

[System.Serializable]
public class QuitDialogUI
{
	public GameObject root;
	public Button btnOK;
	public Button btnCancel;
}

public class QuitDialogFacade :  UIFacade
{
	public const string NAME = "QuitDialog";

	public QuitDialogUI uiQuitDialog;
}
