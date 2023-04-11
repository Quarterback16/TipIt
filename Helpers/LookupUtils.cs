﻿namespace TipIt.Helpers
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
                    return 17;
                if (teamCode == "DRAG")
                    return 26;
                if (teamCode == "NQLD")
                    return 10;
                if (teamCode == "NZW")
                    return 18;
                if (teamCode == "BULL")
                    return 15;
                if (teamCode == "NEWC")
                    return 19;
                if (teamCode == "PENR")
                    return 6;
                if (teamCode == "WTIG")
                    return 17;
                if (teamCode == "BRIS")
                    return 11;
                if (teamCode == "SHRK")
                    return 10;
                if (teamCode == "MANL")
                    return 15;
                if (teamCode == "PARR")
                    return 11;
                if (teamCode == "CANB")
                    return 13;
                if (teamCode == "SSYD")
                    return 9;
                if (teamCode == "MELB")
                    return 7;
                if (teamCode == "SYDR")
                    return 7;
                return 0;
            }
            if (teamCode == "CARL")
                return 10;
            if (teamCode == "ESS")
                return 17;
            if (teamCode == "RICH")
                return 9;
            if (teamCode == "GEEL")
                return 6;
            if (teamCode == "BL")
                return 8;
            if (teamCode == "COLL")
                return 11;
            if (teamCode == "WCE")
                return 18;
            if (teamCode == "GWS")
                return 18;
            if (teamCode == "WB")
                return 12;
            if (teamCode == "HAW")
                return 20;
            if (teamCode == "PORT")
                return 11;
            if (teamCode == "ADEL")
                return 17;
            if (teamCode == "NMFC")
                return 22;
            if (teamCode == "FRE")
                return 13;
            if (teamCode == "STK")
                return 16;
            if (teamCode == "SYD")
                return 8;
            if (teamCode == "MELB")
                return 7;
            if (teamCode == "GCFC")
                return 16;
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
