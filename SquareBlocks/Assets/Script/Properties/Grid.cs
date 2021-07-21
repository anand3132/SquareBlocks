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
            childs = this.GetComponentsInChildren<Transform>(true)
                .Where(x => x.gameObject.transform.parent != transform.parent).ToArray();
            if (childs != null)
            {
                foreach (Transform item in childs)
                {
                    Destroy(item.gameObject);
                }
            }

            //forward allocation
            for (int x=(int)NodeType.RED;x<=(int)NodeType.ORANGE;x++)
            {
                lineBucket.Add((NodeType)x, new Vector3[0]);
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
                currentCell.SetActive(true);
                Node currentNode = currentCell.GetComponent<Node>();

                currentNode.nodeID = itr;
                currentNode.isEdgeNode = GNodeData[itr].isEdgeNode;
                currentNode.nodeType = GNodeData[itr].nodeType;
                currentNode.cellColor = GNodeData[itr].nodeColor;
                currentNode.cellMaterial = Cell.baseMaterial;


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
                        if(currentNode.cellMaterial)
                            currentNode.cellMaterial.color = _color;

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
        //private void DrawLines(Vector3[] vertexPositions, LineRenderer lineRenderer)
        private void DrawLines(List<GameObject> currentEdgeList)
        {
            LineRenderer lineRenderer = currentEdgeList[0].GetComponent<LineRenderer>();
            Vector3[] vertexPositions = new Vector3[currentEdgeList.Count];

            for(int i=0;i<currentEdgeList.Count;i++)
            {
                vertexPositions[i] = currentEdgeList[i].transform.position;
            }

            lineRenderer.loop = false;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = currentEdgeList.Count;

            lineRenderer.numCapVertices = 10;
            lineRenderer.numCornerVertices = 10;
            //lineRenderer.sortingOrder = 3;
            lineRenderer.sortingLayerName = "Foreground";
            lineRenderer.SetPositions(vertexPositions);
            Debug.Log("<color=blue> Drawing Color </color>");
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
        private int openNodeID = -1;
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
                if (currentVectorList.Count > 0)
                {
                    openNodeID = -1;
                    currentVectorList.Clear();
                }
                GameObject obj = _eventData[0] as GameObject;
                currentVectorList.Add(obj);
                openNodeID = obj.GetComponent<Node>().nodeID;
            }
            if (eventName == "OnDrag" && _eventData[0] != null)
            {
                GameObject obj = _eventData[0] as GameObject;
                int currentNodeID = obj.GetComponent<Node>().nodeID;
                foreach (var item in currentVectorList)
                {
                    if (item.GetComponent<Node>().nodeID == currentNodeID)
                        return;
                }
                currentVectorList.Add(obj);
            }
            if (eventName == "OnDragEnd" )
            {
                if (_eventData != null && currentVectorList!=null && currentVectorList.Count>0)
                {
                    GameObject obj = _eventData[0] as GameObject;
                    obj = _eventData[0] as GameObject;

                    if (openNodeID != obj.GetComponent<Node>().nodeID)
                    {
                        currentVectorList.Add(_eventData[0] as GameObject);
                        DrawLines(currentVectorList);
                    }
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
