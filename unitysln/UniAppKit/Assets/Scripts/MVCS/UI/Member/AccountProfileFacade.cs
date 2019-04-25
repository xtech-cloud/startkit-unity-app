using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

[System.Serializable]
public class AccountProfileUI
{
	public Text txtNickname;
}

public class AccountProfileFacade :  UIFacade
{
	public const string NAME = "AccountProfileFacade";

	public AccountProfileUI uiAccountProfile;
}
