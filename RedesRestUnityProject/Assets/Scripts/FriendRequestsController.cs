using System.Collections.Generic;
using Newtonsoft.Json;
using RedesRestModel.Models.DTO;
using UnityEngine;
using UnityEngine.Events;

public class FriendRequestsController : MonoBehaviour
{
	private const string WS_GET_FRIEND_REQUESTS = "/GetFriendRequests?userId=";
	private const string WS_ACCEPT_FRIEND_REQUEST = "/AcceptFriendRequest?requestId=";
	private const string WS_DECLINE_FRIEND_REQUEST = "/DeclineFriendRequest?requestId=";
	[SerializeField] private FriendRequest friendRequestPrefab;
	[SerializeField] private Transform requestContainer;

	[ContextMenuItem("Set request height", "SetRequestHeight")]
	[SerializeField] private float requestHeight;

	[SerializeField] private int maxRequestsQty;
	private readonly Dictionary<long, FriendRequest> _requests = new Dictionary<long, FriendRequest>();
	[SerializeField] private UnityEvent onAcceptedRequest;

	private void Start()
	{
		StartCoroutine(
			WebServiceHelper.Get(
				UrlManager.FriendshipServiceUrl + WS_GET_FRIEND_REQUESTS + UserDataService.UserId,
				response => InstantiateFriendRequests(
					JsonConvert.DeserializeObject<FriendRequestDTO[]>(response)
				)
			)
		);
	}

	private void InstantiateFriendRequests(FriendRequestDTO[] requestsDtos)
	{
		int iterations = Mathf.Clamp(requestsDtos.Length, 0, maxRequestsQty);
		for (var i = 0; i < iterations; i++)
		{
			var requestDto = requestsDtos[i];
			var newChild = Instantiate(friendRequestPrefab, requestContainer);
			_requests.Add(requestDto.RequestId, newChild);

			newChild.SetValues(
				requestDto.RequestId,
				requestDto.SenderName,
				(id) => StartCoroutine(
					WebServiceHelper.Get(
						UrlManager.FriendshipServiceUrl + WS_ACCEPT_FRIEND_REQUEST + id,
						s => RequestAcceptedHandler(id, s))
				),
				(id) => StartCoroutine(
					WebServiceHelper.Get(
						UrlManager.FriendshipServiceUrl + WS_DECLINE_FRIEND_REQUEST + id,
						(s) => RequestDeclinedHandler(id, s)
					)
				)
			);
		}

		PlaceRequests(_requests.Values);
	}

	private void RequestAcceptedHandler(long id, string result)
	{
		Debug.Log($"accepted request {id} -> result: {result}");
		Destroy(_requests[id].gameObject);
		_requests.Remove(id);
		PlaceRequests(_requests.Values);
		onAcceptedRequest.Invoke();
	}

	private void RequestDeclinedHandler(long id, string result)
	{
		Debug.Log($"declined request {id} -> result: {result}");
		Destroy(_requests[id].gameObject);
		_requests.Remove(id);
		PlaceRequests(_requests.Values);
	}

	private void PlaceRequests(IEnumerable<FriendRequest> requests)
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
		if (!friendRequestPrefab)
			return;
		if (!friendRequestPrefab.TryGetComponent(out RectTransform requestRectTransform))
			return;
		requestHeight = requestRectTransform.rect.height;
	}
}