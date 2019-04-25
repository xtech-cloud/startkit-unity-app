using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XTC.Blockly
{

    [RequireComponent(typeof(Image))]
    public class DragBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void OnEndDragCallback(DragBlock _sender);
        public delegate void OnBeginDragCallback(DragBlock _sender);
        public delegate void OnAbortDragCallback(DragBlock _sender);

        public string method { get; set; }
        public OnBeginDragCallback onBeginDrag;
        public OnEndDragCallback onEndDrag;
        public OnAbortDragCallback onAbortDrag;

        public PointerEventData eventData { get; set; }

        private Dictionary<int, GameObject> draggingImage_ = new Dictionary<int, GameObject>();
        private Dictionary<int, RectTransform> draggingPlanes_ = new Dictionary<int, RectTransform>();

        public void OnBeginDrag(PointerEventData _eventData)
        {
            var canvas = FindInParents<Canvas>(gameObject);
            if (canvas == null)
                return;

            eventData = _eventData;

            GameObject clone = GameObject.Instantiate(this.transform.parent.gameObject);
            clone.name = method;

            // We have clicked something that can be dragged.
            // What we want to do is create an thumb for this.
            draggingImage_[eventData.pointerId] = clone;

            draggingImage_[eventData.pointerId].transform.SetParent(canvas.transform, false);
            draggingImage_[eventData.pointerId].transform.SetAsLastSibling();

            // The thumb will be under the cursor.
            // We want it to be ignored by the event system.
            var group = draggingImage_[eventData.pointerId].AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;

            draggingPlanes_[eventData.pointerId] = transform as RectTransform;

            SetDraggedPosition(eventData);

            if (null != onBeginDrag)
                onBeginDrag(this);
        }

        public void OnDrag(PointerEventData _eventData)
        {
            eventData = _eventData;

            if (draggingImage_[eventData.pointerId] != null)
                SetDraggedPosition(eventData);
        }

        private void SetDraggedPosition(PointerEventData _eventData)
        {
            eventData = _eventData;
            if (eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
                draggingPlanes_[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;

            var rt = draggingImage_[eventData.pointerId].GetComponent<RectTransform>();
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlanes_[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos;
                rt.rotation = draggingPlanes_[eventData.pointerId].rotation;
            }
        }

        public void OnEndDrag(PointerEventData _eventData)
        {
            eventData = _eventData;
            if (draggingImage_[eventData.pointerId] != null)
                Destroy(draggingImage_[eventData.pointerId]);

            draggingImage_[eventData.pointerId] = null;
            draggingImage_.Remove(eventData.pointerId);

            if (null != onAbortDrag)
                onAbortDrag(this);

            //ignore when drop on ui
            //if(eventData.pointerCurrentRaycast.isValid)
            //	return;

            if (null != onEndDrag)
                onEndDrag(this);
        }

        static public T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null) return null;
            var comp = go.GetComponent<T>();

            if (comp != null)
                return comp;

            var t = go.transform.parent;
            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
            return comp;
        }
    }
}//namespace
