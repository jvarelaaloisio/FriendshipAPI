using System.Web.Mvc;
using RedesRest.Service;

namespace RedesRest.Controllers
{
	public class RedesController : Controller
	{
		[HttpGet]
		public JsonResult TopScores(int quantity)
		{
			var playerScores = ScoreService.GetTopScores(quantity);
			return Json(playerScores, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetUserId(string username)
		{
			int id = UserService.GetUserId(username);
			return Json(id, JsonRequestBehavior.AllowGet);
		}
		
		[HttpPost]
		public JsonResult ValidateUser(string username, string password)
		{
			var result = !string.IsNullOrEmpty(username + password)
			             && UserService.IsValidUser(username, password);
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GenerateNewUser(string username, string password)
		{
			var result = !string.IsNullOrEmpty(username + password);
			if (result) result = UserService.InsertUser(username, password);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
		
		[HttpGet]
		public JsonResult GetFriends(int userId)
		{
			var friends = UserService.GetFriends(userId);
			return Json(friends, JsonRequestBehavior.AllowGet);
		}
		
		[HttpPost]
		public JsonResult SendFriendRequest(int userId, string friendName)
		{
			long result = string.IsNullOrEmpty(friendName) ? 0 : 1;
			if (result > 0)
				result = UserService.SendFriendRequest(userId, friendName);
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetFriendRequests(int userId)
		{
			var friendRequests = UserService.GetFriendRequests(userId);
			return Json(friendRequests, JsonRequestBehavior.AllowGet);
		}
		
		[HttpGet]
		public JsonResult AcceptFriendRequest(long requestId)
		{
			var result = UserService.AcceptFriendRequest(requestId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
		
		[HttpGet]
		public JsonResult DeclineFriendRequest(long requestId)
		{
			var result = UserService.DeclineFriendRequest(requestId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
		
		[HttpPost]
		public JsonResult RemoveFriend(int userId, int friendId)
		{
			var result = UserService.RemoveFriend(userId, friendId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}