using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;

public class SignInModel : Model
{
    public const string NAME = "SignInModel";

    public class SignInStatus : Model.Status
    {
        public Error latestError = Error.OK;
    }

    protected override void setup()
    {
        status_ = new SignInStatus();
    }

    protected override void dismantle()
    {
    }

    private SignInController controller
    {
        get
        {
            return controllerCenter_.FindController(SignInController.NAME) as SignInController;
        }
    }

    private SignInStatus status
    {
        get
        {
            return status_ as SignInStatus;
        }
    }

    public void UpdateLoginResult(Error _error)
    {
        status.latestError = _error;
        controller.UpdateLoginResult(status);
    }
}