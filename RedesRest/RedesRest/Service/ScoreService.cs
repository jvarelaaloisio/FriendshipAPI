using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using RedesRest.DB;
using RedesRestModel.Models.DTO;

namespace RedesRest.Service
{
	public class ScoreService
	{
		private const string SP_GET_TOP_SCORES = "GetTopScores";
		private const string SP_PARAMETER_GET_TOP_SCORES_QUANTITY = "quantity";

		public static PlayerScoreDTO[] GetTopScores(int quantity)
		{
			string query = SP_GET_TOP_SCORES;

			var sqlCommand
				= new SqlCommand(query) {CommandType = CommandType.StoredProcedure};
			sqlCommand.Parameters.AddWithValue(SP_PARAMETER_GET_TOP_SCORES_QUANTITY, quantity);
			var scoresData
				= Base.ExecuteQuery(sqlCommand);
			return JsonConvert.DeserializeObject<PlayerScoreDTO[]>(scoresData);
		}
	}
}