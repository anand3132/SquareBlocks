using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//this is a test comment from main branch
namespace SquareBlock {

    public class Grid : IProperties
    {
        private GameObject CellObjects;
        private int GColumn;
        private int GRow;
        private List<NodeData> GNodeData;
        private Transform[] childs=null;
        private Dictionary<NodeType, Vector3[]> lineBucket = new Dictionary<NodeType, Vector3[]>();
        private void OnGameInitialize(GameData gameData)
        {
            ClearAllGridCells();
            GColumn = gameData.gridHeight;
            GRow = gameData.gridWidth;
            GNodeData = gameData.nodeDataList;
        }
        private void ClearAllGridCells()
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
                currentNode.nodeColor = GNodeData[itr].nodeColor;
                currentNode.nodeMaterial = Cell.baseMaterial;

                if (currentNode.isEdgeNode)
                {
                    currentNode.GetComponent<Image>().sprite = Cell.baseSprite;
                    Color _color;
                    ColorUtility.TryParseHtmlString(currentNode.nodeColor, out _color);

                    if (_color != null)
                    {
                        currentNode.GetComponent<Image>().color = _color;
                        if(currentNode.nodeMaterial)
                            currentNode.nodeMaterial.color = _color;

                    }
                    else
                    {
                        Debug.Log("Not a valid Color: " + currentNode.nodeColor.ToString());
                    }
                }
            }
            Debug.Log("<color=green>------------------------------------------------ </color>");

            ListenerController.Instance.DispatchEvent("UpdateAllCells");

            Debug.Log("<color=green>------------------------------------------------- </color>");

        }

        //------------------------------------------------------------------------------------------------
        private void DrawLines(List<GameObject> currentEdgeList)
        {
            Node startNode = currentEdgeList[0].GetComponent<Node>();
            LineRenderer lineRenderer = currentEdgeList[0].GetComponent<LineRenderer>();

            Vector3[] vertexPositions = new Vector3[currentEdgeList.Count];

            for (int i = 0; i < currentEdgeList.Count; i++)
            {
                Node currentNode = currentEdgeList[i].GetComponent<Node>();
                vertexPositions[i] = currentEdgeList[i].transform.position;
                vertexPositions[i].z -= 10;
                currentNode.nodeStatus = startNode.nodeType;
                currentEdgeList[i].GetComponent<Image>().color = startNode.GetComponent<Image>().color;
            }

            lineRenderer.loop = false;
            lineRenderer.startWidth = 0.3f;
            lineRenderer.endWidth = 0.3f;
            lineRenderer.positionCount = currentEdgeList.Count;

            lineRenderer.numCapVertices = 10;
            lineRenderer.numCornerVertices = 10;
            //lineRenderer.sortingOrder = 3;
            lineRenderer.material = startNode.nodeMaterial;
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
            ListenerController.Instance.RegisterObserver("StopGame", this);

        }

        public override void UnRegisterEvents()
        {
            ListenerController.Instance.UnRegisterObserver("InitializeGameElements", this);
            ListenerController.Instance.UnRegisterObserver("PlotGrid", this);

            ListenerController.Instance.UnRegisterObserver("OnDragBegin", this);
            ListenerController.Instance.UnRegisterObserver("OnDrag", this);
            ListenerController.Instance.UnRegisterObserver("OnDragEnd", this);
            ListenerController.Instance.UnRegisterObserver("StopGame", this);

        }
        public List<GameObject> currentVectorList = new List<GameObject>();
        private Node openNode = null;
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
                    openNode = null;
                    currentVectorList.Clear();
                }
                GameObject obj = _eventData[0] as GameObject;
                currentVectorList.Add(obj);
                openNode = obj.GetComponent<Node>();
            }
            if (eventName == "OnDrag" && _eventData[0] != null)
            {
                GameObject obj = _eventData[0] as GameObject;
                Node currentNode = obj.GetComponent<Node>();
                obj.transform.localScale = Vector3.one;
                float scaleSpeed = 0.2f;

                var rect = currentNode.GetComponent<RectTransform>();
                var sequence = DOTween.Sequence().Join(
                                rect.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), scaleSpeed, 2, 1f));
                sequence.SetLoops(1, LoopType.Yoyo);

                if ((currentNode.isEdgeNode ==true&& currentNode.nodeType!=openNode.nodeType)|| (currentNode.nodeStatus != NodeType.MAX)) {
                   
                    
                    currentVectorList.Clear();
                    Debug.Log("<color=red>Line Crossing.....!! </color>");
                    //UIController.Instance.ToastMsg("ilegal Line Crossing !!!");
                    UIController.Instance.LockUI("ilegal Line Crossing !!!");

                    //ListenerController.Instance.DispatchEvent("StopGame");
                    return;
                }

                foreach (var item in currentVectorList)
                {
                    if (item.GetComponent<Node>().nodeID == currentNode.nodeID)
                        return;
                }
                currentVectorList.Add(obj);
            }
            if (eventName == "OnDragEnd" )
            {
                if (_eventData != null && currentVectorList!=null && currentVectorList.Count>0)
                {
                    GameObject obj = _eventData[0] as GameObject;
                    Node currentNode = obj.GetComponent<Node>();
                    if (openNode.nodeID != currentNode.nodeID && openNode.nodeType == currentNode.nodeType)
                    {
                        currentVectorList.Add(_eventData[0] as GameObject);
                        DrawLines(currentVectorList);
                    }
                    else
                    {
                        currentVectorList.Clear();
                    }
                }
                else
                {
                    currentVectorList.Clear();
                }
            }
            if (eventName == "StopGame")
            {
                lineBucket.Clear();
                currentVectorList.Clear();
                ClearAllGridCells();

            }
        }
    }
}
