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

    private AccountProfileView viewProfile {
        get {
            return viewCenter_.FindView(AccountProfileView.NAME) as AccountProfileView;
        }
    }

    public void EnterLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void RefreshProfile(AccountModel.Profile _profile)
    {
        viewProfile.RefreshProfile(_profile);
    }
    
}
