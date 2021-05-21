using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AddFriendController : MonoBehaviour
{
	private const string WS_SEND_FRIEND_REQUEST = "/SendFriendRequest";
	private const string FIELD_USER_ID = "userId";
	private const string FIELD_FRIEND_NAME = "friendName";
	[SerializeField] private TMP_Text friendRequestSentFeedback;
	private Dictionary<string, string> _fields;

	private void Awake()
	{
		_fields = new Dictionary<string, string>
		{
			{FIELD_USER_ID, UserDataService.UserId.ToString()},
			{FIELD_FRIEND_NAME, string.Empty}
		};
	}

	public void AddFriend(TMP_InputField friendName)
	{
		_fields[FIELD_FRIEND_NAME] = friendName.text;
		StartCoroutine(
			WebServiceHelper.Post(
				UrlManager.FriendshipServiceUrl + WS_SEND_FRIEND_REQUEST,
				_fields,
				response => AddFriendCallback(int.Parse(response))
			)
		);
	}

	private void AddFriendCallback(int result)
	{
		bool resultValue = result > 0;
		friendRequestSentFeedback.text = resultValue ? "Friend Request Sent!" : "Error";
		friendRequestSentFeedback.gameObject.SetActive(true);
		if (!resultValue)
			friendRequestSentFeedback.GetComponent<Animator>().Play("Error");
		Invoke(nameof(TurnOffFeedback), 1);
	}

	private void TurnOffFeedback()
		=> friendRequestSentFeedback.gameObject.SetActive(false);
}