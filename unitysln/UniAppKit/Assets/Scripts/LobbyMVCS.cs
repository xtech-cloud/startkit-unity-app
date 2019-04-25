using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using XTC.MVCS;
using XTC.Types;
using XTC.Logger;

public class LobbyMVCS : RootMono
{
    public UIFacade[] uIFacades;

    private BootloaderModel modelBootloader = null;
    private BootloaderBatchController controllerBootloader = null;
    private AccountModel modelAccount = null;
    private AccountService serviceAccount = null;

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");
        
        //common facades
        foreach (UIFacade facade in uIFacades)
            facade.Register();

        //custom facades
        UIFacade[] facades = this.transform.Find("UIFacades").GetComponentsInChildren<UIFacade>();
        foreach (UIFacade facade in facades)
            facade.Register();

        initialize();

        // window tools
        WindowToolsView viewWindowTools = new WindowToolsView();
        framework.viewCenter.Register(WindowToolsView.NAME, viewWindowTools);

        // quit dialog
        QuitDialogView viewQuitDialog = new QuitDialogView();
        framework.viewCenter.Register(QuitDialogView.NAME, viewQuitDialog);

        // bootloader
        BootloaderView viewBootloader = new BootloaderView();
        framework.viewCenter.Register(BootloaderView.NAME, viewBootloader);
        controllerBootloader = new BootloaderBatchController();
        framework.controllerCenter.Register(BootloaderBatchController.NAME, controllerBootloader);
        modelBootloader = new BootloaderModel();
        framework.modelCenter.Register(BootloaderModel.NAME, modelBootloader);

        // account
        serviceAccount = new AccountService();
        modelAccount = new AccountModel();
        AccountController controllerAccount = new AccountController();
        AccountProfileView viewAccountProfile = new AccountProfileView();
        framework.modelCenter.Register(AccountModel.NAME, modelAccount);
        framework.viewCenter.Register(AccountProfileView.NAME, viewAccountProfile);
        framework.controllerCenter.Register(AccountController.NAME, controllerAccount);
        framework.serviceCenter.Register(AccountService.NAME, serviceAccount);
        serviceAccount.domain = Constant.Domain;
        serviceAccount.MockProcessor = AccountMock.Processor;
        serviceAccount.useMock = DataCache.offline;

        LobbyNavigationView viewNavigation = new LobbyNavigationView();
        framework.viewCenter.Register(LobbyNavigationView.NAME, viewNavigation);
    }

    void OnEnable()
    {
        Debug.Log("---------------  OnEnable ------------------------");
        setup();
    }

    void Start()
    {
        Debug.Log("---------------  Start ------------------------");

        Error error;
        SQLiteUtility.Open(Constant.DataBasePath, out error);
        if(Error.OK != error)
            this.LogError(error);

        // setup data
        modelAccount.SaveActiveAccount(DataCache.activeAccountID);

        executeBootloader();
    }

    void OnDisable()
    {
        Debug.Log("---------------  OnDisable ------------------------");
        dismantle();
    }

    void OnDestroy()
    {
        Debug.Log("---------------  OnDestroy ------------------------");

        UIFacade[] facades = this.transform.Find("UIFacades").GetComponentsInChildren<UIFacade>();
        foreach (UIFacade facade in facades)
            facade.Cancel();
        foreach(UIFacade facade in uIFacades)
            facade.Cancel();
            
        release();

        Error error;
        SQLiteUtility.Close(Constant.DataBasePath, out error);
    }

    private void executeBootloader()
    {
        List<BootloaderModel.Step> steps = new List<BootloaderModel.Step>();

        // fetch profile
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
            step.name = Constant.BootloaderStep.FetchProfile;
            step.length = 1;
            step.tip = "bootloader_step_fetch_profile";
            step.onExecute = () =>
            {
                serviceAccount.FetchProfile(DataCache.activeAccountID, ()=>{
                    controllerBootloader.FinishCurrentStep();
                });
            };
            steps.Add(step);

            modelBootloader.SaveSteps(steps);
        }

        // refresh profile
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
            step.name = Constant.BootloaderStep.RefreshProfile;
            step.length = 1;
            step.tip = "bootloader_step_refresh_profile";
            step.onExecute = () =>
            {
                modelAccount.RefreshProfile();
                controllerBootloader.FinishCurrentStep();
            };
            steps.Add(step);

            modelBootloader.SaveSteps(steps);
        }

        controllerBootloader.Execute();
    }
}
