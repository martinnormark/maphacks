// No usings needed

namespace MapHacks.Models
{
	public class AppViewModel
	{
		public UserDetailsViewModel UserDetails { get; set; }

		public bool IsAuthenticated
		{
			get { return this.UserDetails != null; }
		}
	}
}