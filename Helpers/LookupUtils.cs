namespace TipIt.Helpers
{
	public static class LookupUtils
	{
		public static int LookupEasyPoints(
			string teamCode,
			string leagueCode)
		{
			if (leagueCode == "NRL")
			{
				if (teamCode == "TITN")
					return 19;
				if (teamCode == "DRAG")
					return 19;
				if (teamCode == "NQLD")
					return 15;
				if (teamCode == "NZW")
					return 13;
				if (teamCode == "BULL")
					return 9;
				if (teamCode == "NEWC")
					return 19;
				if (teamCode == "PENR")
					return 6;
				if (teamCode == "WTIG")
					return 16;
				if (teamCode == "BRIS")
					return 5;
				if (teamCode == "SHRK")
					return 10;
				if (teamCode == "MANL")
					return 14;
				if (teamCode == "PARR")
					return 12;
				if (teamCode == "CANB")
					return 10;
				if (teamCode == "SSYD")
					return 11;
				if (teamCode == "MELB")
					return 7;
				if (teamCode == "SYDR")
					return 7;
				if (teamCode == "DOLP")
					return 11;
				return 0;
			}
			if (teamCode == "CARL")
				return 16;
			if (teamCode == "ESS")
				return 21;
			if (teamCode == "RICH")
				return 24;
			if (teamCode == "GEEL")
				return 7;
			if (teamCode == "BL")
				return 5;
			if (teamCode == "COLL")
				return 12;
			if (teamCode == "WCE")
				return 24;
			if (teamCode == "GWS")
				return 10;
			if (teamCode == "WB")
				return 10;
			if (teamCode == "HAW")
				return 8;
			if (teamCode == "PORT")
				return 18;
			if (teamCode == "ADEL")
				return 9;
			if (teamCode == "NMFC")
				return 22;
			if (teamCode == "FRE")
				return 11;
			if (teamCode == "STK")
				return 14;
			if (teamCode == "SYD")
				return 8;
			if (teamCode == "MELB")
				return 20;
			if (teamCode == "GCFC")
				return 6;
			return 0;
		}

		public static int LookupEasyPoints_2025(
			string teamCode,
			string leagueCode)
		{
			if (leagueCode == "NRL")
			{
				if (teamCode == "TITN")
					return 17;
				if (teamCode == "DRAG")
					return 18;
				if (teamCode == "NQLD")
					return 11;
				if (teamCode == "NZW")
					return 15;
				if (teamCode == "BULL")
					return 16;
				if (teamCode == "NEWC")
					return 16;
				if (teamCode == "PENR")
					return 7;
				if (teamCode == "WTIG")
					return 14;
				if (teamCode == "BRIS")
					return 8;
				if (teamCode == "SHRK")
					return 9;
				if (teamCode == "MANL")
					return 10;
				if (teamCode == "PARR")
					return 13;
				if (teamCode == "CANB")
					return 14;
				if (teamCode == "SSYD")
					return 11;
				if (teamCode == "MELB")
					return 6;
				if (teamCode == "SYDR")
					return 10;
				if (teamCode == "DOLP")
					return 17;
				return 0;
			}
			if (teamCode == "CARL")
				return 9;
			if (teamCode == "ESS")
				return 16;
			if (teamCode == "RICH")
				return 24;
			if (teamCode == "GEEL")
				return 10;
			if (teamCode == "BL")
				return 7;
			if (teamCode == "COLL")
				return 7;
			if (teamCode == "WCE")
				return 20;
			if (teamCode == "GWS")
				return 9;
			if (teamCode == "WB")
				return 11;
			if (teamCode == "HAW")
				return 7;
			if (teamCode == "PORT")
				return 12;
			if (teamCode == "ADEL")
				return 14;
			if (teamCode == "NMFC")
				return 22;
			if (teamCode == "FRE")
				return 11;
			if (teamCode == "STK")
				return 16;
			if (teamCode == "SYD")
				return 8;
			if (teamCode == "MELB")
				return 14;
			if (teamCode == "GCFC")
				return 15;
			return 0;
		}

		public static int LookupEasyPoints_2021(
			string teamCode,
			string leagueCode)
		{
			if (leagueCode == "NRL")
			{
				if (teamCode == "TITN")
					return 40;
				if (teamCode == "DRAG")
					return 18;
				if (teamCode == "NQLD")
					return 14;
				if (teamCode == "NZW")
					return 19;
				if (teamCode == "BULL")
					return 20;
				if (teamCode == "NEWC")
					return 17;
				if (teamCode == "PENR")
					return 15;
				if (teamCode == "WTIG")
					return 19;
				if (teamCode == "BRIS")
					return 15;
				if (teamCode == "SHRK")
					return 15;
				if (teamCode == "MANL")
					return 12;
				if (teamCode == "PARR")
					return 11;
				if (teamCode == "CANB")
					return 9;
				if (teamCode == "SSYD")
					return 11;
				if (teamCode == "MELB")
					return 8;
				if (teamCode == "SYDR")
					return 7;
				return 0;
			}
			if (teamCode == "CARL")
				return 17;
			if (teamCode == "ESS")
				return 15;
			if (teamCode == "RICH")
				return 7;
			if (teamCode == "GEEL")
				return 11;
			if (teamCode == "BL")
				return 9;
			if (teamCode == "COLL")
				return 10;
			if (teamCode == "WCE")
				return 8;
			if (teamCode == "GWS")
				return 9;
			if (teamCode == "WB")
				return 11;
			if (teamCode == "HAW")
				return 13;
			if (teamCode == "PORT")
				return 16;
			if (teamCode == "ADEL")
				return 19;
			if (teamCode == "NMFC")
				return 15;
			if (teamCode == "FRE")
				return 19;
			if (teamCode == "STK")
				return 16;
			if (teamCode == "SYD")
				return 20;
			if (teamCode == "MELB")
				return 14;
			if (teamCode == "GCFC")
				return 40;
			return 0;
		}
	}
}
