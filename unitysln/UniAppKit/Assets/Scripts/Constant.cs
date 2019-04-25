using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
	public const string Domain = "http://127.0.0.1";
	
	public static string DataBaseDir{
		get{
			return Path.Combine(Application.persistentDataPath, "db");
		}
	}

	public static string DataBaseFile{
		get{
			return "data.db";
		}
	}

	public static string DataBasePath{
		get{
			return Path.Combine(DataBaseDir, DataBaseFile);
		}
	}

	public static class BootloaderStep
	{
		public const string FetchProfile = "FetchProfile";
		public const string RefreshProfile = "RefreshProfile";
	}
}
