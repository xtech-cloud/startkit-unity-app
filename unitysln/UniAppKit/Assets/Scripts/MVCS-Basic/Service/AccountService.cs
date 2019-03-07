using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;

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

    public void FetchProfile(string _userid)
    {
        Dictionary<string, Any> param = new Dictionary<string, Any>();
        param.Add("userid", new Any(_userid));
        post("/account/profile/fetch", param, (_reply) =>
        {
            model.SaveProfile("{}");
            model.UpdateEnterLobby();
        }, (_error) =>
        {
            Error err = Error.NewAccessErr(_error);
            //model.UpdateFetchProfileResult(err);
        }, null);
    }
}
