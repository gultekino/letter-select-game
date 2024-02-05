    using System;
    using System.Linq;
    using UnityEngine;

    
    public class GoalManager : Singleton<GoalManager>
    {
        LevelGoal currentLevelGoal;
        public event Action OnLetterNeededByGoal;
        public event Action<int> OnGoalWordCompleted;

        private void Start()
        {
            LevelManager.Instance.OnLevelStart += LevelStart;
            LetterManager.Instance.OnLetterSelected += LetterSelected;
        }

        private void LevelStart()
        {
            string word;
            int index;
            (word, index) = LevelManager.Instance.TryGetGoalWord();
            currentLevelGoal = new LevelGoal(word, index);
            GridsManager.Instance.SetGoalGrid(word.Length);
        }

        private void OnDisable()
        {
            LetterManager.Instance.OnLetterSelected -= LetterSelected;
        }

        private void LetterSelected(LetterCarrier letterCarrier)
        {
            var letterCarrying = letterCarrier.GetLetterCarrying();
            var indexesOfLetter = currentLevelGoal.goalWord.AllIndexesOf(letterCarrying); //Is letter in current goal word?
            if(!indexesOfLetter.Any())
                return;

            var indexesOfNotFilledLettersInGoal =
                indexesOfLetter.Where(t => currentLevelGoal.lettersStatus[t] == LetterStatus.LetterEmpty); //Is letters already filled?
            if (!indexesOfNotFilledLettersInGoal.Any())
                return;
            
            var index = indexesOfNotFilledLettersInGoal.First();
            currentLevelGoal.lettersStatus[index] = LetterStatus.LetterFilled;
            GridsManager.Instance.LetterNeededByGoal(letterCarrier, index);
            if (currentLevelGoal.lettersStatus.All(t => t == LetterStatus.LetterFilled)){
                OnGoalWordCompleted?.Invoke(currentLevelGoal.wordIndexOnLevel);
                WordCompleted();
            }
        }

        private void WordCompleted()
        {
            GridsManager.Instance.ClearGoalGrid();
            string word;
            int index;
            (word, index) = LevelManager.Instance.TryGetGoalWord();
            if (word==null)
                return;

            currentLevelGoal = new LevelGoal(word, index);
            GridsManager.Instance.SetGoalGrid(word.Length);
        }
    }