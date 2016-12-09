using System;

namespace Rlc.Monitor.Messages
{
	[Serializable]
	public class ImageNoChangeMessage : BaseMessage
	{
		public ImageNoChangeMessage() {}
        public bool IsThumbnail { get; set; }
    }
}