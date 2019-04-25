using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MVCS;

public class WindowToolsView : View
{
	public const string NAME = "WindowToolsView";

	private WindowToolsUI uiWindowTools
	{
		get{
			return (UIFacade.Find(WindowToolsFacade.NAME) as WindowToolsFacade).uiWindowTools;
		}
	}

	protected override void setup()
	{
		uiWindowTools.root.SetActive(RuntimePlatform.WindowsPlayer == Application.platform);
	}

	protected override void bindEvents()
	{
		uiWindowTools.btnMinimize.onClick.AddListener(onMinimizeClick);
		uiWindowTools.btnClose.onClick.AddListener(onCloseClick);
	}

	protected override void unbindEvents()
	{
		uiWindowTools.btnMinimize.onClick.RemoveListener(onMinimizeClick);
		uiWindowTools.btnClose.onClick.RemoveListener(onCloseClick);
	}

	protected override void dismantle()
	{

	}

	private void onMaxmizeClick()
	{
		WindowUtility.Maximize();
	}

	private void onMinimizeClick()
	{
		WindowUtility.Minimize();
	}

	private void onCloseClick()
	{
		(center_.FindView(QuitDialogView.NAME) as QuitDialogView).Popup();
	}
}
