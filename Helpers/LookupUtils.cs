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
                    return 12;
                if (teamCode == "DRAG")
                    return 25;
                if (teamCode == "NQLD")
                    return 17;
                if (teamCode == "NZW")
                    return 15;
                if (teamCode == "BULL")
                    return 12;
                if (teamCode == "NEWC")
                    return 12;
                if (teamCode == "PENR")
                    return 6;
                if (teamCode == "WTIG")
                    return 25;
                if (teamCode == "BRIS")
                    return 15;
                if (teamCode == "SHRK")
                    return 11;
                if (teamCode == "MANL")
                    return 8;
                if (teamCode == "PARR")
                    return 10;
                if (teamCode == "CANB")
                    return 12;
                if (teamCode == "SSYD")
                    return 8;
                if (teamCode == "MELB")
                    return 6;
                if (teamCode == "SYDR")
                    return 6;
                return 0;
            }
            if (teamCode == "CARL")
                return 17;
            if (teamCode == "ESS")
                return 13;
            if (teamCode == "RICH")
                return 10;
            if (teamCode == "GEEL")
                return 9;
            if (teamCode == "BL")
                return 8;
            if (teamCode == "COLL")
                return 19;
            if (teamCode == "WCE")
                return 12;
            if (teamCode == "GWS")
                return 12;
            if (teamCode == "WB")
                return 8;
            if (teamCode == "HAW")
                return 20;
            if (teamCode == "PORT")
                return 16;
            if (teamCode == "ADEL")
                return 21;
            if (teamCode == "NMFC")
                return 25;
            if (teamCode == "FRE")
                return 16;
            if (teamCode == "STK")
                return 12;
            if (teamCode == "SYD")
                return 10;
            if (teamCode == "MELB")
                return 7;
            if (teamCode == "GCFC")
                return 30;
            return 0;
        }

        public static int LookupEasyPoints2021(
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
