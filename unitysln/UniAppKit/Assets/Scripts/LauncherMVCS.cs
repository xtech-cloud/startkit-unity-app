using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using XTC.MVCS;
using XTC.Types;
using XTC.Text;

public class LauncherMVCS : RootMono
{
    public UIFacade[] uIFacades;
    

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");

        applyStyle();
        
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

    void Start()
    {
        Debug.Log("---------------  Start ------------------------");

        Translator.language = "zh_CN";
        Translator.MergeFromResource("translator/UI", true);

        StartCoroutine(enterAuth());
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

    private void applyStyle()
    {
        if(RuntimePlatform.WindowsPlayer == Application.platform)
            WindowUtility.ApplyWindowsStyle();
    }

    private IEnumerator enterAuth()
    {
        yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene("Auth");
    }
}
