using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SquareBlock {

    public class Grid : IController
    {
        private GameObject CellObjects;
        private int GColumn;
        private int GRow;
        private List<NodeData> GNodeData;
        private Transform[] childs=null;
        private Dictionary<NodeType, Vector3[]> lineBucket = new Dictionary<NodeType, Vector3[]>();
        private void OnGameInitialize(GameData gameData)
        {
            //forward allocation
            for (int x=(int)NodeType.RED;x<=(int)NodeType.ORANGE;x++)
            {
                lineBucket.Add((NodeType)x, new Vector3[0]);
            }

            childs = this.GetComponentsInChildren<Transform>(true)
                            .Where(x => x.gameObject.transform.parent != transform.parent).ToArray();
            if(childs!=null)
            {
                foreach(Transform item in childs)
                {
                    Destroy(item.gameObject);
                }
            }
            GColumn = gameData.gridHeight;
            GRow = gameData.gridWidth;
            GNodeData = gameData.nodeDataList;
        }
        private void PlotGrid(CellElements Cell)
        {
            int totalCell = GColumn * GRow;
            for (int itr = 0; itr < totalCell; itr++)
            {
                int row = itr / GColumn;
                int col = itr % GColumn;
                GameObject currentCell = Instantiate(Cell.basePrefab, this.transform);
                Node currentNode = currentCell.GetComponent<Node>();

                currentNode.nodeID = itr;
                currentNode.isEdgeNode = GNodeData[itr].isEdgeNode;
                currentNode.nodeType = GNodeData[itr].nodeType;
                currentNode.cellColor = GNodeData[itr].nodeColor;
                currentNode.rowID = row;
                currentNode.colID = col;

                if (currentNode.isEdgeNode)
                {
                    currentNode.GetComponent<Image>().sprite = Cell.baseSprite;

                    Color _color;
                    ColorUtility.TryParseHtmlString(currentNode.cellColor, out _color);

                    if (_color != null)
                    {
                        currentNode.GetComponent<Image>().color = _color;
                    }
                    else
                    {
                        Debug.Log("Not a valid Color: " + currentNode.cellColor.ToString());
                    }
                }
            }
            Debug.Log("<color=green>------------------------------------------------ </color>");

            ListenerController.Instance.DispatchEvent("UpdateAllCells");

            Debug.Log("<color=green>------------------------------------------------- </color>");

        }

        private void AddPointToList(Vector3 pos)
        {

        }
        //------------------------------------------------------------------------------------------------
        private void DrawLines(Vector3[] vertexPositions, LineRenderer lineRenderer)
        {
            lineRenderer.loop = false;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = vertexPositions.Length;
            lineRenderer.SetPositions(vertexPositions);
            lineRenderer.numCapVertices = 10;
            lineRenderer.numCornerVertices = 10;
        }

        //------------------------------------------------------------------------------------------------
        public override void RegisterEvents()
        {
            ListenerController.Instance.RegisterObserver("InitializeGameElements",this);
            ListenerController.Instance.RegisterObserver("PlotGrid",this);

            ListenerController.Instance.RegisterObserver("OnDragBegin", this);
            ListenerController.Instance.RegisterObserver("OnDrag", this);
            ListenerController.Instance.RegisterObserver("OnDragEnd", this);

        }

        public override void UnRegisterEvents()
        {
            ListenerController.Instance.UnRegisterObserver("InitializeGameElements", this);
            ListenerController.Instance.UnRegisterObserver("PlotGrid", this);

            ListenerController.Instance.UnRegisterObserver("OnDragBegin", this);
            ListenerController.Instance.UnRegisterObserver("OnDrag", this);
            ListenerController.Instance.UnRegisterObserver("OnDragEnd", this);

        }
        public List<GameObject> currentVectorList = new List<GameObject>();
        protected override void OnEvent(string eventName, params object[] _eventData)
        {
            if (eventName == "InitializeGameElements" && _eventData[0] != null)
            {
                OnGameInitialize(_eventData[0] as GameData);
            }
            if (eventName == "PlotGrid" && _eventData[0] != null)
            {
                PlotGrid(_eventData[0] as CellElements);
            }
            if (eventName == "OnDragBegin" && _eventData[0] != null)
            {
                currentVectorList.Add(_eventData[0] as GameObject);

            }
            if (eventName == "OnDrag" && _eventData[0] != null)
            {

            }
            if (eventName == "OnDragEnd" )
            {
                GameObject obj = _eventData[0] as GameObject;

                if (obj != null && currentVectorList.Count>0)
                {
                    if(currentVectorList[0].GetComponent<Node>().nodeID != obj.GetComponent<Node>().nodeID)
                        currentVectorList.Add(_eventData[0] as GameObject);
                    else
                        currentVectorList.Clear();
                }
                else
                {
                    currentVectorList.Clear();
                }
            }
        }
    }
}
