using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using XTC.MVCS;
using XTC.Types;

public class LobbyMVCS : RootMono
{
    public UIFacade[] uIFacades;

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");
        
        
        foreach(UIFacade facade in uIFacades)
            facade.Register();

        initialize();

        // window tools
        WindowToolsView viewWindowTools = new WindowToolsView();
        framework.viewCenter.Register(WindowToolsView.NAME, viewWindowTools);

        // quit dialog
        QuitDialogView viewQuitDialog = new QuitDialogView();
        framework.viewCenter.Register(QuitDialogView.NAME, viewQuitDialog);
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

        foreach(UIFacade facade in uIFacades)
            facade.Cancel();
            
        release();
    }
}
