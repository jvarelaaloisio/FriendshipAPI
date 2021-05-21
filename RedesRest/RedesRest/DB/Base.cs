using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace RedesRest.DB
{
	public static class Base
	{
		private static readonly SqlConnection SqlConnection;

		static Base()
		{
			SqlConnection = new SqlConnection(
				ConfigurationManager
					.ConnectionStrings["RedesDb"]
					.ConnectionString);
		}

		public static string ExecuteQuery(SqlCommand sqlCommand)
		{
			sqlCommand.Connection = SqlConnection;
			try
			{
				SqlConnection.Open();
				var dataReader = sqlCommand.ExecuteReader();
				var dataTable = new DataTable();
				dataTable.Load(dataReader);
				SqlConnection.Close();
				return JsonConvert.SerializeObject(dataTable);
			}
			finally
			{
				SqlConnection.Close();
			}
		}
		
		public static int ExecuteScalar(SqlCommand sqlCommand)
		{
			sqlCommand.Connection = SqlConnection;
			try
			{
				SqlConnection.Open();
				var result = sqlCommand.ExecuteScalar();
				
				SqlConnection.Close();
				return Convert.ToInt32(result);
			}
			finally
			{
				SqlConnection.Close();
			}
		}
	}
}