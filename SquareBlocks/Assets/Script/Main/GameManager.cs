using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace SquareBlock {
    /// <summary>
    /// This class handle the whole game state logics as well as the game data
    /// </summary>
    public class GameManager :IController
    {
        public GameObject cellPrefab;
        public Sprite CellTexture;
        public Material lineMaterial;

        private CellElements cellElement = new CellElements();
        public List<GameData> gdataList; //= new List<GameData>();


        private  void Start() {
            if (ListenerController.Instance == null) {
                Debug.Log("<color=red>ListenerController Not Initiated !!! </color>");
            }
            ListenerController.Instance.DispatchEvent("InitializeUIController");

            gdataList = CheckAndGetGameData();

            if (gdataList != null && gdataList.Count > 0) {
                ListenerController.Instance.DispatchEvent("UpdateGameData", gdataList);
                return;
            }
            UIController.Instance.ToastMsg("Game Data Error!!");

        }
        /// <summary>
        /// This method search for any existing game data if nothing found it will try to generate one
        /// </summary>
        /// <returns></returns>
        public  List<GameData> CheckAndGetGameData() {
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, "SquareBlockData.json");

            if (File.Exists(filePath)) {
                string JSONstring = File.ReadAllText(filePath);
                gdataList = JsonParser.Deserialize(typeof(List<GameData>), JSONstring) as List<GameData>;
            }
            if (gdataList == null) {
                gdataList = GameDataGenerator.Instance.GenerateGameDataList(10);
                Debug.Log("<color=blue>--------------------------------</color>");
                Debug.Log("<color=blue>Generating New Game Data</color>");
                Debug.Log("<color=blue>--------------------------------</color>");
            }
            return gdataList;
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
            if (eventName == "StopGame" )
            { 

            }
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
