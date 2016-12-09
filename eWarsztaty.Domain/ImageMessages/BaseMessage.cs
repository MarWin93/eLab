using System;

namespace Rlc.Monitor.Messages
{
	public class BaseMessage
	{
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public string TeacherConnectionId { get; set; }
        public DateTime TimeStamp { get; set; }
	}
}