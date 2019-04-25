using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using XTC.MVCS;
using XTC.Types;
using XTC.Blockly;

public class RoomMVCS : RootMono
{
    public UIFacade[] uIFacades;
    public Transform canvas2D;

    private BootloaderModel modelBootloader = null;
    private BootloaderBatchController controllerBootloader = null;

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");

        Physics.gravity = Vector3.zero;
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

        // navigation
        RoomNavigationView viewNavigation = new RoomNavigationView();
        framework.viewCenter.Register(RoomNavigationView.NAME, viewNavigation);
    }

    void OnEnable()
    {
        Debug.Log("---------------  OnEnable ------------------------");
        setup();
    }

    void Start()
    {
        Debug.Log("---------------  Start ------------------------");

        executeBootloader();
    }

    void Update()
    {
    }

    void LateUpdate()
    {
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
        foreach (UIFacade facade in uIFacades)
            facade.Cancel();

        release();

        Error error;
        SQLiteUtility.Close(Constant.DataBasePath, out error);
    }

    private void executeBootloader()
    {
        List<BootloaderModel.Step> steps = new List<BootloaderModel.Step>();

        // add a step
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
            step.name = "loading";
            step.length = 1;
            step.tip = "loading ...";
            step.onExecute = () =>
            {
                RootMono.instance.StartCoroutine(sleep());
            };
            steps.Add(step);
        }
        
        modelBootloader.SaveSteps(steps);
        controllerBootloader.Execute();
    }

    private IEnumerator sleep()
    {
        yield return new WaitForSeconds(3.0F);
        controllerBootloader.FinishCurrentStep();
    }

}
