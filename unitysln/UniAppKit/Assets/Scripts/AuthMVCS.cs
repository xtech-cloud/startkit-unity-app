using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using XTC.MVCS;
using XTC.Types;

public class AuthMVCS : RootMono
{
    public UIFacade[] uIFacades;

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");
        
        foreach(UIFacade facade in uIFacades)
            facade.Register();

        initialize();

        //windowtools
        WindowToolsView viewWindowTools = new WindowToolsView();
        framework.viewCenter.Register(WindowToolsView.NAME, viewWindowTools);

        // quit dialog
        QuitDialogView viewQuitDialog = new QuitDialogView();
        framework.viewCenter.Register(QuitDialogView.NAME, viewQuitDialog);

        // account
        AccountService serviceAccount = new AccountService();
        AccountModel modelAccount = new AccountModel();
        AccountController controllerAccount = new AccountController();
        framework.modelCenter.Register(AccountModel.NAME, modelAccount);
        framework.controllerCenter.Register(AccountController.NAME, controllerAccount);
        framework.serviceCenter.Register(AccountService.NAME, serviceAccount);

        //signin
        SignInView viewSignIn = new SignInView();
        SignInModel modelSignIn = new SignInModel();
        SignInController controllerSignIn = new SignInController();
        SignInService serviceSignIn = new SignInService();

        
        serviceSignIn.domain = Constant.Domain;
        /* 
        serviceSignIn.MockProcessor = AuthMock.Processor;
        serviceSignIn.useMock = true;
        */
        framework.viewCenter.Register(SignInView.NAME, viewSignIn);
        framework.modelCenter.Register(SignInModel.NAME, modelSignIn);
        framework.controllerCenter.Register(SignInController.NAME, controllerSignIn);
        framework.serviceCenter.Register(SignInService.NAME, serviceSignIn);
    }

    void OnEnable()
    {
        Debug.Log("---------------  OnEnable ------------------------");
        setup();
    }

    void OnDisable()
    {
        Debug.Log("---------------  OnDisable ------------------------");
        dismantle();
    }

    void OnDestroy()
    {
        Debug.Log("---------------  OnDestroy ------------------------");

        framework.viewCenter.Cancel(SignInView.NAME);

        foreach(UIFacade facade in uIFacades)
            facade.Cancel();
            
        release();
    }
}
