using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

[System.Serializable]
public class LobbySampleUI
{
	public Toggle tgTab;
	public Transform tfPage;
	public Button btnEnterRoom;
}

public class LobbyNavigationFacade :  UIFacade
{
	public const string NAME = "LobbyNavigationFacade";

	public LobbySampleUI uiSample;
}
