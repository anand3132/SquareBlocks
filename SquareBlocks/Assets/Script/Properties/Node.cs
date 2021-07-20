using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace SquareBlock {
    public class Node : IController,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        private Grid grid;

        public Sprite cellSprite;
        public string cellColor;
        public NodeType nodeType;
        public bool isEdgeNode;
        public int nodeID=-1;

        private LineRenderer lineRenderer;

        public int rowID = -1;
        public int colID = -1;

        Vector3 startPos = Vector3.zero;
        Vector3[] linePositions = null;
        void DrawLines(Vector3[] vertexPositions)
        {
            lineRenderer.loop = false;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = vertexPositions.Length;
            lineRenderer.SetPositions(vertexPositions);
            lineRenderer.numCapVertices = 10;
            lineRenderer.numCornerVertices = 10;
        }

        public override void RegisterEvents()
        {
            ListenerController.Instance.RegisterObserver("UpdateAllCells", this);
            ListenerController.Instance.RegisterObserver("OnDragBegin", this);


        }

        public override void UnRegisterEvents()
        {
            ListenerController.Instance.UnRegisterObserver("UpdateAllCells", this);


        }

        protected override void OnEvent(string eventName, params object[] _eventData)
        {
            if (eventName == "UpdateAllCells")
            {
                Debug.Log("Updating Cell Data : <color=green>"+ nodeID + " </color>");
                grid = GetComponentInParent<Grid>();
                this.lineRenderer = GetComponent<LineRenderer>();
                if (this.lineRenderer == null)
                {
                    this.lineRenderer= gameObject.AddComponent<LineRenderer>();
                }

            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.isEdgeNode)
            {
                GameObject obj = eventData.pointerCurrentRaycast.gameObject;
                if(obj.GetComponent<Node>())
                    ListenerController.Instance.DispatchEvent("OnDragBegin", obj);
            }
        }
                
           //     float rotationSpeed = 20.0f;
           //     float scaleSpeed = 0.2f;
           //     var rect = this.GetComponent<RectTransform>();
           //     var sequence = DOTween.Sequence()
           ////.Append(rect.DOLocalRotate(new Vector3(0, 0, 360), rotationSpeed, RotateMode.FastBeyond360).SetRelative())
           //.Join(rect.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), scaleSpeed, 2, 1f));
           //     sequence.SetLoops(1, LoopType.Yoyo);


           //     linePositions = new Vector3[2];
           //     linePositions[0] = transform.position;
           //     Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
           //     linePositions[1] = worldPosition;
           //     DrawLines(linePositions);
        //    }
        //}

        public void OnDrag(PointerEventData eventData)
        {

           

            ////linePositions = new Vector3[2];
            //linePositions[0] = transform.position;
            //linePositions[0].z = 0;
            //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            //worldPosition.z = 0;
            //linePositions[1] = worldPosition;
            //DrawLines(linePositions);
            ////Debug.Log("ID : \n" + eventData.pointerCurrentRaycast.gameObject.GetComponent<Node>().nodeID.ToString());

            //Debug.Log("<color=red>OnDrag</color> rowID: " + rowID + "   colID: " + colID);
            //Debug.Log("<color=green>---------------------------------------</color>");

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.isEdgeNode)
            {
                GameObject obj = eventData.pointerCurrentRaycast.gameObject;
                if (obj.GetComponent<Node>())
                    ListenerController.Instance.DispatchEvent("OnDragEnd", obj);

            }
            else
                ListenerController.Instance.DispatchEvent("OnDragEnd", null);

        }
    }
}
