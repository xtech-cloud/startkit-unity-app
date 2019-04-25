using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XTC.Blockly
{
    public class DropBlock : MonoBehaviour, IDropHandler
    {
        public delegate void onBlockDropCallback(DragBlock _block);
        public onBlockDropCallback onBlockDrop;

        public void OnDrop(PointerEventData eventData)
        {
            GameObject go = eventData.pointerDrag;
            if (null == go)
                return;

            DragBlock block = go.GetComponent<DragBlock>();
            if (null == block)
                return;

			Debug.Log(block.method);
            if (null != onBlockDrop)
                onBlockDrop(block);
        }
    }
}//namespace
