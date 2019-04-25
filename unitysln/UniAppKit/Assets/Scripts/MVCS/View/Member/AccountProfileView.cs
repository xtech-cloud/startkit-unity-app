using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MVCS;
using XTC.Text;

public class AccountProfileView : View
{
	public const string NAME = "AccountProfileView";

	private AccountProfileUI uiAccountProfile
	{
		get{
			return (UIFacade.Find(AccountProfileFacade.NAME) as AccountProfileFacade).uiAccountProfile;
		}
	}

	protected override void setup()
	{
	}

	protected override void bindEvents()
	{
	}

	protected override void unbindEvents()
	{
	}

	protected override void dismantle()
	{

	}

	public void RefreshProfile(AccountModel.Profile _profile)
	{
		uiAccountProfile.txtNickname.text = _profile.nickname;
	}
}
