﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TipIt.Events;
using TipIt.Helpers;
using TipIt.Interfaces;
using TipIt.Models;
using Serilog;

namespace TipIt.Implementations
{
    public class TippingContext : ITippingContext
    {
        private readonly ILogger _logger;
        public int CurrentSeason { get; set; }

        public TippingContext(
            bool explain = false)
        {
            CurrentSeason = 2025;
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "l:/Users/quart/Dropbox/logs/tipit.log",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
            LeagueDict = LoadLeagues(explain);
            LeagueSchedule = LoadSchedule(explain);
            if (CurrentWeek(explain) < 4 )
                LeaguePastResults = LoadLastYearsResults(explain);
            else
                LeaguePastResults = LoadThisYearsResults(explain);
            if (explain)
                DumpResults("NRL");
        }

        private int CurrentWeek(
            bool explain = false)
        {
            var currentWeek = 0;
            //  Use NRL to pace the season
            foreach (var item in LeagueSchedule["NRL"])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (g.GameDate > DateTime.Now)
                    {
                        currentWeek = item.Key;
                        break;
                    }
                }
                if (currentWeek != 0)
                    break;
            }
            if (explain)
                Console.WriteLine($"Current NRL Week : {currentWeek}");
            return currentWeek;
        }

        public Dictionary<string, Dictionary<int, List<Game>>> LeaguePastResults
        {
            get;
            set;
        }

        public Dictionary<string, Dictionary<int, List<Game>>> LeagueSchedule
        {
            get;
            set;
        }

        public int MaxScore(string leagueCode)
        {
            var maxScore = 0;
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (g.HomeScore > maxScore)
                        maxScore = g.HomeScore;
                    if (g.AwayScore > maxScore)
                        maxScore = g.AwayScore;
                }
            }
            return maxScore;
        }

        public int MinScore(string leagueCode)
        {
            var minScore = 999;
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (g.HomeScore < minScore)
                        minScore = g.HomeScore;
                    if (g.AwayScore < minScore)
                        minScore = g.AwayScore;
                }
            }
            return minScore;
        }

        public Dictionary<string, List<Team>> LeagueDict 
        { 
            get; 
            set; 
        }

        public int NextRound(string leagueCode)
        {
            var nextRound = 0;
            foreach (var item in LeagueSchedule[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (g.GameDate < DateTime.Now)
                        continue;
                    nextRound = item.Key;
                    break;
                }
                if (nextRound > 0)
                    break;
            }
            return nextRound;
        }

        public int HomeFieldAdvantage(
            string leagueCode)
        {
            int homefieldAdvantage;
            var totalHomePoints = 0;
            var totalAwayPoints = 0;
            var totalGames = 0;
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    totalHomePoints += g.HomeScore;
                    totalAwayPoints += g.AwayScore;
                    totalGames++;
                }
            }
            homefieldAdvantage = ( totalHomePoints - totalAwayPoints )
                / totalGames; 
            return homefieldAdvantage;
        }

        internal void DumpResults(string league)
        {
            if (!LeaguePastResults.ContainsKey(league))
                return;
            foreach (var item in LeaguePastResults[league])
            {
                var games = item.Value;
                Console.WriteLine($"Round {item.Key} has {games.Count} games");
                foreach (var g in games)
                {
                    Console.WriteLine(g);
                }
            }
        }

        public int LeagueCount()
        {
            return LeagueDict.Count;
        }

        public decimal AverageScore(
            string leagueCode)
        {
            var totalScore = 0;
            var totalGames = 0;
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    totalScore += g.AwayScore + g.HomeScore;
                }
                totalGames += games.Count;
            }
            var avgScore = ( totalScore / (decimal)totalGames ) / 2.0M;
            return Math.Round(avgScore,0);
        }

        public decimal AverageMargin(
            string leagueCode)
        {
            var totalScore = 0;
            var totalGames = 0;
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    totalScore += Math.Abs(g.AwayScore - g.HomeScore);
                }
                totalGames += games.Count;
            }
            var avgScore = totalScore / (decimal)totalGames;
            return Math.Round(avgScore, 0);
        }

        public decimal AverageScore(
            string leagueCode,
            string teamCode)
        {
            var totalScore = 0;
            var totalGames = 0;
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (g.HomeTeam.Equals(teamCode))
                    {
                        totalScore += g.HomeScore;
                        totalGames++;
                    }
                    if (g.AwayTeam.Equals(teamCode))
                    {
                        totalScore += g.AwayScore;
                        totalGames++;
                    }
                }
            }
            var avgScore = totalScore / (decimal)totalGames;
            return Math.Round(avgScore, 0);
        }

		public string FormLast(
            int gamesBack, 
            string leagueCode, 
            string teamCode)
		{
            var sb = new StringBuilder();
            var teamGameCount = 0;
            var nRounds = LeaguePastResults[leagueCode].Count;
			for (int r = nRounds+1; r > 1; r--)
			{
                if (LeaguePastResults[leagueCode].Count < r - 1)
                    continue;
                var games = LeaguePastResults[leagueCode][r - 1];
				for (int i = games.Count; i > 0; i--)
                {
                    Game g = games[i-1];
                    if (!g.Involves(teamCode))
                        continue;
                    if (g.GameDate.Year < 2020)
                        continue;
                    //if (g.GameDate < DateTime.Now.AddDays(-300))
                    //    continue;
                    sb.Append($"{g.GameResultShort(teamCode)} ");
                    teamGameCount++;
                }
                if (teamGameCount == gamesBack)
                    break;
            }
            return sb.ToString();
        }

		public string CurrentForm(
            string leagueCode,
            string teamCode,
            bool expand = false)
        {
            var sb = new StringBuilder();
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (!g.Involves(teamCode))
                        continue;
                    if (g.GameDate.Year != CurrentSeason)
                        continue;
                    //if (g.GameDate < DateTime.Now.AddDays(-300))
                    //    continue;
                   sb.AppendLine($"{g.GameLine(teamCode)} ");
                }
            }
            return sb.ToString();
		}

		public decimal EasyPointsForTeam(
            string leagueCode,
            string teamCode)
        {
            var record = new Record(teamCode);
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (g.GameDate.Year < CurrentSeason)
                        continue;

                    if (g.HomeTeam.Equals(teamCode)
                        || g.AwayTeam.Equals(teamCode))
                    {
                        if (g.WinFor(teamCode))
                        {
                            record.Wins++;
                            record.EasyPoints += LookupUtils.LookupEasyPoints(
                                teamCode,
                                leagueCode);
                        }
                        else if (g.LossFor(teamCode))
                            record.Losses++;
                        else
                        {
                            record.Draws++;
                            record.EasyPoints += LookupUtils.LookupEasyPoints(
                                teamCode,
                                leagueCode) / 2.0M;
                        }
                    }
                }
            }
            return record.EasyPoints;
        }

        public Record PastRecord(
            string leagueCode,
            string teamCode)
        {
            var record = new Record(teamCode);
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                foreach (var g in games)
                {
                    if (g.GameDate.Year == 2020)
                        Explain("Including results for 2020");
                    if (g.HomeTeam.Equals(teamCode) 
                        || g.AwayTeam.Equals(teamCode))
                    {
                        if (g.WinFor(teamCode))
                        {
                            record.Wins++;
                            record.EasyPoints += LookupUtils.LookupEasyPoints(
                                teamCode,
                                leagueCode);
                        }
                        else if (g.LossFor(teamCode))
                            record.Losses++;
                        else
                        {
                            record.Draws++;
                            record.EasyPoints += LookupUtils.LookupEasyPoints(
                                teamCode,
                                leagueCode) / 2.0M;
                        }
                    }
                }
            }
            return record;
        }

        public string TeamRecords(
            string leagueCode)
        {
            var teamDict = new Dictionary<string, Record>();
            var sb = new StringBuilder();
            var teams = GetTeams(leagueCode);
            foreach (var team in teams)
            {
                var record = PastRecord(
                    leagueCode, 
                    team);
                teamDict.Add(team, record);
            }
            List<KeyValuePair<string, Record>> myList 
                = teamDict.ToList();

            myList.Sort(
                delegate (
                    KeyValuePair<string, Record> pair1,
                    KeyValuePair<string, Record> pair2)
                {
                    return pair2.Value.EasyPoints.CompareTo(
                        pair1.Value.EasyPoints);
                });
            foreach (KeyValuePair<string,Record> pair in myList)
            {
                sb.AppendLine(
                    PastRecord(
                        leagueCode,
                        pair.Key)
                    .ToString());
            }
            return sb.ToString();
        }

        public decimal EasyPoints(
            List<string> aflSet,
            List<string> nrlSet)
        {
            decimal easyPoints = 0.0M;
            easyPoints += EasyPointsFor("NRL", nrlSet);
            easyPoints += EasyPointsFor("AFL", aflSet);
            return easyPoints;
        }

        private decimal EasyPointsFor(
            string leagueCode, 
            List<string> teamSet)
        {
            var easyPoints = 0.0M;
            foreach (var team in teamSet)
            {
                easyPoints += EasyPointsForTeam(
                    leagueCode,
                    team);
                Explain($"{leagueCode}:{team} {easyPoints}");
            }
            return easyPoints;
        }

        public List<string> GetTeams(string leagueCode)
        {
            var teams = new List<string>();
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                Explain($"Round {item.Key} has {games.Count} games");
                foreach (var g in games)
                {
                    if (!teams.Contains(g.HomeTeam))
                        teams.Add(g.HomeTeam);
                    if (!teams.Contains(g.AwayTeam))
                        teams.Add(g.AwayTeam);
                }
            }
            return teams;
        }

        public LeagueStats LastYearsStats(
            string leagueCode)
        {
            var totalScore = 0;
            var totalGames = 0;
            var scoreDict = new SortedDictionary<int, int>();
            foreach (var item in LeaguePastResults[leagueCode])
            {
                var games = item.Value;
                Explain($"Round {item.Key} has {games.Count} games");
                foreach (var g in games)
                {
                    totalGames++;
                    totalScore += g.HomeScore;
                    totalScore += g.AwayScore;
                    AddScore(scoreDict, g.HomeScore);
                    AddScore(scoreDict, g.AwayScore);
                }
                //if (!(totalGames == (item.Key * 8)))
                //    Console.WriteLine("error");
            }
            decimal averageGamePoints = (decimal) totalScore / totalGames;
            var stats = new LeagueStats
            {
                League = leagueCode,
                TotalPoints = totalScore,
                TotalGames = totalGames,
                TeamScoreAverage = (int) Math.Round(averageGamePoints / 2.0M ),
                TeamScoreMode = ScoreMode(scoreDict)                
            };
            return stats;
        }

        private int ScoreMode(
            SortedDictionary<int, int> scoreDict)
        {
            var mode = 0;
            var topFreq = 0;
            foreach (KeyValuePair<int,int> pair in scoreDict)
            {
                if (pair.Value > topFreq)
                {
                    topFreq = pair.Value;
                    mode = pair.Key;
                }
            }
            return mode;
        }

        private void AddScore(
            SortedDictionary<int, int> scoreDict, 
            int score)
        {
            if (!scoreDict.ContainsKey(score))
                scoreDict.Add(score,0);
            scoreDict[score]++;
        }

        public Dictionary<string, List<Team>> LoadLeagues(
            bool explain = false)
        {
            var leagueDict = new Dictionary<string, List<Team>>();
            var eventStore = new TeamEventStore();
            var teams = (List<AddTeamEvent>)eventStore
                .Get<AddTeamEvent>(
                    "add-team");
            foreach (var team in teams)
            {
                var theTeam = new Team(team);
                if (!leagueDict.ContainsKey(theTeam.League))
                    leagueDict.Add(theTeam.League, new List<Team>());
                var teamList = leagueDict[theTeam.League];
                teamList.Add(theTeam);
                leagueDict[theTeam.League] = teamList;
            }
            if (explain)
                DisplayTeams(teams);
            return leagueDict;
        }

        public Dictionary<string, Dictionary<int, List<Game>>> LoadSchedule(
            bool explain = false)
        {
            if (explain)
                Console.WriteLine("Loading Schedules ...");
            var leagueSched = new Dictionary<string, Dictionary<int, List<Game>>>();
            var eventStore = new ScheduleEventStore();
            var events = (List<ScheduleEvent>)eventStore
                .Get<ScheduleEvent>("schedule");
            if (explain)
                Console.WriteLine( $"{events.Count} schedule events found");
            foreach (var e in events)
            {
                var theGame = new Game(e);
                if (!leagueSched.ContainsKey(theGame.League))
                    leagueSched.Add(
                        theGame.League,
                        new Dictionary<int, List<Game>>());
                var roundDict = leagueSched[theGame.League];
                if (!roundDict.ContainsKey(theGame.Round))
                    roundDict.Add(theGame.Round, new List<Game>());
                var gameList = roundDict[theGame.Round];
                gameList.Add(theGame);
                roundDict[theGame.Round] = gameList;
                leagueSched[theGame.League] = roundDict;
            }
            if (explain)
                DisplayLeagues(leagueSched);
            return leagueSched;
        }

        private void DisplayLeagues(
            Dictionary<string, Dictionary<int, List<Game>>> leagueSched)
        {
            foreach (var item in leagueSched.Keys)
            {
                Explain(item);
            }
        }

        public Dictionary<string, Dictionary<int, List<Game>>> LoadLastYearsResults(
            bool explain = false)
        {
            var leagueResults = new Dictionary<string, Dictionary<
                int, 
                List<Game>>>();
            var eventStore = new ResultEventStore();
            var events = (List<ResultEvent>)eventStore.Get<ResultEvent>(
                "results");
            if (explain)
                Explain($"{events.Count} result events found");
            foreach (var e in events)
            {
                var theGame = new Game(e);
                if (!theGame.HasBeenPlayed())
                    continue;

                if (!leagueResults.ContainsKey(theGame.League))
                    leagueResults.Add(
                        theGame.League,
                        new Dictionary<int, List<Game>>());

                var roundDict = leagueResults[theGame.League];

                if (!roundDict.ContainsKey(theGame.Round))
                    roundDict.Add(
                        theGame.Round, 
                        new List<Game>());

                var gameList = roundDict[theGame.Round];
                if (theGame.Round == 19 
                    && theGame.League == "NRL"
                    && theGame.GameDate.Year == 2020)
					Console.WriteLine();

                gameList.Add(theGame);
                roundDict[theGame.Round] = gameList;
                leagueResults[theGame.League] = roundDict;
            }
            if (explain)
            {
                Explain($"{leagueResults["NRL"].Count} results loaded for NRL");
                var gameCount = 0;
                foreach (var round in leagueResults["NRL"])
                {
                    DumpRound(round);
                    gameCount += round.Value.Count;
                }
                Explain($"{gameCount} NRL Games Loaded");
                Explain($"{leagueResults["AFL"].Count} rounds loaded for AFL");
            }
            return leagueResults;
        }

        private void DumpRound(KeyValuePair<int, List<Game>> round)
        {
            foreach (var g in round.Value)
            {
                Explain(g.ToString());
            }
        }

        private void Explain(string msg)
        {
            _logger.Information(msg);
        }

        public Dictionary<string, Dictionary<int, List<Game>>> LoadThisYearsResults(
            bool explain = false)
        {
            if (explain)
                Console.WriteLine("Loading This Years Results");

            var leagueResults = new Dictionary<
                string, 
                Dictionary<
                    int,
                    List<Game>>>();
            var eventStore = new ResultEventStore();
            var events = (List<ResultEvent>)eventStore.Get<ResultEvent>(
                "results");
            foreach (var e in events)
            {
                var theGame = new Game(e);
                if (explain)
                    Console.WriteLine($"result on {theGame.GameDate:u}");
                if (theGame.GameDate.Year != DateTime.Now.Year)
                    continue;

                if (!theGame.HasBeenPlayed())
                    continue;

                if (!leagueResults.ContainsKey(theGame.League))
                {
                    leagueResults.Add(
                        theGame.League,
                        new Dictionary<int, List<Game>>());
                    if (explain)
                        Console.WriteLine(
                            $"Added League {theGame.League}");
                }

                var roundDict = leagueResults[theGame.League];

                if (!roundDict.ContainsKey(theGame.Round))
                {
                    roundDict.Add(
                        theGame.Round,
                        new List<Game>());
                    if (explain)
                        Console.WriteLine(
                            $"Added Round {theGame.Round}");
                }

                var gameList = roundDict[theGame.Round];
                gameList.Add(theGame);
                roundDict[theGame.Round] = gameList;
                leagueResults[theGame.League] = roundDict;
            }
            return leagueResults;
        }

        private void DisplayTeams(
            List<AddTeamEvent> teams)
        {
            foreach (var team in teams)
            {
                var theTeam = new Team(team);
                Explain(theTeam.ToString());
            }
        }

        public void ProcessLeagueSchedule(
            string league,
            IGameProcessor processor)
        {
            foreach (var item in LeagueSchedule[league])
            {
                var games = item.Value;
                Explain($"Round {item.Key} has {games.Count} games");
                var i = 0;
                var lastRound = 0;
                foreach (var g in games)
                {
                    if ( g.Round != lastRound )
					{
                        lastRound = g.Round;
                        i = 0;
					}
                    i++;
                    processor.ProcessGame(g,i);
                }
            }
        }

        #region  IDisposable

        public bool IsDisposed { get; private set; }

        public int ScheduledRoundCount(
            string leagueCode)
        {
            if (LeagueSchedule.ContainsKey(leagueCode))
                return LeagueSchedule[leagueCode].Count;
            return 0;
        }

        public int ScheduledGameCount(
            string leagueCode, 
            int round)
        {
            if (LeagueSchedule.ContainsKey(leagueCode))
            {
                var sched = LeagueSchedule[leagueCode];
                return sched[round].Count;
            }
            return 0;
        }

        ~TippingContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // do nothing just yet
                }
            }

            IsDisposed = true;
        }

        #endregion
    }
}
