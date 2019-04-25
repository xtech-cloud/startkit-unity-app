using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;

public class SignInController : Controller
{
    public const string NAME = "SignInController";

    protected override void setup()
    {
    }

    protected override void dismantle()
    {
    }

    private SignInView view
    {
        get
        {
            return viewCenter_.FindView(SignInView.NAME) as SignInView;
        }
    }

    public void UpdateLoginResult(SignInModel.SignInStatus _status)
    {
        if (_status.latestError.IsOK)
        {
            throw new System.NotImplementedException("NotImplemented");
            AccountModel modelAccount = (modelCenter_.FindModel(AccountModel.NAME) as AccountModel);
			modelAccount.SaveActiveAccount("admin");
            modelAccount.UpdateEnterLobby();
        }
        else
        {
            view.RefreshSigninError(_status.latestError.message);
        }
    }
}
