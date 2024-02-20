    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class GridsManager : Singleton<GridsManager>
    {
        [SerializeField] List<GridHandler> gridHandlers;
        [SerializeField] private GoalGridHandler goalGridHandler;
        [SerializeField] SmallGoalGridManager smallGoalGridManager;
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
        }

        private void LetterClicked(LetterCarrier letterCarrier)
        {
            var emptySlot = gridB.GetEmptySlot();
            
            if (emptySlot == null)
                return;
            if (GoalManager.Instance.PartOfTheGoal(letterCarrier))
                return;
            
            LetterManager.Instance.MoveLetterGridB(letterCarrier, emptySlot);
        }

        public int GetGridBEmptySlotCount() => gridB.GetEmptySlotsCount();
        
        private void OnDisable()
        {
            LetterManager.Instance.OnLetterClicked -= LetterClicked;
        }

        public void LetterNeededByGoal(LetterCarrier letterCarrier, int indexOfLetter)
        {
            letterCarrier.CarryingSlot.EmptySlot();
            var slot = goalGridHandler.GetSlot(new Vector2(0, indexOfLetter));
            slot.CarryItem(letterCarrier);
            letterCarrier.GetCarried(slot);
        }

        public void PrepareGridForGoalWord(int goalLength)
        {
            goalGridHandler.InitializeGrid(goalLength);
        }

        public void EmptyASlot(Slot slot)
        {
            slot.EmptySlot();
        }
    }
