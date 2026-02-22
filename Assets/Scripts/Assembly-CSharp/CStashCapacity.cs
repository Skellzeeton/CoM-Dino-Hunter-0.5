public class CStashCapacity
{
	public int nLevel;

	public bool isCrystalPurchase;

	public int nPrice;

	public int nCapacity;

	public string sLevelUpDesc = string.Empty;

	public CStashCapacity(int level, bool iscrystalpurchase, int price, int capacity, string lvlupdesc)
	{
		nLevel = level;
		isCrystalPurchase = iscrystalpurchase;
		nPrice = price;
		nCapacity = capacity;
		sLevelUpDesc = lvlupdesc;
	}
}
