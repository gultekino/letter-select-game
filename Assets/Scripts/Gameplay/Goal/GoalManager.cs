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
            var indexesOfLetter = currentLevelGoal.goalWord.AllIndexesOf(letterCarrying);
            if(!indexesOfLetter.Any())
                return;

            var indexOfLetter = indexesOfLetter.First(t=>currentLevelGoal.lettersStatus[t]==LetterStatus.LetterEmpty);
            currentLevelGoal.lettersStatus[indexOfLetter] = LetterStatus.LetterFilled;
            GridsManager.Instance.LetterPartOfGoalSelected(letterCarrier, indexOfLetter);
        }
    }