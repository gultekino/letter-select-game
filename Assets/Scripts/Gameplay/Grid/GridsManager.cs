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
            //Check if letterCarrier is in gridA
            LetterManager.Instance.LetterGetsCarried(gridB.GetSlot(Vector2.zero), letterCarrier);
        }

        private void OnDisable()
        {
            LetterManager.Instance.OnLetterClicked -= LetterClicked;
        }
    }
