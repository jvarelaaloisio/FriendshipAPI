using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using RedesRest.DB;
using RedesRestModel.Models.DTO;

namespace RedesRest.Service
{
	public class UserService
	{
		private const string SP_PARAMETER_NAME = "name";
		private const string SP_PARAMETER_PASSWORD = "password";
		private const string SP_PARAMETER_USER_ID = "UserId";
		private const string SP_GENERATE_USER = "GenerateUser";
		private const string SP_GET_USER_ID = "GetUserId";
		private const string SP_GET_FRIENDS = "GetFriends";
		private const string SP_REMOVE_FRIEND = "RemoveFriend";
		private const string SP_SEND_FRIEND_REQUEST = "SendFriendRequest";
		private const string SP_GET_FRIEND_REQUESTS = "GetFriendRequests";
		private const string SP_ACCEPT_FRIEND_REQUEST = "AcceptFriendRequest";
		private const string SP_DECLINE_FRIEND_REQUEST = "DeclineFriendRequest";
		private const string SP_PARAMETER_FRIEND_ID = "FriendId";

		public static bool IsValidUser(string username, string password)
		{
			var command = new SqlCommand("ValidateUserPassword") {CommandType = CommandType.StoredProcedure};
			command.Parameters.AddWithValue(SP_PARAMETER_NAME, username);
			command.Parameters.AddWithValue(SP_PARAMETER_PASSWORD, password);
			var resultData = Base.ExecuteScalar(command);
			return resultData > 0;
		}

		public static bool InsertUser(string username, string password)
		{
			SqlCommand command = new SqlCommand(SP_GENERATE_USER);
			command.Parameters.AddWithValue(SP_PARAMETER_NAME, username);
			command.Parameters.AddWithValue(SP_PARAMETER_PASSWORD, password);
			command.CommandType = CommandType.StoredProcedure;
			var result = Base.ExecuteScalar(command);
			return result > 0;
		}

		public static int GetUserId(string username)
		{
			var command = new SqlCommand(SP_GET_USER_ID);
			command.Parameters.AddWithValue(SP_PARAMETER_NAME, username);
			var resultParam = new SqlParameter("userId", SqlDbType.Int)
			{
				Direction = ParameterDirection.Output
			};
			command.Parameters.Add(resultParam);
			command.CommandType = CommandType.StoredProcedure;
			var result = Base.ExecuteScalar(command);
			return (int)resultParam.Value;
		}

		public static FriendDTO[] GetFriends(int userId)
		{
			var command = new SqlCommand(SP_GET_FRIENDS);
			command.Parameters.AddWithValue(SP_PARAMETER_USER_ID, userId);
			command.CommandType = CommandType.StoredProcedure;
			var result = Base.ExecuteQuery(command);
			var friends = JsonConvert.DeserializeObject<FriendDTO[]>(result);
			foreach (var friend in friends)
			{
				Console.WriteLine(friend.UserId + " -> " + friend.Name);
			}
			return friends;
		}

		public static long SendFriendRequest(int userId, string friendName)
		{
			var command = new SqlCommand(SP_SEND_FRIEND_REQUEST);
			command.Parameters.AddWithValue(SP_PARAMETER_USER_ID, userId);
			command.Parameters.AddWithValue("FriendName", friendName);
			var resultParam = new SqlParameter("NewRequestId", SqlDbType.BigInt)
			{
				Direction = ParameterDirection.Output
			};
			command.Parameters.Add(resultParam);
			command.CommandType = CommandType.StoredProcedure;
			Base.ExecuteScalar(command);
			return (long)resultParam.Value;
		}

		public static FriendRequestDTO[] GetFriendRequests(int userId)
		{
			var command = new SqlCommand(SP_GET_FRIEND_REQUESTS);
			command.Parameters.AddWithValue(SP_PARAMETER_USER_ID, userId);
			command.CommandType = CommandType.StoredProcedure;
			var result = Base.ExecuteQuery(command);
			var friends = JsonConvert.DeserializeObject<FriendRequestDTO[]>(result);
			return friends;
		}

		public static bool AcceptFriendRequest(long requestId)
		{
			var command = new SqlCommand(SP_ACCEPT_FRIEND_REQUEST);
			command.Parameters.AddWithValue("RequestId", requestId);
			command.CommandType = CommandType.StoredProcedure;
			var result = Base.ExecuteScalar(command);
			return result > 0;
		}

		public static bool DeclineFriendRequest(long requestId)
		{
			var command = new SqlCommand(SP_DECLINE_FRIEND_REQUEST);
			command.Parameters.AddWithValue("RequestId", requestId);
			command.CommandType = CommandType.StoredProcedure;
			var result = Base.ExecuteScalar(command);
			return result > 0;
		}

		public static bool RemoveFriend(int userId, int friendId)
		{
			var command = new SqlCommand(SP_REMOVE_FRIEND);
			command.Parameters.AddWithValue(SP_PARAMETER_USER_ID, userId);
			command.Parameters.AddWithValue(SP_PARAMETER_FRIEND_ID, friendId);
			command.CommandType = CommandType.StoredProcedure;
			var result = Base.ExecuteScalar(command);
			return result > 0;
		}
	}
}