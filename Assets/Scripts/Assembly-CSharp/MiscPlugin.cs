using System.Runtime.InteropServices;

public class MiscPlugin
{
	[DllImport("__Internal")]
	protected static extern bool OSIsIAPCrack();

	public static bool IsIAPCrack()
	{
		return OSIsIAPCrack();
	}
}
