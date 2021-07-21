using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

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

        string path= System.IO.Path.Combine(Application.persistentDataPath, "SquareBlockData.json");
        private GameDataGenerator()
        {
            if (File.Exists(path))
            {
                Debug.Log("<color=green>File Found in " + path + "</color>");
            }
            else
            {
                Debug.Log("<color=blue>File Not Found in " + path + "</color>");

            }
        }

        public string GetGameDataFilePath()
        {
            return path;
        }

        private static int gameDataGeneratorInstances = 0;

        private GameData gameData;
        [SerializeField]
        private List<GameData> gameDataList = new List<GameData>();
        public GameData GenerateGameData(int count=1)
        {
            gameData = GameData.CreateGameData();
            GetGameDataValues(gameData);

            if (gameData != null)
            {
                for (int i = 0; i < count; i++)
                {
                    gameDataList.Add(gameData);
                }
                string jstr=JsonParser.Serialize(typeof(List<GameData>), gameDataList);
                JsonParser.Instance.WriteJsonData(jstr, "SquareBlockData");

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
            Dictionary<NodeType, string> colorDic = new Dictionary<NodeType, string>{
                 {NodeType.RED,     "#FF0000" }
                ,{NodeType.GREEN,   "#008200" }
                ,{NodeType.BLUE,    "#1F00FF" }
                ,{NodeType.ORANGE,  "#FF7D00" }
                ,{NodeType.YELLOW,  "#EFEF02" }
            };
          //  string[] colorCodes = { "#FF0000", "#008200", "#1F00FF", "#FF7D00", "#EFEF02" };

            for (int i =0;i<(gameData.gridHeight * gameData.gridWidth); i++)
            {
                NodeData ndata = new NodeData();

                ndata.isEdgeNode = Random.Range(0,3)==1?true:false;
                
                
                ndata.nodeType = ndata.isEdgeNode ? colorDic.Keys.ElementAt((int)Random.Range(0,colorDic.Keys.Count)) : NodeType.MAX;
                
                string colorCode = null;
                colorDic.TryGetValue(ndata.nodeType, out colorCode);
                ndata.nodeColor = colorCode;
                gameData.nodeDataList.Add(ndata);
            }
          //  Debug.Log("<color=blue> nodeDataList count:</color>" + gameData.nodeDataList.Count);

        }
    }

}