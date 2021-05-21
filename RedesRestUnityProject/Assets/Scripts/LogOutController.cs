using UnityEngine;
using UnityEngine.SceneManagement;

public class LogOutController : MonoBehaviour
{
	[SerializeField] private int loginScreen;
	
	public void LogOut()
	{
		SceneManager.LoadSceneAsync(loginScreen, LoadSceneMode.Additive);
		SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
	}
}
