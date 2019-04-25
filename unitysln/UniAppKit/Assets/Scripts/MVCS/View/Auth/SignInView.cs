using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MVCS;
using XTC.Text;

public class SignInView : View
{
	public const string NAME = "SignInView";

	private SignInUI uiSignIn
	{
		get{
			return (UIFacade.Find("SignIn") as SignInFacade).uiSignIn;
		}
	}

	private SignInService service{
		get {
			return serviceCenter_.FindService(SignInService.NAME) as SignInService;
		}
	}

	protected override void setup()
	{
		RefreshSigninError("");
	}

	protected override void bindEvents()
	{
		uiSignIn.btnSignin.onClick.AddListener(onSignInClick);
		uiSignIn.btnOfflineEnter.onClick.AddListener(onOfflineEnterClick);
	}

	protected override void unbindEvents()
	{
		uiSignIn.btnSignin.onClick.RemoveListener(onSignInClick);
		uiSignIn.btnOfflineEnter.onClick.RemoveListener(onOfflineEnterClick);
	}

	protected override void dismantle()
	{

	}

	public void RefreshSigninError(string _error)
	{
		uiSignIn.txtError.text = _error;
	}

	private void onSignInClick()
	{
		//clear error tip
		RefreshSigninError("");

		string username = uiSignIn.inputUsername.text;
		if(string.IsNullOrEmpty(username))
		{
			string error = Translator.Translate("username_not_allow_empty");
			RefreshSigninError(error);
			return;
		}

		string password = uiSignIn.inputPassword.text;
		if(string.IsNullOrEmpty(password))
		{
			string error = Translator.Translate("password_not_allow_empty");
			RefreshSigninError(error);
			return;
		}

		service.CallLogin(username, password);
	}

	private void onOfflineEnterClick()
	{
		DataCache.offline = true;
		AccountModel modelAccount = (modelCenter_.FindModel(AccountModel.NAME) as AccountModel);
		modelAccount.SaveActiveAccount(AccountModel.OFFLINE_ACCOUNT_ID);
		modelAccount.UpdateEnterLobby();
	}


}
