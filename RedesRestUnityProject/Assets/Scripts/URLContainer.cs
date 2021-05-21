using UnityEngine;

[CreateAssetMenu(menuName = "Redes/Url Container", fileName = "URLContainer", order = 0)]
public class URLContainer : ScriptableObject
{
	[SerializeField] private string url;

	public string URL => url;
}