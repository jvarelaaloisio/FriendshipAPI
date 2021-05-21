using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
	private const string WS_LOGIN_SIGNATURE = "/ValidateUser";
	private const string WS_CREATE_SIGNATURE = "/GenerateNewUser";
	private const string WS_GET_USER_ID = "/GetUserId";
	private const string FIELD_USERNAME = "username";
	private const string FIELD_PASSWORD = "password";
	[SerializeField] private TMP_InputField username;
	[SerializeField] private TMP_InputField password;
	[SerializeField] private Image login;
	[SerializeField] private Image createAccount;
	[SerializeField] private int friendsScene;
	private Dictionary<string, string> _fields;

	private void Awake()
	{
		_fields = new Dictionary<string, string>
		{
			{FIELD_USERNAME, string.Empty},
			{FIELD_PASSWORD, string.Empty}
		};
	}

	public void TryLogin()
	{
		_fields[FIELD_USERNAME] = username.text;
		_fields[FIELD_PASSWORD] = password.text;
		StartCoroutine(
			WebServiceHelper
				.Post(
					UrlManager.FriendshipServiceUrl + WS_LOGIN_SIGNATURE,
					_fields,
					(result) => VerifyResult(login, username.text, bool.Parse(result)
					)
				)
		);
	}

	public void CreateAccount()
	{
		_fields[FIELD_USERNAME] = username.text;
		_fields[FIELD_PASSWORD] = password.text;
		StartCoroutine(
			WebServiceHelper
				.Post(
					UrlManager.FriendshipServiceUrl + WS_CREATE_SIGNATURE,
					_fields,
					(result) => VerifyResult(
						createAccount,
						username.text,
						bool.Parse(result)
					)
				)
		);
	}

	private static void ColorButton(Graphic button, bool result)
	{
		button.color = result ? Color.green : Color.red;
	}

	private void VerifyResult(Graphic button, string username, bool result)
	{
		ColorButton(button, result);
		if (!result)
			return;
		UserDataService.Username = username;
		StartCoroutine(
			WebServiceHelper
				.Post(
					UrlManager.FriendshipServiceUrl + WS_GET_USER_ID,
					new Dictionary<string, string>() {{FIELD_USERNAME, username}},
					response =>
					{
						var userId = int.Parse(response);
						Debug.Log($"User Id is {userId}");
						UserDataService.UserId = userId;
						SceneManager.LoadSceneAsync(friendsScene, LoadSceneMode.Additive);
						SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
					}
				)
		);
	}
}