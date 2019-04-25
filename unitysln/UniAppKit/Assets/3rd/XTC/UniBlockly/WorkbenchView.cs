using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

namespace XTC.Blockly
{
    public class WorkbenchView : View
    {
        public const string NAME = "WorkbenchView";

        private WorkbenchUI uiWorkbench
        {
            get
            {
                return (UIFacade.Find(BlocklyFacade.NAME) as BlocklyFacade).uiWorkbench;
            }
        }

        private WorkbenchModel model
        {
            get
            {
                return modelCenter_.FindModel(WorkbenchModel.NAME) as WorkbenchModel;
            }
        }

        private PageModel modelPage
        {
            get
            {
                return modelCenter_.FindModel(PageModel.NAME) as PageModel;
            }
        }

        protected override void setup()
        {
        }
        protected override void bindEvents()
        {
            DropBlock dropBlock = uiWorkbench.board.GetComponent<DropBlock>();
            dropBlock.onBlockDrop += onBlockDrop;
        }

        protected override void unbindEvents()
        {
            // TODO
            // how to fix the exception
            // MissingReferenceException: The object of type 'RectTransform' has been destroyed but you are still trying to access it.
            //DropBlock dropBlock = uiWorkbench.board.GetComponent<DropBlock>();
            //dropBlock.onBlockDrop -= onBlockDrop;
        }

        protected override void dismantle()
        {

        }

        public void Clean()
        {
            destroyActiveChildren(uiWorkbench.templateExpression.parent.gameObject);
        }

        public void RefreshAddUnit(PageModel.PageStatus _status, PageModel.Unit _unit)
        {
            BlockModel.BlockStatus statusBlock = _status.Access(BlockModel.BlockStatus.NAME) as BlockModel.BlockStatus;
            CustomElementModel.CustomElementStatus statusElement = statusBlock.Access(CustomElementModel.CustomElementStatus.NAME) as CustomElementModel.CustomElementStatus;
            BlockModel.Block block =  statusBlock.blocks.Find((_item)=>{
                return _item.method.Equals(_unit.block);
            });
            addBlock(_unit.uuid, block, _unit.variants, statusElement);
        }

        public void RefreshAddUnits(PageModel.PageStatus _status, List<PageModel.Unit> _units)
        {
            destroyActiveChildren(uiWorkbench.templateExpression.parent.gameObject);
            BlockModel.BlockStatus statusBlock = _status.Access(BlockModel.BlockStatus.NAME) as BlockModel.BlockStatus;
            CustomElementModel.CustomElementStatus statusElement = statusBlock.Access(CustomElementModel.CustomElementStatus.NAME) as CustomElementModel.CustomElementStatus;
            foreach (PageModel.Unit unit in _units)
            {
                BlockModel.Block block =  statusBlock.blocks.Find((_item)=>{
                    return _item.method.Equals(unit.block);
                });
                addBlock(unit.uuid, block, unit.variants, statusElement);
            }
        }

        private void onBlockDrop(DragBlock _block)
        {
            model.UpdateNewUnit(_block.method, 0);
        }

        private void addBlock(string _uuid, BlockModel.Block _block, Dictionary<string, string> _variants, CustomElementModel.CustomElementStatus _elementStatus)
        {
            if(null == _block)
                return;
                
            BlockBuilder.Symbol symbol = BlockBuilder.LineToSymbol(_block, 0);
            addExpression(_uuid, _block, _variants, _elementStatus, _block.elements[0], symbol);
            for (int line = 1; line < _block.elements.Count; ++line)
            {
                addExpression(_uuid, _block, _variants, _elementStatus, new List<BlockModel.Element>(0), BlockBuilder.Symbol.Blank);
                symbol = BlockBuilder.LineToSymbol(_block, line);
                addExpression(_uuid, _block, _variants, _elementStatus, _block.elements[line], symbol);
            }
        }

        private void addExpression(string _uuid, BlockModel.Block _block, Dictionary<string, string> _variants, CustomElementModel.CustomElementStatus _elementStatus,
            List<BlockModel.Element> _elements, BlockBuilder.Symbol _symbol)
        {
            GameObject clone = GameObject.Instantiate(uiWorkbench.templateExpression.gameObject);
            clone.transform.SetParent(uiWorkbench.templateExpression.parent);
            clone.transform.localScale = Vector3.one;
            clone.SetActive(true);
            clone.name = _uuid;
            BlockBuilder.BuildExpression(_elements, _variants, clone, _elementStatus, _symbol, _block.color);
        }


        private static void destroyActiveChildren(GameObject _gameobject)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in _gameobject.transform)
            {
                if (!child.gameObject.activeSelf)
                    continue;
                children.Add(child.gameObject);
            }

            foreach (GameObject child in children)
            {
                GameObject.Destroy(child);
            }
        }

    }//class PageView
}//namespace
