﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public interface IClubService
	{
		Task<IEnumerable<Club>> GetUserClubs(string userId);
	}
}
