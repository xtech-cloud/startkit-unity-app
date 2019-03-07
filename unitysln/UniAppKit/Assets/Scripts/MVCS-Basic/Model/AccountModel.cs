using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;

public class AccountModel : Model
{
    public const string NAME = "AccountModel";

    public const string OFFLINE_ACCOUNT_ID = "guest";

    public class Account
    {
        public string accountID = "";
        public string profile = "";
    }

    public class AccountStatus : Model.Status
    {
        public Error latestError = Error.OK;
        public Account activeAccount = null;
    }

    protected override void setup()
    {
        status_ = new AccountStatus();
    }

    protected override void dismantle()
    {
    }

    private AccountStatus status
    {
        get
        {
            return status_ as AccountStatus;
        }
    }

    private AccountController controller
    {
        get{
            return controllerCenter_.FindController(AccountController.NAME) as AccountController;
        }
    }

    public void UpdateLoginResult(Error _error)
    {
        status.latestError = _error;
        //controller.UpdateLoginResult(status);
    }

    public void UpdateActiveAccount(Account _account)
    {
        status.activeAccount = _account;
        controller.FetchProfile(status);
    }

    public void SaveProfile(string _profile)
    {
        status.activeAccount.profile = _profile;
    }

    public void UpdateEnterLobby()
    {
        controller.EnterLobby();
    }
}