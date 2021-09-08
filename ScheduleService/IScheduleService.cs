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

		List<Game> GetSchedule(
			string team,
			string leagueCode,
			int season);

		List<string> GetLeagues();

		Game GetGame(
			string team,
			string leagueCode,
			int season,
			int round);
	}
}
