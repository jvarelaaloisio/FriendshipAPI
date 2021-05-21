using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
	[SerializeField] private int loginScreen;
	private void Awake()
	{
		SceneManager.LoadSceneAsync(loginScreen, LoadSceneMode.Additive);
	}
}
