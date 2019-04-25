using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCache 
{
    public static string activeAccountID {
        get{
            return PlayerPrefs.GetString("account.active", AccountModel.OFFLINE_ACCOUNT_ID);
        }
        set{
            PlayerPrefs.SetString("account.active", value);
            PlayerPrefs.Save();
        }
    }

    public static string activeProjectUUID {
        get{
            return PlayerPrefs.GetString("project.active.uuid", "");
        }
        set{
            PlayerPrefs.SetString("project.active.uuid", value);
            PlayerPrefs.Save();
        }
    }

    public static string activeProjectType {
        get{
            return PlayerPrefs.GetString("project.active.type", "");
        }
        set{
            PlayerPrefs.SetString("project.active.type", value);
            PlayerPrefs.Save();
        }
    }

    public static string activeProjectName {
        get{
            return PlayerPrefs.GetString("project.active.name", "");
        }
        set{
            PlayerPrefs.SetString("project.active.name", value);
            PlayerPrefs.Save();
        }
    }

    public static string activeStoryUUID {
        get{
            return PlayerPrefs.GetString("story.active", "");
        }
        set{
            PlayerPrefs.SetString("story.active", value);
            PlayerPrefs.Save();
        }
    }

    public static string playEntry {
        get{
            return PlayerPrefs.GetString("play.entry", "");
        }
        set{
            PlayerPrefs.SetString("play.entry", value);
            PlayerPrefs.Save();
        }
    }

    public static string playMode {
        get{
            return PlayerPrefs.GetString("play.mode", "");
        }
        set{
            PlayerPrefs.SetString("play.mode", value);
            PlayerPrefs.Save();
        }
    }

    public static bool offline {
        get{
            return 1 == PlayerPrefs.GetInt("offline", 0);
        }
        set{
            PlayerPrefs.SetInt("offline", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static Vector3 gravity {
        get{
            float x = PlayerPrefs.GetFloat("gravity.x", 0);
            float y = PlayerPrefs.GetFloat("gravity.y", 0);
            float z = PlayerPrefs.GetFloat("gravity.z", 0);
            return new Vector3(x, y, z);
        }
        set{
            PlayerPrefs.SetFloat("gravity.x", value.x);
            PlayerPrefs.SetFloat("gravity.y", value.y);
            PlayerPrefs.SetFloat("gravity.z", value.z);
            PlayerPrefs.Save();
        }
    }
}
