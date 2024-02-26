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
            goalChangeQueue.Enqueue(GoalWordChanged(goalWordIndex, previousGoalWordIndex, goalWordLength));
            StartCoroutine(ASD());
        }

        private bool isGoalChanging = false;
        Queue<IEnumerator> goalChangeQueue = new Queue<IEnumerator>();

        private IEnumerator ASD()
        {
            while (goalChangeQueue.Count > 0 && !isGoalChanging)
                yield return StartCoroutine(goalChangeQueue.Dequeue());
        }
        private IEnumerator GoalWordChanged(int goalWordIndex, int previousGoalWordIndex, int goalWordLength)
        {
            isGoalChanging = true;
            var goalSlots = goalGridHandler.Slots;
            yield return MoveGoalToTable(previousGoalWordIndex,goalSlots);
            goalGridHandler.DestroySlotsParent();
            goalGridHandler.InitializeGrid(goalWordLength);
            yield return MoveTableToGoalGrid(goalWordIndex);
            yield return MoveGridBToGoalGrid();
            isGoalChanging = false;
        }

        private IEnumerator MoveGoalToTable(int goalWordIndex, List<Slot> goalSlots)
        {
            yield return new WaitForSeconds(0.5f);
            TableManager.Instance.FillWordInTable(goalSlots, goalWordIndex);
        }

        private void LetterClicked(LetterCarrier letterCarrier)
        {
            var emptySlot = gridB.GetEmptySlot();
            
            if (emptySlot == null || isGoalChanging) //Blocks input while goal is changing
                return; 
            
            var letterIndexInTheGoal = GoalManager.Instance.TryGetIndexOfLetterInTheGoal(letterCarrier);
            if (letterIndexInTheGoal != -1)//If the letter is in the goal grid
            {
                PlaceLetterInGoalGrid(letterCarrier,letterIndexInTheGoal);
            }
            else
            {
                LetterManager.Instance.MoveLetterGridB(letterCarrier, emptySlot);
            }
        }
        
        private void PlaceLetterInGoalGrid(LetterCarrier letterCarrier, int indexOfLetter)
        {
            letterCarrier.CarryingSlot.EmptySlot();
            var slot = goalGridHandler.GetSlot(new Vector2(0, indexOfLetter));
            slot.CarryItem(letterCarrier);
            letterCarrier.GetCarried(slot);
            GoalManager.Instance.LetterInGoalSelected(indexOfLetter);
        }
        
        private void OnDisable()
        {
            LetterManager.Instance.OnLetterClicked -= LetterClicked;
        }

        private IEnumerator MoveTableToGoalGrid(int wordIndex)
        {
            var tableSlots = TableManager.Instance.GetTableSlotsForGoal(wordIndex);
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
                var letterCarrier = slot.GetCarriedItem();
                if (letterCarrier)
                {
                    int indexInGoal = GoalManager.Instance.TryGetIndexOfLetterInTheGoal(letterCarrier);
                    if (indexInGoal != -1)
                    {
                        PlaceLetterInGoalGrid(letterCarrier, indexInGoal);
                    }
                }
            }
        }

        public void EmptyASlot(Slot slot)
        {
            slot.EmptySlot();
        }
    }
