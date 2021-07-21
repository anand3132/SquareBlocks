using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace SquareBlock {
    public class GameManager :IController
    {
        public GameObject cellPrefab;
        public Sprite CellTexture;
        public Material lineMaterial;

        private CellElements cellElement = new CellElements();
        public List<GameData> gdataList; //= new List<GameData>();

        private void Start()
        {
            if(ListenerController.Instance==null)
            {
                Debug.Log("<color=red>ListenerController Not Initiated !!! </color>");
            }
            ListenerController.Instance.DispatchEvent("InitializeUIController");

            string filePath = GameDataGenerator.Instance.GetGameDataFilePath();
            if (File.Exists(filePath))
            {
                string JSONstring = File.ReadAllText(filePath);
                gdataList = JsonParser.Deserialize(typeof(List<GameData>), JSONstring) as List<GameData>;
                //gdataList = JsonUtility.FromJson<List<GameData>>(JSONstring);
                if(gdataList!=null&& gdataList.Count>0)
                {
                    ListenerController.Instance.DispatchEvent("UpdateGameData", gdataList);
                    return;
                }
            }
                GameData gdata = GameDataGenerator.Instance.GenerateGameData(3);
                gdataList.Add(gdata);
                ListenerController.Instance.DispatchEvent("UpdateGameData", gdataList);
                Debug.Log("<color=blue>--------------------------------</color>");
                Debug.Log("<color=blue>Generating New Game Data</color>");
                Debug.Log("<color=blue>--------------------------------</color>");
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
