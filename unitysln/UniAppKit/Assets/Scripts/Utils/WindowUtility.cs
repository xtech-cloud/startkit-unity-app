using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowUtility
{
    
    public static void ApplyWindowsStyle()
    {
#if UNITY_STANDALONE_WIN
        applyWindowsStyle();
#endif
    }

    public static void Maximize()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(GetForegroundWindow(), SW_SHOWMAXIMIZED);
#endif
    }

    public static void Minimize()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
#endif
    }

    public static void Restore()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(GetForegroundWindow(), SW_SHOWRESTORE);
#endif
    }

#if UNITY_STANDALONE_WIN

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, long dwNewLong);

    [DllImport("user32.dll")] 
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags); 


    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern int SystemParametersInfo(int uAction, int uParam, ref RECT re, int fuWinTni);

    [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
    public static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {

        public int cbSize;

        public IntPtr hWnd;

        public int uCallbackMessage;

        public int uEdge;

        public RECT rc;

        public IntPtr lParam;

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString()
        {
            return "{left=" + left.ToString() + ", " + "top=" + top.ToString() + ", " +
                "right=" + right.ToString() + ", " + "bottom=" + bottom.ToString() + "}";
        }

    }

    private const int ABE_LEFT = 0;
    private const int ABE_TOP = 1;
    private const int ABE_RIGHT = 2;
    private const int ABE_BUTTOM = 3;

    private const uint SWP_SHOWWINDOW = 0x0040;
    private const int GWL_STYLE = -16;

    //see https://docs.microsoft.com/en-us/windows/desktop/winmsg/window-styles
    #region  Window Styles

    private const long WS_POPUP = 0x80000000L;
    #endregion

    const int SW_SHOWMINIMIZED = 2;
    const int SW_SHOWMAXIMIZED = 3;
    const int SW_SHOWRESTORE = 1;

    private static void applyWindowsStyle()
    {
        Screen.fullScreen = false;
        Resolution[] resolutions = Screen.resolutions;
        Resolution maxResolution = resolutions[resolutions.Length - 1];
        
        //get data of taskbar
        int dwMessage=0x00000005;
        APPBARDATA pdat=new APPBARDATA();
        SHAppBarMessage(dwMessage,ref pdat);

        IntPtr Handle = GetForegroundWindow();
        //none border
        SetWindowLong(Handle, GWL_STYLE, WS_POPUP);

        int x = 0;
        int y = 0;
        int width = 0;
        int height = 0;
        if(ABE_BUTTOM == pdat.uEdge)
        {
            x = 0;
            y = 0;
            width = maxResolution.width;
            height = pdat.rc.top;
        }
        else if(ABE_TOP == pdat.uEdge)
        {
            x = 0;
            y = pdat.rc.bottom;
            width = maxResolution.width;
            height = maxResolution.height - pdat.rc.bottom;
        }
        else if(ABE_LEFT == pdat.uEdge)
        {
            x = pdat.rc.right;
            y = 0;
            width = maxResolution.width - pdat.rc.right;
            height = maxResolution.height;
        }
        else if(ABE_RIGHT == pdat.uEdge)
        {
            x = 0;
            y = 0;
            width = pdat.rc.left;
            height = maxResolution.height;
        }

        SetWindowPos(GetForegroundWindow(), 0, x, y, width, height, SWP_SHOWWINDOW); 
    }
#endif
}
