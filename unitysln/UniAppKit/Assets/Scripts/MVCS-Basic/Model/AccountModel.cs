using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;

public class AccountModel : Model
{
    public const string NAME = "AccountModel";

    public class AccountStatus : Model.Status
    {
        public Error latestError = Error.OK;
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

    public void UpdateLoginResult(Error _error)
    {
        status.latestError = _error;
        //controller.UpdateLoginResult(status);
    }
}