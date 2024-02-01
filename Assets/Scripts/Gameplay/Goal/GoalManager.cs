    using System;
    using System.Linq;
    using UnityEngine;

    public class Goal
    {
        public LetterStatus[] lettersStatus;
       public  string goalWord;

        public Goal(string GoalWord)
        {
            goalWord = GoalWord;
            lettersStatus = new LetterStatus[GoalWord.Length];
        }
    }

    public enum LetterStatus
    {
        Default=-1,
        LetterEmpty,
        LetterFilled
    }

    public class GoalManager : Singleton<GoalManager>
    {
        Goal currentGoal = new Goal("LMALOWTFISECONOMY");

        private void Start()
        {
            LetterManager.Instance.OnLetterSelected += LetterSelected;
        }

        private void OnDisable()
        {
            LetterManager.Instance.OnLetterSelected -= LetterSelected;
        }

        private void LetterSelected(LetterCarrier letterCarrier, Action<LetterCarrier, int> callBack)
        {
            var indexesOfLetter = currentGoal.goalWord.AllIndexesOf(letterCarrier.GetLetterCarrying()); 
            if (!indexesOfLetter.Any())
                return;

            var indexOfLetter = indexesOfLetter.First(t=>currentGoal.lettersStatus[t]==LetterStatus.LetterEmpty);
            currentGoal.lettersStatus[indexOfLetter] = LetterStatus.LetterFilled;
            GridsManager.Instance.LetterPartOfGoalSelected(letterCarrier, indexOfLetter);
        }
    }