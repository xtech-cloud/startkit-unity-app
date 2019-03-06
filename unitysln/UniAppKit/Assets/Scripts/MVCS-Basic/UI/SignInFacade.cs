using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

[System.Serializable]
public class SignInUI
{
	public InputField inputUsername;
	public InputField inputPassword;
	public Button btnSignin;
	public Text txtError;
}

public class SignInFacade :  UIFacade
{
	public SignInUI uiSignIn;
}
