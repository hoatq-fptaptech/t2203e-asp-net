using System;
using Microsoft.AspNetCore.Authorization;
namespace T2203E_API.Requirements
{
	public class YearOldRequirement : IAuthorizationRequirement
	{
		
		public YearOldRequirement(int min,int max)
		{
			MinYear = min;
			MaxYear = max;
		}

        public int MinYear { get; set; }
        public int MaxYear { get; set; }
    }
}

