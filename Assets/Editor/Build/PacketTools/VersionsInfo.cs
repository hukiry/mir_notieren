namespace Hukiry.Pack
{
	public class VersionsInfo
	{
		public int firstVer = 1;
		public int middleVer = 0;
		public int tailVer = 0;

		public VersionsInfo(string ver)
		{
			try
			{
				string[] vers = ver.Split('.');
				int.TryParse(vers[0], out firstVer);
				int.TryParse(vers[1], out middleVer);
				int.TryParse(vers[2], out tailVer);
			}
			catch
			{
				firstVer = 1;
				middleVer = 0;
				tailVer = 0;
			}
		}

		public string GetVersionName()
		{
			return string.Format("{0}.{1}.{2}", firstVer, middleVer, tailVer);
		}

		public int GetVersionCode()
		{
			return firstVer * 100 + middleVer;
		}

		public string GetLargeVersions()
		{
			return string.Format("{0}.{1}", firstVer, middleVer);
		}

		public string AddOversizeVersions()
		{
			return string.Format("{0}.{1}.{2}", firstVer, middleVer, tailVer + 1);
		}

		public string AddOversizeLargeVersions()
		{
			return string.Format("{0}.{1}.{2}", firstVer, middleVer + 1, 0);
		}

		public override string ToString()
		{
			return GetVersionName();
		}
	}
}