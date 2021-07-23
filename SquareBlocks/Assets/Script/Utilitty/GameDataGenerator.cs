using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace SquareBlock
{
/// <summary>
/// This is a Generator Class which is used to generate game data object  consit of random cirle placements
/// </summary>
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

        [SerializeField]
        public List<GameData> GenerateGameDataList(int elements = 1)
        {

            List<GameData> gameDataList = new List<GameData>();
            for (int i = 0; i < elements; i++)
            {
                GameData gameData = GenerateGameData(new GameData());
                gameDataList.Add(gameData);
            }
            //Write game data list json to path

            string jstr = JsonParser.Serialize(typeof(List<GameData>), gameDataList);
            JsonParser.Instance.WriteJsonData(jstr, "SquareBlockData");
            Debug.Log("<color=blue> GameData is written To : </color>" + JsonParser.Instance.GetDefaultPath());

            return gameDataList;
        }
        private GameData GenerateGameData(GameData gameData)
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
            return gameData;

        }
    }

}