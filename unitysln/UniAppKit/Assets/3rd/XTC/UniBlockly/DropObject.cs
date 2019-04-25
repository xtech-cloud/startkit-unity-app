using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XTC.Blockly
{
    public class DropObject : MonoBehaviour, IDropHandler
    {
        public delegate void onObjectDropCallback(DragObject _object);
        public onObjectDropCallback onObjectDrop;
        public string blank {get; private set;}
        public UnityEngine.UI.Text captionText {get;private set;}


        public Image imgIcon {get; private set;}
        private Button btnClear {get;set;}

        void Awake()
        {
            imgIcon = this.transform.Find("icon").GetComponent<Image>();
            imgIcon.gameObject.SetActive(false);

            captionText = this.transform.Find("text").GetComponent<UnityEngine.UI.Text>();
            blank = captionText.text;
            
            btnClear = this.transform.Find("btnClear").GetComponent<Button>();
            btnClear.onClick.AddListener(()=>{
                imgIcon.gameObject.SetActive(false);
                captionText.text = blank;
            });
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject go = eventData.pointerDrag;
            if (null == go)
                return;

            DragObject obj = go.GetComponent<DragObject>();
            if (null == obj)
                return;

            imgIcon.sprite = obj.icon.sprite;
            captionText.text = obj.text;
            imgIcon.gameObject.SetActive(true);

            if (null != onObjectDrop)
                onObjectDrop(obj);
        }
    }
}//namespace
