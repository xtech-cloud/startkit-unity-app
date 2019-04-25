using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

namespace XTC.Blockly
{
    public class BlockView : View
    {
        public const string NAME = "BlockView";

        private BlocklyUI uiBlockly
        {
            get
            {
                return (UIFacade.Find(BlocklyFacade.NAME) as BlocklyFacade).uiBlockly;
            }
        }
        protected override void setup()
        {
            uiBlockly.root.gameObject.SetActive(true);
            uiBlockly.btnSearchBlock.gameObject.SetActive(true);
            uiBlockly.btnFold.gameObject.SetActive(false);

        }
        protected override void bindEvents()
        {
        }

        protected override void unbindEvents()
        {
        }

        protected override void dismantle()
        {

        }

        public bool active{
            set{
                uiBlockly.rootBlockBar.gameObject.SetActive(value);
            }
        }

        public void RefreshBlocks(BlockModel.BlockStatus _status)
        {
            CustomElementModel.CustomElementStatus statusElement = _status.Access(CustomElementModel.CustomElementStatus.NAME) as CustomElementModel.CustomElementStatus;
            foreach (BlockModel.Block block in _status.blocks)
            {
                addBlock(block, statusElement);
            }
        }

        private void addBlock(BlockModel.Block _block, CustomElementModel.CustomElementStatus _elementStatus)
        {
            GameObject clone = GameObject.Instantiate(uiBlockly.tsTempalteBlock.gameObject);
            clone.transform.SetParent(uiBlockly.tsTempalteBlock.parent);
            clone.transform.localScale = Vector3.one;
            clone.SetActive(true);
            clone.name = _block.method;

            BlockBuilder.BuildBlock(_block, clone, _elementStatus);

            Transform tsSection = uiBlockly.tsTempalteBlock.parent.Find(_block.ns);
            if(null != tsSection)
            {
                clone.transform.SetSiblingIndex(tsSection.GetSiblingIndex());
            }
        }
    }//class PageView
}//namespace
