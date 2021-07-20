using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace SquareBlock
{
    [System.Serializable]
    public class GameDataGenerator
    {
        private static GameDataGenerator instance = null;

        private static readonly object padlock = new object();
        public static GameDataGenerator Instance {
            get {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameDataGenerator();
                    }
                    return instance;
                }
            }
        }

        private GameDataGenerator()
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, "SquareBlockData.json");

            if (File.Exists(path))
            {
                Debug.Log("<color=green>File Found in " + path + "</color>");
            }
            else
            {
                Debug.Log("<color=blue>File Not Found in " + path + "</color>");

            }
        }
        private static int gameDataGeneratorInstances = 0;

        private GameData gameData;
        public GameData GenerateGameData()
        {
            gameData = GameData.CreateGameData();
            GetGameDataValues(gameData);

            if (gameData != null)
            {
                JsonParser.Instance.WriteJsonData(gameData, "SquareBlockData");

                Debug.Log("<color=blue> GameData is written To : </color>" + JsonParser.Instance.GetDefaultPath());

                return gameData;
            }
            return null;
        }
        private void GetGameDataValues(GameData gameData)
        {
            gameData.gridHeight = 5;
            gameData.gridWidth = 5;
            gameData.nodeDataList = new List<NodeData>();
            string[] colorCodes = { "#FF0000", "#008200", "#1F00FF", "#FF7D00", "#EFEF02" };
            for (int i =0;i<(gameData.gridHeight * gameData.gridWidth); i++)
            {
                NodeData ndata = new NodeData();

                ndata.isEdgeNode = Random.Range(0,3)==1?true:false;
                
                ndata.nodeType = ndata.isEdgeNode ? NodeType.RED : NodeType.MAX; ;

                ndata.nodeColor=ndata.isEdgeNode? colorCodes[Random.Range(0, colorCodes.Length)] : "#FFFFFF"; 

                gameData.nodeDataList.Add(ndata);
            }
          //  Debug.Log("<color=blue> nodeDataList count:</color>" + gameData.nodeDataList.Count);

        }
    }

}