    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class GridsManager : Singleton<GridsManager>
    {
        [SerializeField] List<GridHandler> gridHandlers;
        [SerializeField] private GoalGridHandler goalGridHandler;
        private GridHandler gridA;
        private GridHandler gridB;

        protected override void Awake()
        {
            base.Awake();
            gridA = gridHandlers[0];
            gridA.InitializeGrid();;
            gridB = gridHandlers[1];
            gridB.InitializeGrid();
            gridA.FillGridWithLetterCarriers();
        }

        private void OnEnable()
        {
            LetterManager.Instance.OnLetterClicked += LetterClicked;
            GoalManager.Instance.GoalWordChanged += HandleGoalWordChanged;
        }

        private void HandleGoalWordChanged(int goalWordIndex, int previousGoalWordIndex, int goalWordLength)
        {
            //StartCoroutine(GoalWordChanged(goalWordIndex, previousGoalWordIndex, goalWordLength));
            
            
            MoveGoalToTable(previousGoalWordIndex);
            goalGridHandler.InitializeGrid(goalWordLength);
            MoveTableToGoalGrid(goalWordIndex);
            MoveGridBToGoalGrid();
        }

        private IEnumerator GoalWordChanged(int goalWordIndex, int previousGoalWordIndex, int goalWordLength)
        {
            MoveGoalToTable(goalWordIndex);
            yield break;
        }

        private IEnumerator MoveGoalToTable(int goalWordIndex)
        {
            var goalSlots = goalGridHandler.Slots;
            TableManager.Instance.FillWordInTable(goalSlots, goalWordIndex);
            yield break;
        }

        private void LetterClicked(LetterCarrier letterCarrier)
        {
            var emptySlot = gridB.GetEmptySlot();
            
            if (emptySlot == null)
                return; 
            
            var letterIndexInTheGoal = GoalManager.Instance.TryGetIndexOfLetterInTheGoal(letterCarrier);
            if (letterIndexInTheGoal != -1)//If the letter is in the goal grid
            {
                PlaceLetterInGoal(letterCarrier,letterIndexInTheGoal);
                GoalManager.Instance.LetterInGoalSelected(letterIndexInTheGoal);
            }
            else
            {
                LetterManager.Instance.MoveLetterGridB(letterCarrier, emptySlot);
            }
        }
        
        public void PlaceLetterInGoal(LetterCarrier letterCarrier, int indexOfLetter)
        {
            letterCarrier.CarryingSlot.EmptySlot();
            var slot = goalGridHandler.GetSlot(new Vector2(0, indexOfLetter));
            slot.CarryItem(letterCarrier);
            letterCarrier.GetCarried(slot);
        }
        
        public int GetGridBEmptySlotCount() => gridB.GetEmptySlotsCount();
        
        private void OnDisable()
        {
            LetterManager.Instance.OnLetterClicked -= LetterClicked;
        }

        private void MoveTableToGoalGrid(int wordIndex)
        {
            var tableSlots = TableManager.Instance.GetTableSlotsForGoal(wordIndex);
            for (var index = 0; index < tableSlots.Count; index++)
            {
                var slot = tableSlots[index];
                var letterCarrier = slot.GetCarriedItem();
                if (letterCarrier)
                {
                    PlaceLetterInGoal(letterCarrier, index);
                }
            }
        }

        private void MoveGridBToGoalGrid()
        {
            foreach (var slot in gridB.Slots)
            {
                var letterCarrier = slot.GetCarriedItem();
                if (letterCarrier)
                    GoalManager.Instance.TryGetIndexOfLetterInTheGoal(letterCarrier);
            }
        }

        public void EmptyASlot(Slot slot)
        {
            slot.EmptySlot();
        }

        public List<Slot> GetGoalGridSlots()
        {
            return goalGridHandler.Slots;
        }
    }
