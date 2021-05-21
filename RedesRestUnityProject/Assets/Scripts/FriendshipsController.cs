using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RedesRestModel.Models.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FriendshipsController : MonoBehaviour
{
	private const string WS_GET_FRIENDS = "/GetFriends?userId=";
	private const string WS_REMOVE_FRIEND = "/RemoveFriend";
	private const string FIELD_USER_ID = "userId";
	private const string FIELD_FRIEND_ID = "friendId";

	[SerializeField] private TMP_Text friendsData;

	[SerializeField] private Friendship friendshipPrefab;
	[SerializeField] private Transform friendsContainer;

	[ContextMenuItem("Set request height", "SetRequestHeight")]
	[SerializeField] private float requestHeight;

	[SerializeField] private int maxRequestsQty;
	private Dictionary<long, Friendship> _friends;
	[SerializeField] private UnityEvent onRemovedFriend;
	private Dictionary<string, string> _fieldsForRemoveWs;

	private void Awake()
	{
		_fieldsForRemoveWs = new Dictionary<string, string>
		{
			{FIELD_USER_ID, UserDataService.UserId.ToString()},
			{FIELD_FRIEND_ID, string.Empty}
		};
	}

	private void Start()
	{
		LoadFriends();
	}

	[ContextMenu("LoadFriends")]
	public void LoadFriends()
	{
		StartCoroutine(
			WebServiceHelper.Get(
				UrlManager.FriendshipServiceUrl + WS_GET_FRIENDS + UserDataService.UserId,
				response => InstantiateFriendShips(
					JsonConvert.DeserializeObject<FriendDTO[]>(response)
				)
			)
		);
	}

	private void SetFriendNames(FriendDTO[] friends)
	{
		var data
			= friends.Aggregate(
				string.Empty,
				(current, friend) => current + (friend.Name + '\n'));

		friendsData.text = data;
	}

	private void InstantiateFriendShips(FriendDTO[] friendDtos)
	{
		_friends = new Dictionary<long, Friendship>();
		int iterations = Mathf.Clamp(friendDtos.Length, 0, maxRequestsQty);
		for (var i = 0; i < iterations; i++)
		{
			var friendDto = friendDtos[i];
			var newChild = Instantiate(friendshipPrefab, friendsContainer);
			_friends.Add(friendDto.UserId, newChild);
			newChild.SetValues(
				friendDto.UserId,
				friendDto.Name,
				TryRemoveFriend
			);
		}

		PlaceFriendships(_friends.Values);
	}

	private void TryRemoveFriend(int id)
	{
		_fieldsForRemoveWs[FIELD_FRIEND_ID] = id.ToString();
		StartCoroutine(
			WebServiceHelper.Post(
				UrlManager.FriendshipServiceUrl + WS_REMOVE_FRIEND,
				_fieldsForRemoveWs,
				response => FriendRemovedHandler(id, bool.Parse(response))
			)
		);
	}

	private void FriendRemovedHandler(int id, bool result)
	{
		Debug.Log($"removed friend {id} -> result: {result}");
		Destroy(_friends[id].gameObject);
		_friends.Remove(id);
		PlaceFriendships(_friends.Values);
	}

	private void PlaceFriendships(IEnumerable<Friendship> requests)
	{
		int index = 0;
		foreach (var request in requests)
		{
			request.SetPosition(Vector2.up * (requestHeight * (index + 1.0f / 2)));
			index++;
		}
	}
	
	private void SetRequestHeight()
	{
		if (!friendshipPrefab)
			return;
		if (!friendshipPrefab.TryGetComponent(out RectTransform requestRectTransform))
			return;
		requestHeight = requestRectTransform.rect.height;
	}
}