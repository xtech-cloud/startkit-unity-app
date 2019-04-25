using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using XTC.Types;
using XTC.MVCS;

public class AccountMock
{
    private static AccountDAO daoAccount = new AccountDAO();

    public static void AutoMigrate()
    {
        daoAccount.AutoMigrate();
    }

    public static IEnumerator Processor(string _url, string _method, Dictionary<string, Any> _params, Service.OnReplyCallback _onReply, Service.OnErrorCallback _onError, Service.Options _options)
    {
        yield return new WaitForEndOfFrame();

        if (_url.EndsWith("/account/profile/fetch"))
        {
            fetchProfile(_params, _onReply, _onError);
        }
        else
        {
            _onError("404");
        }
    }

    private static void fetchProfile(Dictionary<string, Any> _params, Service.OnReplyCallback _onReply, Service.OnErrorCallback _onError)
    {
        MockReply reply = new MockReply();

        try
        {
            string accountID = _params["accountID"].AsString;
            Error error;
            AccountModel.Account account = daoAccount.QueryProfile(accountID, out error);
            reply.data.Add("profile", account.profile);
        }
        catch (System.Exception e)
        {
            reply.code = -1;
            reply.message = e.Message;
        }

        _onReply(reply.ToJSON());
    }
}
