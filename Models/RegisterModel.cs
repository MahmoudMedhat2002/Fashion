using System.ComponentModel.DataAnnotations;

namespace Fashion.Models
{
	public class RegisterModel
	{
        [MaxLength(50)]
        public string FirstName { get; set; }
		[MaxLength(50)]
		public string LastName { get; set; }
		[MaxLength(50)]
		public string UserName { get; set; }
		[MaxLength(50)]
		public string Email { get; set; }
		[MaxLength(50)]
		public string Password { get; set; }
    }
}
