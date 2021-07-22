using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace SquareBlock {
    /// <summary>
    /// This class consist of one node's properties and its user interface handlers 
    /// it dispatch the user interaction, towards the grid were the data is processed 
    /// </summary>

    public class Node : IProperties, IBeginDragHandler, IDragHandler, IEndDragHandler {
        private Grid grid;

        public Sprite nodeSprite;
        public string nodeColor;
        public NodeType nodeType;
        public bool isEdgeNode;
        public int nodeID = -1;
        public Material nodeMaterial;
        public NodeType nodeStatus = NodeType.MAX;
        private LineRenderer lineRenderer;

        Vector3 startPos = Vector3.zero;
        void DrawLines(Vector3[] vertexPositions) {
            lineRenderer.loop = false;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = vertexPositions.Length;
            lineRenderer.SetPositions(vertexPositions);
            lineRenderer.numCapVertices = 10;
            lineRenderer.numCornerVertices = 10;
        }

        public override void RegisterEvents() {
            ListenerController.Instance.RegisterObserver("UpdateAllCells", this);
        }

        public override void UnRegisterEvents() {
            ListenerController.Instance.UnRegisterObserver("UpdateAllCells", this);
        }

        protected override void OnEvent(string eventName, params object[] _eventData) {
            if (eventName == "UpdateAllCells") {
                Debug.Log("Updating Cell Data : <color=green>" + nodeID + " </color>");
                grid = GetComponentInParent<Grid>();
                this.lineRenderer = GetComponent<LineRenderer>();
                if (this.lineRenderer == null) {
                    this.lineRenderer = gameObject.AddComponent<LineRenderer>();
                }

            }
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (this.isEdgeNode) {
                GameObject obj = eventData.pointerCurrentRaycast.gameObject;
                if (obj.GetComponent<Node>())
                    ListenerController.Instance.DispatchEvent("OnDragBegin", obj);
                float scaleSpeed = 0.2f;

                var rect = this.GetComponent<RectTransform>();
                var sequence = DOTween.Sequence().Join(
                                rect.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), scaleSpeed, 2, 1f));
                sequence.SetLoops(1, LoopType.Yoyo);
            }
        }

        public int pastNodeID = -1;
        public void OnDrag(PointerEventData eventData) {
            GameObject currentNodeOnDrag = eventData.pointerCurrentRaycast.gameObject;
            if (currentNodeOnDrag == null || !currentNodeOnDrag.GetComponent<Node>())
                return;
            int currentNodeID = currentNodeOnDrag.GetComponent<Node>().nodeID;
            if (pastNodeID != currentNodeID) {
                ListenerController.Instance.DispatchEvent("OnDrag", currentNodeOnDrag);
                pastNodeID = currentNodeID;
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            GameObject obj = eventData.pointerCurrentRaycast.gameObject;

            if (this.isEdgeNode && obj.GetComponent<Node>()) {
                if (obj.GetComponent<Node>().isEdgeNode) {
                    ListenerController.Instance.DispatchEvent("OnDragEnd", obj);
                    return;
                }
                ListenerController.Instance.DispatchEvent("OnDragEnd", null);
            }

        }
    }
}
