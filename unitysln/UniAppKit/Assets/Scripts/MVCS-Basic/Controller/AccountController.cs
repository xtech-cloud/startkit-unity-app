using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XTC.MVCS;

public class AccountController : Controller
{
    public const string NAME = "AccountController";

    protected override void setup()
    {
    }

    protected override void dismantle()
    {
    }

    private AccountService service {
        get {
            return serviceCenter_.FindService(AccountService.NAME) as AccountService;
        }
    }

    private AccountModel model {
        get {
            return modelCenter_.FindModel(AccountModel.NAME) as AccountModel;
        }
    }

    public void FetchProfile(AccountModel.AccountStatus _status)
    {
        if(_status.activeAccount.accountID.EndsWith(AccountModel.OFFLINE_ACCOUNT_ID))
        {
            model.SaveProfile("{}");
            model.UpdateEnterLobby();
            return;
        }

        service.FetchProfile(_status.activeAccount.accountID);
    }

    public void EnterLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    
}
