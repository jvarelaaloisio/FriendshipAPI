using UnityEngine;

public class UrlManager : MonoBehaviour
{
	[SerializeField] private URLContainer friendshipsURLContainer;
	private static UrlManager _instance;

	private void Awake()
	{
		if (!_instance)
			_instance = this;
		else
			Destroy(this);
	}

	public static string FriendshipServiceUrl => _instance.friendshipsURLContainer.URL;
}