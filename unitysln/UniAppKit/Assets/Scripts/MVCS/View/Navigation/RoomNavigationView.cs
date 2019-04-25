using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MVCS;
using XTC.Text;
using XTC.Blockly;

public class RoomNavigationView : View
{
	public const string NAME = "RoomNavigationView";

	private RoomTitleUI uiTitle{
		get{
			return (UIFacade.Find(RoomNavigationFacade.NAME) as RoomNavigationFacade).uiRoomTitle;
		}
	}

	protected override void setup()
	{
	}

	protected override void bindEvents()
	{
		uiTitle.btnBackLobby.onClick.AddListener(onBackLobbyClick);
	
	}

	protected override void unbindEvents()
	{
		uiTitle.btnBackLobby.onClick.RemoveListener(onBackLobbyClick);
	}

	protected override void dismantle()
	{

	}
	
	private void onBackLobbyClick()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
	}
}
