using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SquareBlock
{
    [System.Serializable]
    public enum NodeType
    {
        MAX
        , RED
        , GREEN
        , BLUE
        , YELLOW
        , ORANGE
    }

    [System.Serializable]
    public class NodeData
    {
        public bool isEdgeNode;
        public string nodeColor;
        public NodeType nodeType;
        public Material nodeMaterial;

    }
    public class CellElements
    {
        public Sprite baseSprite;
        public GameObject basePrefab;
        public Material baseMaterial;
    }

    [System.Serializable]
    public class GameData
    {
        private static int modelDataInstances = 0;
        public static GameData CreateGameData()
        {
            if (modelDataInstances++ < 1)
            {
                return new GameData();
            }
            else
            {
                Debug.Log("<color=red> Cant Create More than one GameData Instance</color>");
            }
            return null;
        }
        protected GameData() { }
        public int gridWidth;
        public int gridHeight;
        public List<NodeData> nodeDataList;

    }


}//squareBlocks


