    using System;
    using System.Linq;
    using UnityEngine;

    
    public class GoalManager : Singleton<GoalManager>
    {
        LevelGoal currentLevelGoal;

        private void Start()
        {
            LevelManager.Instance.OnLevelStart += LevelStart;
            LetterManager.Instance.OnLetterSelected += LetterSelected;
        }

        private void LevelStart()
        {
            currentLevelGoal = new LevelGoal(LevelManager.Instance.TryGetGoalWord());
            GridsManager.Instance.SetGoalGrid(currentLevelGoal.goalWord.Length);
        }

        private void OnDisable()
        {
            LetterManager.Instance.OnLetterSelected -= LetterSelected;
        }

        private void LetterSelected(LetterCarrier letterCarrier, Action<LetterCarrier, int> callBack)
        {
            var letterCarrying = letterCarrier.GetLetterCarrying();
            var indexesOfLetter = currentLevelGoal.goalWord.AllIndexesOf(letterCarrying); //Is letter in current goal word?
            if(indexesOfLetter.Count() == 0)
                return;

            var indexesOfNotFilledLettersInGoal =
                indexesOfLetter.Where(t => currentLevelGoal.lettersStatus[t] == LetterStatus.LetterEmpty); //Is letters already filled?
            if (!indexesOfNotFilledLettersInGoal.Any())
                return;
            
            var index = indexesOfNotFilledLettersInGoal.First();
            currentLevelGoal.lettersStatus[index] = LetterStatus.LetterFilled;
            GridsManager.Instance.LetterNeededByGoal(letterCarrier, index);
        }
    }