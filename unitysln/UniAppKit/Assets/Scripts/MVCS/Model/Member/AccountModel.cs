using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;
using XTC.Logger;

public class AccountModel : Model
{
    public const string NAME = "AccountModel";

    public const string OFFLINE_ACCOUNT_ID = "00000000000000000000000000000000"; //32 zero

    public class Property
    {
        public const string Profile = "profile";
    }

    public class Profile
    {
        public string nickname = "";
    }

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
        property[Property.Profile] = new Profile();
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

    public void SaveActiveAccount(string _accountID)
    {
        DataCache.activeAccountID = _accountID;
        Account account = new Account();
        account.accountID = _accountID;
        status.activeAccount = account;
    }

    public void SaveProfile(AccountStatus _status, string _profile)
    {
        if(0 != _status.code)
        {
            this.LogError(_status.message);
            return;
        }
        status.activeAccount.profile = _profile;
        property[Property.Profile] = _profile;
    }

    public void RefreshProfile()
    {
        string profileJson = (string)property[Property.Profile];
        Profile profile = JsonUtility.FromJson<Profile>(profileJson);
        if(null == profile)
            return;
        controller.RefreshProfile(profile);
    }

    public void UpdateEnterLobby()
    {
        controller.EnterLobby();
    }
}