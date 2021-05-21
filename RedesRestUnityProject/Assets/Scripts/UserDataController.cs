using TMPro;
using UnityEngine;

public class UserDataController : MonoBehaviour
{

	[SerializeField] private TMP_Text usernameData;

	private void Start()
	{
		usernameData.text = UserDataService.Username;
	}
}