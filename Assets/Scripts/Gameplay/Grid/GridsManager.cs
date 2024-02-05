    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GridsManager : Singleton<GridsManager>
    {
        [SerializeField] List<GridHandler> gridHandlers;
        private GridHandler gridA;
        private GridHandler gridB;
        private GridHandler gridC;

        protected override void Awake()
        {
            base.Awake();
            gridA = gridHandlers[0];
            gridA.InitializeGrid();
            gridB = gridHandlers[1];
            gridB.InitializeGrid();
            gridC = gridHandlers[2];
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
            LetterManager.Instance.MoveLetterGridB(letterCarrier, emptySlot);
        }

        public int GetGridBEmptySlotCount() => gridB.GetEmptySlotsCount();
        
        private void OnDisable()
        {
            LetterManager.Instance.OnLetterClicked -= LetterClicked;
        }

        public GridHandler GetGoalGridHandler()
        {
            return gridC;
        }

        public void LetterNeededByGoal(LetterCarrier letterCarrier, int indexOfLetter)
        {
            letterCarrier.CarryingSlot.EmptySlot();
            var slot = gridC.GetSlot(new Vector2(0, indexOfLetter));
            slot.CarryItem(letterCarrier);
            letterCarrier.GetCarried(slot);
        }

        public void SetGoalGrid(int goalLength)
        {
            gridC.InitializeGrid(goalLength);
        }

        public void EmptyASlot(Slot slot)
        {
            slot.EmptySlot();
        }

        public void ClearGoalGrid()
        {
            gridC.ClearGrid();
        }
    }
