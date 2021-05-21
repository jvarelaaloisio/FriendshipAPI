using Newtonsoft.Json;
using RedesRestModel.Models.DTO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TopScoresController : MonoBehaviour
{
	private const string GET_TOP_SCORES
		= "/TopScores?quantity=";

	[SerializeField] private TMP_Text resultText;
	[SerializeField] private int scoreQty = 10;

	private void Start()
	{
		SetScores();
	}

	[ContextMenu("Set Scores")]
	public void SetScores()
	{
		StartCoroutine(
			WebServiceHelper.Get(
				UrlManager.FriendshipServiceUrl + GET_TOP_SCORES + scoreQty,
				response
					=> SetScoreValues(
						JsonConvert.DeserializeObject<PlayerScoreDTO[]>(response)
					)
			)
		);
	}

	private void SetScoreValues(PlayerScoreDTO[] scores)
	{
		string text = "Top Scores:\n";
		for (var i = 0; i < scores.Length; i++)
		{
			var score = scores[i];
			text += $"{i + 1}: {score.Name} | {score.ScoreValue}\n";
		}

		resultText.text = text;
	}
}