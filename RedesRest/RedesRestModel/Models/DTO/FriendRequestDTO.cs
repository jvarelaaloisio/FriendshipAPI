namespace RedesRestModel.Models.DTO
{
	public class FriendRequestDTO
	{
		public long RequestId { get; set; }
		public int SenderId { get; set; }
		public string SenderName { get; set; }
		public int Receiver { get; set; }
		public string ReceiverName { get; set; }
		public short State { get; set; }
		public string StateName { get; set; }
	}
}