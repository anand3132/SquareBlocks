using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SquareBlock {
    public class GameManager :IController
    {
        public GameObject cellPrefab;
        public Sprite CellTexture;
        public Material lineMaterial;

        private CellElements cellElement = new CellElements();
        private GameData gameData = null;

        private void Start()
        {
            if(ListenerController.Instance==null)
            {
                Debug.Log("<color=red>ListenerController Not Initiated !!! </color>");
            }
            gameData = GameDataGenerator.Instance.GenerateGameData();
            ListenerController.Instance.DispatchEvent("StartGame", gameData);
        }
        public override void RegisterEvents()
        {
            ListenerController.Instance.RegisterObserver("StartGame", this);
            ListenerController.Instance.RegisterObserver("StopGame", this);
            ListenerController.Instance.RegisterObserver("PauseGame", this);

        }

        public override void UnRegisterEvents()
        {
            ListenerController.Instance.UnRegisterObserver("StartGame", this);
            ListenerController.Instance.UnRegisterObserver("StopGame", this);
            ListenerController.Instance.UnRegisterObserver("PauseGame", this);

        }

        protected override void OnEvent(string eventName, params object[] _eventData)
        {
            if (eventName == "StartGame" && _eventData[0] != null)
            {
                InitializeGameElements(_eventData[0] as GameData);
            }
            if (eventName == "StopGame" && _eventData[0] != null)
            {
                if(AnalyzeGameStatus())
                {
                    UpdateLeaderBoard();
                    ShowGameSelectionMenue();
                }
            }
        }

        private void ShowGameSelectionMenue()
        {
            throw new NotImplementedException();
        }

        private void UpdateLeaderBoard()
        {
            throw new NotImplementedException();
        }

        private bool AnalyzeGameStatus()
        {
            throw new NotImplementedException();
        }

        private void InitializeGameElements(GameData gameData)
        {
            ListenerController.Instance.DispatchEvent("InitializeGameElements", gameData);

            cellElement.basePrefab = cellPrefab;
            cellElement.baseSprite = CellTexture;
            cellElement.baseMaterial = lineMaterial;

            ListenerController.Instance.DispatchEvent("PlotGrid", cellElement);

        }

    }
}
