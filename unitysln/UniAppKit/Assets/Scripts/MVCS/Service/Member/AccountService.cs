using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;
using SimpleJSON;

public class AccountService : Service
{
    public const string NAME = "AccountService";

    protected override void setup()
    {
    }

    protected override void dismantle()
    {
    }

    private AccountModel model
    {
        get
        {
            return modelCenter_.FindModel(AccountModel.NAME) as AccountModel;
        }
    }

    public void FetchProfile(string _accountID, Action _onFinish)
    {
        Dictionary<string, Any> param = new Dictionary<string, Any>();
        param.Add("accountID", new Any(_accountID));
        post("/account/profile/fetch", param, (_reply) =>
        {
            handleFetchProfile(_reply);
            if(null != _onFinish)
                _onFinish();
        }, (_error) =>
        {
            AccountModel.AccountStatus status = new AccountModel.AccountStatus();
            status.code = -1;
            status.message = _error;
            model.SaveProfile(status, null);
        }, null);
    }

    private void handleFetchProfile(string _reply)
    {
        AccountModel.AccountStatus status = new AccountModel.AccountStatus();
        string profile = "";
        ServiceUtility.HandleReply(_reply, status, (_data) =>
        {
            profile = _data["profile"].Value;
        });

        model.SaveProfile(status, profile);
    } 
}
