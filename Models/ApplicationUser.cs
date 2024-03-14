using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Fashion.Models
{
	public class ApplicationUser : IdentityUser
	{
        [MaxLength(50)]
        public string FirstName { get; set; }
		[MaxLength(50)]
		public string LastName { get; set; }
    }
}
