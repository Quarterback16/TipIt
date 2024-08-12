using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TipIt.Events;
using TipIt.Implementations;
using TipIt.Models;

namespace ScheduleService
{
	public class ScheduleMaster : IScheduleService
	{
		public string SchedulePath { get; set; }

		public Dictionary<string, Dictionary<int, List<Game>>> LeagueSchedules
		{
			get;
			set;
		}

		public ScheduleMaster()
		{
			SchedulePath = "L:\\apps\\Schedules";
			LeagueSchedules = new Dictionary<string, Dictionary<int, List<Game>>>();
			LoadSchedules();
		}

		private void LoadSchedules()
		{
			// for each json schedule file found, load em ALL in
			var path = SchedulePath;
			DirectoryInfo dir = new DirectoryInfo(path);
			foreach (FileInfo fi in dir.GetFiles())
			{
				if (fi.Name.Length != 22)
					continue;
				if (fi.Extension.Equals(".json")
					&& fi.Name.Substring(3,10) == "-schedule-")
				{
					var jsonFile = fi.Name;
					LoadSchedule(jsonFile);
				}
			}
		}

		private void LoadSchedule(
			string fileName)
		{
			var eventStore = new ScheduleEventStore(fileName);
			var fileSeason = SeasonFrom(fileName);
			var events = (List<ScheduleEvent>)eventStore
				.Get<ScheduleEvent>("schedule");
			foreach (var e in events)
			{
				var theGame = new Game(e);
				if (!LeagueSchedules
					.ContainsKey(
						LeagueKey(
							theGame.League, 
							fileSeason)))
				{
					LeagueSchedules.Add(
						LeagueKey(theGame.League, fileSeason),
						new Dictionary<int, List<Game>>());
				}
				var roundDict = LeagueSchedules[LeagueKey(theGame.League, fileSeason)];
				if (!roundDict.ContainsKey(theGame.Round))
					roundDict.Add(theGame.Round, new List<Game>());
				var gameList = roundDict[theGame.Round];
				gameList.Add(theGame);
				roundDict[theGame.Round] = gameList;
				LeagueSchedules[LeagueKey(theGame.League, fileSeason)] = roundDict;
			}
		}

		private static int SeasonFrom(
			string fileName)
		{
			string[] parts = fileName.Replace(".json","").Split('-');
			return int.Parse(parts[2].ToString());
		}

		public Game GetGame(
			string team, 
			DateTime whichDay,
			string leagueCode, 
			int season)
		{
			var result = new Game();
			var key = LeagueKey(leagueCode, season);
			if (LeagueSchedules.ContainsKey(key))
			{
				var rounds = LeagueSchedules[key];
				foreach (var round in rounds)
				{
					var games = round.Value;
					foreach (var game in games)
					{
						if (game.GameDate.Date.Equals(whichDay.Date)
							&& (game.AwayTeam == team || game.HomeTeam == team))
						{
							result = game;
							break;
						}
					}
					if (result.Round > 0)
						break;
				}
			}
			return result;
		}

		public Game GetGame(
			string team,
			string leagueCode,
			int season,
			int round)
		{
			var key = LeagueKey(
				leagueCode,
				season);
			var theRound = LeagueSchedules[key]
				.FirstOrDefault(kpv => kpv.Key == round);
			return theRound.Value
				.FirstOrDefault(
					r => r.HomeTeam.Equals(team) || r.AwayTeam.Equals(team));
		}

		public List<Game> GetSchedule(
			string team,
			string leagueCode,
			int season)
		{
			var result = new List<Game>();
			var key = LeagueKey(leagueCode, season);
			if (LeagueSchedules.ContainsKey(key))
			{
				var rounds = LeagueSchedules[key];
				foreach (var round in rounds)
				{
					var games = round.Value;
					foreach (var game in games)
					{
						if (game.HomeTeam.Equals(team) 
							|| game.AwayTeam.Equals(team))
						{
							result.Add(game);
						}
					}
				}
			}
			return result;
		}

		public List<string> GetLeagues()
		{
			var leagues = new List<string>();
			foreach (var item in LeagueSchedules)
				leagues.Add(item.Key);
			return leagues;
		}

		private string LeagueKey(
			string league, 
			int season)
				=> $"{league}:{season}";
		
		public bool HasSeason(
			string leagueCode, 
			int season)
		{
			var key = LeagueKey(leagueCode, season);
			if (LeagueSchedules.ContainsKey(key))
				return true;
			return false;
		}

		public int Rounds(
			string leagueCode,
			int season)
		{
			var rounds = 0;
			var key = LeagueKey(leagueCode, season);
			if (LeagueSchedules.ContainsKey(key))
			{
				rounds = LeagueSchedules[key].Count;
			}
			return rounds;
		}

		public int Games(
			string leagueCode,
			int season)
		{
			var games = 0;
			var key = LeagueKey(
				leagueCode, 
				season);
			if (LeagueSchedules.ContainsKey(key))
			{
				var rounds = LeagueSchedules[key];
				foreach (var round in rounds)
					games += round.Value.Count;
			}
			return games;
		}

		public string GetRound(
			int round,
			string leagueCode,
			int season)
		{
			var sb = new StringBuilder();
			var key = LeagueKey(leagueCode, season);
			if (LeagueSchedules.ContainsKey(key))
			{
				var sched = LeagueSchedules[key];
				var games = sched[round];
				foreach (var game in games)
				{
					sb.AppendLine(game.ToString());
				}
			}
			return sb.ToString();
		}

		public List<Game> GetRoundData(
			int round,
			string leagueCode,
			int season)
		{
			var result = new List<Game>();
			var key = LeagueKey(leagueCode, season);
			if (LeagueSchedules.ContainsKey(key))
			{
				var sched = LeagueSchedules[key];
				var games = sched[round];
				result.AddRange(games);
			}
			return result;
		}

		public bool IsSeason(
			string leagueCode,
			int season,
			DateTime theDate)
		{
			var inSeason = false;
			var key = LeagueKey(leagueCode, season);
			var seasonStart = DateTime.MaxValue;
			var seasonEnd = DateTime.MinValue;
			if (LeagueSchedules.ContainsKey(key))
			{
				var rounds = LeagueSchedules[key];
				foreach (var round in rounds)
				{
					foreach (var game in round.Value)
					{
						if (game.GameDate < seasonStart)
							seasonStart = game.GameDate;
						if (game.GameDate > seasonEnd)
							seasonEnd = game.GameDate;
					}
				}
				return (theDate >= seasonStart && theDate <= seasonEnd);
			}
			return inSeason;
		}

		public DateTime SeasonStart(
			string leagueCode,
			int season)
		{
			var key = LeagueKey(leagueCode, season);
			var seasonStart = DateTime.MaxValue;
			if (LeagueSchedules.ContainsKey(key))
			{
				var rounds = LeagueSchedules[key];
				foreach (var round in rounds)
				{
					foreach (var game in round.Value)
					{
						if (game.GameDate < seasonStart)
							seasonStart = game.GameDate;
					}
				}
			}
			return seasonStart;
		}

		public DateTime SeasonEnd(
			string leagueCode,
			int season)
		{
			var key = LeagueKey(leagueCode, season);
			var seasonEnd = DateTime.MinValue;
			if (LeagueSchedules.ContainsKey(key))
			{
				var rounds = LeagueSchedules[key];
				foreach (var round in rounds)
				{
					foreach (var game in round.Value)
					{
						if (game.GameDate > seasonEnd)
							seasonEnd = game.GameDate;
					}
				}
			}
			return seasonEnd;
		}

		public int RoundFor(
			string leagueCode,
			int season,
			DateTime theDate)
		{
			var key = LeagueKey(leagueCode, season);
			var theRound = 0;
			if (LeagueSchedules.ContainsKey(key))
			{
				var rounds = LeagueSchedules[key];
				foreach (var round in rounds)
				{
					foreach (var game in round.Value)
					{
						if (game.GameDate >= theDate)
						{
							theRound = game.Round;
							break;
						}
					}
					if (theRound > 0)
						break;
				}
			}
			return theRound;
		}
	}
}
