    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GridsManager : MonoBehaviour
    {
        [SerializeField] List<GridHandler> gridHandlers;
        private GridHandler gridA;
        private GridHandler gridB;
        private GridHandler gridC;
        
        private void Awake()
        {
            InitializeGrids();
            gridA = gridHandlers[0];
            gridB = gridHandlers[1];
            gridC = gridHandlers[2];
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
            }
        }

        private void InitializeGrids()
        {
            foreach (var gridHandler in gridHandlers)
            {
                gridHandler.InitializeGrid();
            }
        }
    }
