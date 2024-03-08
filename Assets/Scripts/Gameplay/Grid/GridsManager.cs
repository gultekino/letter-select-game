    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class GridsManager : Singleton<GridsManager>
    {
        [SerializeField] List<GridHandler> gridHandlers;
        [SerializeField] private GoalGridHandler goalGridHandler;
        [SerializeField] private TableGridHandler tableGridHandler;
        private GridHandler gridA;
        private GridHandler gridB;
        private bool newGoalCompleted = false;
        private bool isGoalChanging = false;
        Queue<IEnumerator> goalChangeQueue = new Queue<IEnumerator>();

        #region Initialization

        private void Start()
        {
            InitializeGrids();
        }

        private void InitializeGrids()
        {
            gridA = gridHandlers[0];
            gridB = gridHandlers[1];
            tableGridHandler.InitializeGrid();
            gridA.InitializeGrid();
            gridB.InitializeGrid();
            gridA.FillGridWithLetterCarriers();
            gridA.AlignAllLettersToDown();
        }
        #region Events Subscriptions
        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            LetterManager.Instance.OnLetterClicked += LetterClicked;
            GoalManager.Instance.GoalWordChanged += HandleGoalWordChanged;
        }

        private void UnsubscribeEvents()
        {
            LetterManager.Instance.OnLetterClicked -= LetterClicked;
            GoalManager.Instance.GoalWordChanged -= HandleGoalWordChanged;
        }
        #endregion

        #endregion

        #region GoalWordChanged
         private void HandleGoalWordChanged(int goalWordIndex, int previousGoalWordIndex, int goalWordLength)
        {
            goalChangeQueue.Enqueue(GoalWordChanged(goalWordIndex, previousGoalWordIndex, goalWordLength));
            StartCoroutine(HandleGoalWordChanges());
        }
     
        private IEnumerator HandleGoalWordChanges()
        {
            if (isGoalChanging)
            {
                newGoalCompleted = true;
                yield break;
            }

            isGoalChanging = true;
            while (goalChangeQueue.Count > 0){
                yield return StartCoroutine(goalChangeQueue.Dequeue());
            }
            isGoalChanging = false;
        }
        
        private IEnumerator GoalWordChanged(int goalWordIndex, int previousGoalWordIndex, int goalWordLength)
        {
            var goalSlots = goalGridHandler.Slots;
            yield return MoveGoalToTable(goalSlots, previousGoalWordIndex);
            goalGridHandler.DestroySlotsParent();
            goalGridHandler.InitializeGrid(goalWordLength);
            yield return MoveTableToGoalGrid(goalWordIndex);
            yield return MoveGridBToGoalGrid();
        }

        private IEnumerator MoveGoalToTable(List<Slot> goalSlots, int goalWordIndex)
        {
            yield return new WaitForSeconds(0.5f);
            //TableManager.Instance.FillWordInTable(goalSlots, goalWordIndex);
            tableGridHandler.FillWordInTable(goalSlots, goalWordIndex);
        }
        
        private IEnumerator MoveTableToGoalGrid(int wordIndex)
        {
            //var tableSlots = TableManager.Instance.GetTableSlotsForGoal(wordIndex);
            var tableSlots = tableGridHandler.GetTableSlotsForGoal(wordIndex);
            yield return new WaitForSeconds(0.6f);
            for (var index = 0; index < tableSlots.Count; index++)
            {
                var slot = tableSlots[index];
                var letterCarrier = slot.GetCarriedItem();
                if (letterCarrier)
                {
                    PlaceLetterInGoalGrid(letterCarrier, index);
                }
            }
        }
        
        private IEnumerator MoveGridBToGoalGrid()
        {
            yield return new WaitForSeconds(0.1f);
            foreach (var slot in gridB.Slots)
            {
                if (newGoalCompleted) // GridB can complete the goal so it should listen to the goalCompleted flag
                {
                    newGoalCompleted = false;
                    yield break;
                }
                
                var letterCarrier = slot.GetCarriedItem();
                if (!letterCarrier) continue;
                
                int indexInGoal = GoalManager.Instance.TryGetIndexOfLetterInTheGoal(letterCarrier);
                if (indexInGoal != -1)
                {
                    PlaceLetterInGoalGrid(letterCarrier, indexInGoal);
                }
            }
            gridB.AlignLettersToRight();
        }
        #endregion
       
        
        private void LetterClicked(LetterCarrier letterCarrier)
        {
            var emptySlot = gridB.GetEmptySlot();
            
            if (emptySlot == null || isGoalChanging) //Blocks input while goal is changing
                return; 
            
            var letterCarrierSlot = letterCarrier.CarryingSlot;
            var letterIndexInTheGoal = GoalManager.Instance.TryGetIndexOfLetterInTheGoal(letterCarrier);
            if (letterIndexInTheGoal != -1)//If the letter is in the goal grid
            {
                PlaceLetterInGoalGrid(letterCarrier,letterIndexInTheGoal);
            }
            else
            {
                LetterManager.Instance.MoveLetterGridB(letterCarrier, emptySlot);
            }
            gridA.AlignLettersToDown(letterCarrierSlot);
        }
        
        private void PlaceLetterInGoalGrid(LetterCarrier letterCarrier, int indexOfLetter)
        {
            letterCarrier.CarryingSlot.EmptySlot();
            var slot = goalGridHandler.GetSlot(new Vector2(indexOfLetter, 0));
            slot.CarryItem(letterCarrier);
            letterCarrier.GetCarried(slot);
            GoalManager.Instance.LetterInGoalSelected(indexOfLetter);
        }
        
        public void EmptyASlot(Slot slot)
        {
            slot.EmptySlot();
        }
    }
