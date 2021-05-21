using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Friendship : MonoBehaviour
{
	public int id;
	public Action<int> RemoveFriend;
	[SerializeField] private TMP_Text friendText;
	private RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}

	public void SetValues(
		int id,
		string friendName,
		Action<int> removeFriend)
	{
		this.id = id;
		friendText.text = friendName;
		RemoveFriend = removeFriend;
	}

	public void SetPosition(Vector2 position)
		=> _rectTransform.anchoredPosition = position;

	public void RemoveThisFriend()
		=> RemoveFriend(id);
}