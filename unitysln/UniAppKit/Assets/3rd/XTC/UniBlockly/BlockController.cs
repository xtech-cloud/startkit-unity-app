using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class BlockController : Controller
    {
        public const string NAME = "BlockController";

        private BlockView view{
            get{
                return viewCenter_.FindView(BlockView.NAME) as BlockView;
            }
        }

        public void RefreshBlocks(BlockModel.BlockStatus _status)
        {
            view.RefreshBlocks(_status);
        }
    }//class GroupModel
}//namespace
