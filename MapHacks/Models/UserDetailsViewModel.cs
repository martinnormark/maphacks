// No usings needed

namespace MapHacks.Models
{
	public class UserDetailsViewModel
	{
		public long Id { get; set; }

		public string UserName { get; set; }

		public string ProfilePictureUrl { get; set; }

		public string AccessToken { get; set; }

		public string AccessTokenSecret { get; set; }
	}
}