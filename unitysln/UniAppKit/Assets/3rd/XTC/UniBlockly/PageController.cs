using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class PageController : Controller
    {
        public const string NAME = "PageController";

        private PageView view
        {
            get
            {
                return viewCenter_.FindView(PageView.NAME) as PageView;
            }
        }

        private WorkbenchModel modelWorkbench{
            get{
                return modelCenter_.FindModel(WorkbenchModel.NAME) as WorkbenchModel;
            }
        }

        private WorkbenchController controllerWorkbench{
            get{
                return center_.FindController(WorkbenchController.NAME) as WorkbenchController;
            }
        }

        public void RefreshNewPage(PageModel.Page _page)
        {
            view.RefreshNewPage(_page);
        }

        public void RefreshPages(List<PageModel.Page> _pages)
        {
            view.RefreshPages(_pages);
            controllerWorkbench.Clean();
        }

        public void RefreshDeletePage(PageModel.Page _page)
        {
            view.RefreshDeletePage(_page);
            controllerWorkbench.Clean();
        }

        public void RefreshPage(PageModel.Page _page)
        {
            view.RefreshPage(_page);
        }

        public void OpenTools(PageModel.Page _page)
        {
            view.OpenTools(_page);
        }

        public void ActivatePage(PageModel.PageStatus _status, PageModel.Page _page)
        {
            view.ActivatePage(_page);
            controllerWorkbench.AddUnits(_status, _page.units);
        }

    }//class GroupModel
}//namespace
