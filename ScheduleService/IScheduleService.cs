using System;
using System.Collections.Generic;
using TipIt.Models;

namespace ScheduleService
{
	public interface IScheduleService
	{
		bool HasSeason(
			string leagueCode,
			int season);

		int Rounds(
			string leagueCode,
			int season);

		string GetRound(
			int round,
			string leagueCode,
			int season);

		List<Game> GetRoundData(
			int round,
			string leagueCode,
			int season);

		Game GetGame(
			string team,
			DateTime whichDay,
			string leagueCode,
			int season);

		Game GetGame(
			string team,
			string leagueCode,
			int season,
			int round); 
		
		List<Game> GetSchedule(
			string team,
			string leagueCode,
			int season);

		List<string> GetLeagues();

		bool IsSeason(
			string leagueCode,
			int season,
			DateTime theDate);

		DateTime SeasonStart(
			string leagueCode,
			int season);

		DateTime SeasonEnd(
			string leagueCode,
			int season);

		int RoundFor(
			string leagueCode,
			int season,
			DateTime theDate);
	}
}
