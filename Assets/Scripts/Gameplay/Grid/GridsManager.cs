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
            InitializeGrids();
            gridA = gridHandlers[0];
            gridB = gridHandlers[1];
            gridC = gridHandlers[2];
            gridA.FillGridWithLetterCarriers();
        }

        private void OnEnable()
        {
            LetterManager.Instance.OnLetterClicked += LetterClicked;
        }

        private void InitializeGrids()
        {
            foreach (var gridHandler in gridHandlers)
            {
                gridHandler.InitializeGrid();
            }
        }

        private void LetterClicked(LetterCarrier letterCarrier)
        {
            var emptySlot = gridB.GetEmptySlot();
            if (emptySlot == null)
                return;
            LetterManager.Instance.MoveLetterGridB(letterCarrier, emptySlot);
        }

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
            //gridC.SetGridConfiguration(new GridConfiguration(1, goalLength));
            gridC.InitializeGrid();
        }

        public void EmptyASlot(Slot slot)
        {
            slot.EmptySlot();
        }
    }
