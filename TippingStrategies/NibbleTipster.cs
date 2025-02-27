﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TipIt.Helpers;
using TipIt.Implementations;
using TipIt.Interfaces;
using TipIt.Models;

namespace TipIt.TippingStrategies
{
	public class NibbleTipster : BaseTipster, ITipster
	{
		public Dictionary<string,NibbleRating> Ratings { get; set; }
		public decimal AverageScore { get; set; }
		public int MaxScore { get; set; }
		public int MinScore { get; set; }
		public int HomeFieldAdvantage { get; set; }

		public NibbleTipster(
			TippingContext context) : base(context)
		{
			Ratings = new Dictionary<string, NibbleRating>();

        }

		public string ShowTips(
			string league,
			int round)
        {
            Rate(league);

            DumpMetrics();

            Tip(
                league,
                round);
            return Output();
        }

        public void Rate(string league)
        {
            AverageScore = Context.AverageScore(leagueCode: league);
			Debug.WriteLine($"{league} : Average Score: {AverageScore}");

            HomeFieldAdvantage = Context.HomeFieldAdvantage(leagueCode: league);
			Debug.WriteLine($"{league} : Homefield Advantage: {HomeFieldAdvantage}");

            MaxScore = Context.MaxScore(league);
			Debug.WriteLine($"{league} : Max Score: {MaxScore}");

            MinScore = Context.MinScore(league);
			Debug.WriteLine($"{league} : Min Score: {MinScore}");

            Debug.WriteLine(RateResults(league));
        }

        private void DumpMetrics()
		{
			Console.WriteLine(
				$"Average Score      : {AverageScore}");
			Console.WriteLine(
				$"Homefield Advantage: {HomeFieldAdvantage}");
			Console.WriteLine(
				$"Max Score          : {MaxScore}");
			Console.WriteLine(
				$"Min Score          : {MinScore}");
			Console.WriteLine();
		}

		public List<PredictedResult> Tip(
			string league, 
			int round)
		{
			Predictions.Clear();
			if (round < 1)
				return Predictions;
			var sched = Context.LeagueSchedule[league][round];
			foreach (var game in sched)
			{
				var prediction = Tip(
					game);

				Predictions.Add(
					prediction);
			}
			return Predictions;
		}

		public string RateResults(
			string leagueCode)
		{
			var gamesRated = 0;
			foreach (var item in Context.LeaguePastResults[leagueCode])
            {
                var round = item.Value;
                foreach (var g in round)
                {
					var gameRating = RateGame(g);
					AdjustTeam(
						g.HomeTeam,
						gameRating.HomeRating);
					AdjustTeam(
						g.AwayTeam,
						gameRating.AwayRating);
					gamesRated++;
				}
			}
			Console.WriteLine();
			Console.WriteLine(
				$"Ratings done on {gamesRated} games");
			Debug.WriteLine(
                $"Ratings done on {gamesRated} games");
            Console.WriteLine();
			return DumpRatings(
				leagueCode);
        }

		internal void ClearRatings()
		{
			Ratings.Clear();
		}

		public string DumpRatings(
			string leagueCode)
		{
			var list = Ratings.ToList();
			list.Sort(
				delegate (KeyValuePair<string, NibbleRating> pair1,
				KeyValuePair<string, NibbleRating> pair2)
				{
					return pair2.Value.Total()
						.CompareTo(
							pair1.Value.Total());
				});
			var sb = new StringBuilder();
			foreach (KeyValuePair<string,NibbleRating> pair in list)
			{
				sb.AppendLine(
					$@"{
						StringUtils.StringOfSize(4,pair.Key)
						} {
						pair.Value
						} {
						CurrentForm(
							leagueCode,
							pair.Key)
						}");
			}
			return sb.ToString();
		}

		private string CurrentForm(
			string leagueCode,
			string teamCode)
		{
			return Context.FormLast(
				4,
				leagueCode,
				teamCode);
		}

		private void AdjustTeam(
			string team, 
			NibbleRating rating)
		{
			if (!Ratings.ContainsKey(team))
				Ratings.Add(team, new NibbleRating());
			Ratings[team].Offence += rating.Offence;
			Ratings[team].Defence += rating.Defence;
		}

		public NibbleGameRating RateGame(
			Game game, int averageScore)
		{
			AverageScore = averageScore;
			return RateGame(game);
		}

		public NibbleGameRating RateGame(
			Game g)
		{
			var adjustment = new NibbleGameRating();
			if (!Ratings.ContainsKey(g.HomeTeam))
				Ratings.Add(g.HomeTeam, new NibbleRating());
			if (!Ratings.ContainsKey(g.AwayTeam))
				Ratings.Add(g.AwayTeam, new NibbleRating());

			var projHome = Ratings[g.HomeTeam].Offence 
				+ Ratings[g.AwayTeam].Defence;
			projHome = (projHome / 2) + (int) AverageScore;
			projHome += HomeFieldAdvantage;
			var projAway = Ratings[g.AwayTeam].Offence
				+ Ratings[g.HomeTeam].Defence;
			projAway = (projAway / 2) + (int) AverageScore;

			adjustment.HomeRating.Offence = (int)
				(g.HomeScore - projHome) / FudgeFactor(g);
			adjustment.HomeRating.Defence = (int)
				(g.AwayScore - projAway) / FudgeFactor(g);
			adjustment.AwayRating.Offence = (int)
				(g.AwayScore - projAway) / FudgeFactor(g);
			adjustment.AwayRating.Defence = (int)
				(g.HomeScore - projHome) / FudgeFactor(g);
			return adjustment;
		}

		private int FudgeFactor(Game game)
		{
			if (game.GameDate > DateTime.Now.AddDays(-7) 
				|| game.GameDate.Year == 2020 && game.Round == 19)
				return 2;
			if (game.GameDate > DateTime.Now.AddDays(-14)
				|| game.GameDate.Year == 2020 && game.Round == 18)
				return 3;
			if (game.GameDate > DateTime.Now.AddDays(-21)
				|| game.GameDate.Year == 2020 && game.Round == 17)
				return 4;
			return 5;
		}

		public PredictedResult Tip(
			Game g)
		{
			var result = new PredictedResult(g);

			var homeOff = Ratings[g.HomeTeam].Offence;
			var homeDef = Ratings[g.HomeTeam].Defence;
			var awayOff = Ratings[g.AwayTeam].Offence;
			var awayDef = Ratings[g.AwayTeam].Defence;

			var homeScore = AverageScore + ((homeOff + awayDef) / 2);
			var awayScore = AverageScore + ((awayOff + homeDef) / 2);

			result.HomeScore = MaxMin((int)homeScore);
			result.AwayScore = MaxMin((int)awayScore);

			return result;
		}

		private int MaxMin(int score)
		{
			if (score > MaxScore)
				score = MaxScore;
			if (score < MinScore)
				score = MinScore;
			return score;
		}

	}

	public class NibbleRating
	{
		public int Offence { get; set; }
		public int Defence { get; set; }

		public NibbleRating()
		{
			Offence = 0;
			Defence = 0;
		}

		public override string ToString()
		{
			return $@"Off:{
				StringUtils.PadLeft(4, Offence.ToString())
				} Def:{
				StringUtils.PadLeft(4, Defence.ToString())
				} Tot:{
				StringUtils.PadLeft(4, Total().ToString())
				}";
		}

		public int Total()
		{
			return Offence + (-Defence);
		}
	}

	public class NibbleGameRating
	{
		public NibbleRating HomeRating { get; set; }
		public NibbleRating AwayRating { get; set; }

		public NibbleGameRating()
		{
			HomeRating = new NibbleRating();
			AwayRating = new NibbleRating();
		}
		public override string ToString()
		{
			return $"Home:{HomeRating} Away:{AwayRating}";
		}
	}
}
