using UnityEngine;

public class WallContorller : MonoBehaviour
{
	public GameObject unbrokenColumn;
	public GameObject brokenColumn;

	public bool isBroken;


	void Start()
	{
		if (isBroken)
		{
			BreakColumn();
		}
		else
		{
			CinematicManager.instance.wallBreakFunc += BreakColumn;

			unbrokenColumn.SetActive(true);
			brokenColumn.SetActive(false);
		}
	}

	public void BreakColumn()
	{
		//isBroken = true;
		unbrokenColumn.SetActive(false);
		brokenColumn.SetActive(true);
	}
	
}