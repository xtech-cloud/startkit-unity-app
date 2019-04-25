using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class GroupController : Controller
    {
        public const string NAME = "GroupController";

        private GroupView view{
            get{
                return viewCenter_.FindView(GroupView.NAME) as GroupView;
            }
        }

        public void RefreshGroups(GroupModel.GroupStatus _status)
        {
            view.RefreshGroups(_status);
        }

        public void RefreshSections(GroupModel.GroupStatus _status)
        {
            view.RefreshSections(_status);
        }

    }//class GroupModel
}//namespace
