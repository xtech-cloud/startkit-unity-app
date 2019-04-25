using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MVCS;

public class QuitDialogView : View
{
	public const string NAME = "QuitDialogView";

	private QuitDialogUI uiQuitDialog
	{
		get{
			return (UIFacade.Find(QuitDialogFacade.NAME) as QuitDialogFacade).uiQuitDialog;
		}
	}

	protected override void setup()
	{
		uiQuitDialog.root.SetActive(false);
	}

	protected override void bindEvents()
	{
		uiQuitDialog.btnOK.onClick.AddListener(onOKClick);
		uiQuitDialog.btnCancel.onClick.AddListener(onCancelClick);
	}

	protected override void unbindEvents()
	{
		uiQuitDialog.btnOK.onClick.RemoveListener(onOKClick);
		uiQuitDialog.btnCancel.onClick.RemoveListener(onCancelClick);
	}

	protected override void dismantle()
	{

	}

	public void Popup()
	{
		uiQuitDialog.root.SetActive(true);
	}

	private void onOKClick()
	{
		Application.Quit();
	}

	private void onCancelClick()
	{
		uiQuitDialog.root.SetActive(false);
	}
}
