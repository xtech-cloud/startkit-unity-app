using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

[System.Serializable]
public class RoomTitleUI
{
	public Button btnBackLobby;
}


public class RoomNavigationFacade :  UIFacade
{
	public const string NAME = "RoomNavigationFacade";

	public RoomTitleUI uiRoomTitle;
}
