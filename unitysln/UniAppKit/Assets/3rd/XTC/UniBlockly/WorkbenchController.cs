using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class WorkbenchController : Controller
    {
        public const string NAME = "WorkbenchController";

        private WorkbenchView view{
            get{
                return viewCenter_.FindView(WorkbenchView.NAME) as WorkbenchView;
            }
        }

        private PageModel modelPage{
            get{
                return modelCenter_.FindModel(PageModel.NAME) as PageModel;
            }
        }

        protected override void setup()
        {
            BlockBuilder.OnInputUpdated = OnInputUpdated;
            BlockBuilder.OnDropObjectUpdated = OnDropObjectUpdated;
        }

        public void Clean()
        {
            view.Clean();
        }

        public void AddUnit(PageModel.PageStatus _status, PageModel.Unit _unit)
        {
            view.RefreshAddUnit(_status, _unit);
        }

        public void AddUnits(PageModel.PageStatus _status, List<PageModel.Unit> _units)
        {
            view.RefreshAddUnits(_status, _units);
        }

        private void OnInputUpdated(string _uuid, string _variant, string _text)
        {
            modelPage.UpdateVariantValue(_uuid, _variant, _text);
        }

        private void OnDropObjectUpdated(string _uuid, string _variant, string _text)
        {
            modelPage.UpdateVariantValue(_uuid, _variant, _text);
        }

    }//class GroupModel
}//namespace
