using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using XTC.MVCS;
using XTC.Text;

public class LobbyNavigationView : View
{
	public const string NAME = "LobbyNavigationView";

	private LobbySampleUI uiSample{
		get{
			return (UIFacade.Find(LobbyNavigationFacade.NAME) as LobbyNavigationFacade).uiSample;
		}
	}

	protected override void setup()
	{
		uiSample.tgTab.isOn = false;
		uiSample.tfPage.gameObject.SetActive(false);
	}

	protected override void bindEvents()
	{
		uiSample.tgTab.onValueChanged.AddListener(onTabChanged);
		uiSample.btnEnterRoom.onClick.AddListener(onEnterClick);
	}

	protected override void unbindEvents()
	{
		uiSample.tgTab.onValueChanged.RemoveListener(onTabChanged);
		uiSample.btnEnterRoom.onClick.RemoveListener(onEnterClick);
	}

	protected override void dismantle()
	{

	}

	private void onTabChanged(bool _toggled)
	{
		uiSample.tfPage.gameObject.SetActive(_toggled);
	}

	private void onEnterClick()
	{
		SceneManager.LoadScene("Room");
	}

}
