using System;
using System.Collections.Generic;

namespace TipIt.Models
{
    public class Season
    {
        public List<PredictedResult> Predictions { get; set; }
        public Dictionary<string, int> Wins { get; set; }

        public Season()
        {
            Predictions = new List<PredictedResult>();
            Wins = new Dictionary<string, int>();
        }

        public Dictionary<string, int> GetWins()
        {
            foreach (var pred in Predictions)
            {
                TallyResult(pred);
            }
            return Wins;
        }

        private void TallyResult(
            PredictedResult pred)
        {
            var winner = pred.Winner();
            if (!Wins.ContainsKey(winner))
                Wins.Add(winner, 0);
            Wins[winner]++;
        }
    }
}
