using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;

public class SignInService : Service
{
    public const string NAME = "SignInService";

    protected override void setup()
    {
    }

    protected override void dismantle()
    {
    }

    private SignInModel model
    {
        get
        {
            return modelCenter_.FindModel(SignInModel.NAME) as SignInModel;
        }
    }

    public void CallLogin(string _username, string _password)
    {
        Dictionary<string, Any> param = new Dictionary<string, Any>();
        param.Add("username", new Any(_username));
        param.Add("password", new Any(_password));
        post("/login", param, (_reply) =>
        {
            if (_reply.Equals("ok"))
                model.UpdateLoginResult(Error.OK);
            else
                model.UpdateLoginResult(Error.NewAccessErr(_reply));
        }, (_error) =>
        {
            Error err = Error.NewAccessErr(_error);
            model.UpdateLoginResult(err);
        }, null);
    }
}
