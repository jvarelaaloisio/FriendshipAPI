using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FriendRequest : MonoBehaviour
{
	public long id;
	public Action<long> AcceptRequest;
	public Action<long> DeclineRequest;
	[SerializeField] private TMP_Text senderText;
	private RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}

	public void SetValues(
		long id,
		string senderName,
		Action<long> acceptRequest,
		Action<long> declineRequest)
	{
		this.id = id;
		senderText.text = senderName;
		AcceptRequest = acceptRequest;
		DeclineRequest = declineRequest;
	}

	public void SetPosition(Vector2 position)
		=> _rectTransform.anchoredPosition = position;

	public void AcceptThisRequest()
		=> AcceptRequest(id);

	public void DeclineThisRequest()
		=> DeclineRequest(id);
}