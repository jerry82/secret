using System;
using System.Collections.Generic;

namespace AmazonInterview
{
    public class Record
    {
        public string StudentId { get; set; }
        public string TestId { get; set; }
        public double TestScore { get; set; } 
    }

    public class FinalScore
    {
        public string StudentId { get; set; }
        public double Score { get; set; } 
    }

    public class ScoreQueue 
    {
        public int Top { get; set; }
        
        //this is a sorted score
        public List<double> Scores { get; set; }
   
        /// <summary>
        /// calculate the average score, sometimes Top will > Scores.Count
        /// </summary>
        /// <returns></returns>
        public double GetAverage()
        {
            if (Scores.Count == 0)
                return 0.0;

            //sort scores in ascending order
            MergeSort.Ascend(Scores);

            //calculate the average
            int lim = Top < Scores.Count ? Top : Scores.Count;
            double sum = 0.0;
            for (int i = 0; i < lim; i++)
            {
                sum += Scores[Scores.Count - 1 - i];
            }

            return sum / lim;
        }
    }

    /// <summary>
    /// main class to calculate the student final score
    /// </summary>
    public class Calculator 
    {
        public void BuildScoreDict(Dictionary<string, ScoreQueue> studentScoreDict, List<Record> records, int top)
        {
            foreach (Record record in records)
            {
                string studentId = record.StudentId;

                if (!studentScoreDict.ContainsKey(studentId))
                {
                    studentScoreDict.Add(studentId, new ScoreQueue() { Top = top });
                }

                ScoreQueue queue = studentScoreDict[studentId];
                queue.Scores.Add(record.TestScore);
            }
        }

        /// <summary>
        /// build score dictionary take O(X): X is the number of record
        /// calculate the score of student takes: O(N * K * log(K)):
        /// N : is the number of student
        /// K is the number of tests 
        /// time to merge sort the score of K test, it takes K * log(K)
        /// 
        /// total time: O(X) + O(N * K * log(K)) ~ O(N * K * log(K))
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public List<FinalScore> CalculateFinalScoreFromRecords(List<Record> records)
        {
            Dictionary<string, ScoreQueue> studentScoreDict = new Dictionary<string, ScoreQueue>();
            List<FinalScore> finalScores = new List<FinalScore>();

            //build the dictionary 
            BuildScoreDict(studentScoreDict, records, 5);

            //calculate individual score
            foreach (string studentId in studentScoreDict.Keys)
            {
                ScoreQueue queue = (ScoreQueue)studentScoreDict[studentId];
                FinalScore finalScore = new FinalScore();
                finalScore.StudentId = studentId;
                finalScore.Score = queue.GetAverage();

                finalScores.Add(finalScore);
            }

            return finalScores;
        }
    }

    #region helpers
    public class MergeSort
    {
        public static void Ascend(List<double> arr)
        {
            List<double> result = DoMerge(arr, 0, arr.Count - 1);
            for (int i = 0; i < result.Count; i++)
            {
                arr[i] = result[i];
            }
        }

        private static List<double> DoMerge(List<double> l, int start, int end)
        {
            List<double> result = new List<double>();
            if (start == end)
            {
                result.Add(l[start]);
                return result;
            }

            int mid = (int)((start + end) / 2);
            List<double> left = DoMerge(l, start, mid);
            List<double> right = DoMerge(l, mid + 1, end);

            result = Merge(left, right);
            return result;
        }

        private static List<double> Merge(List<double> a, List<double> b)
        {
            List<double> result = new List<double>();

            int iA = 0;
            int iB = 0;

            while (result.Count < a.Count + b.Count)
            {
                if (iA >= a.Count && iB < b.Count)
                {
                    result.Add(b[iB++]);
                }
                else if (iB >= b.Count && iA < a.Count)
                {
                    result.Add(a[iA++]);
                }
                else
                {
                    if (a[iA] < b[iB])
                        result.Add(a[iA++]);
                    else
                        result.Add(b[iB++]);
                }
            }

            return result;
        }
    }
    #endregion
}
