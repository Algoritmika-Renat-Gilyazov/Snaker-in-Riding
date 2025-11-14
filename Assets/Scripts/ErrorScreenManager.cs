using UnityEngine;
using System.Collections;
using TMPro;

public class ErrorScreenManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI err;
	void Start()
	{
		if(PlayerPrefs.HasKey("ErrorCode"))
		{
			err.text = PlayerPrefs.GetString("ErrorCode");
		} else
		{
			err.text = "UNKNOWN_ERROR";
		}
		StartCoroutine("Wait");
	}

	void Update()
	{

	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(10f);
		Application.Quit();
	}
}