using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class PageModel : Model
    {
        public const string NAME = "PageModel";

        public const string PropertyActiveUUID = "active.uuid";

        public delegate void PageCallback(Page _page);
        public PageCallback OnNewPage;
        public PageCallback OnUpdatePage;
        public PageCallback OnDeletePage;
        public PageCallback OnActivatePage;
        public PageCallback OnUnitsSync;

        public delegate void SyncCallback(List<Page> _pages);
        public SyncCallback OnSync;

        public class Unit
        {
            public string uuid = "";
            public string block = "";
            public int depth = 0;
            public Dictionary<string, string> variants = new Dictionary<string, string>();
        }

        public class Profile
        {
            public string name = "";
            public bool runAwake = true;
        }

        public class Page
        {
            public string uuid = "";
            public Profile profile = new Profile();
            public List<Unit> units = new List<Unit>();
        }

        public class PageStatus : Model.Status
        {
            public List<Page> pages = new List<Page>();
        }

        public WorkbenchController controllerWorkbench
        {
            get
            {
                return controllerCenter_.FindController(WorkbenchController.NAME) as WorkbenchController;
            }
        }

        protected override void setup()
        {
            status_ = new PageStatus();
            property[PropertyActiveUUID] = "";
        }

        protected override void dismantle()
        {

        }

        private PageStatus status
        {
            get
            {
                return status_ as PageStatus;
            }
        }

        private PageController controller
        {
            get
            {
                return controllerCenter_.FindController(PageController.NAME) as PageController;
            }
        }

        public string NameToUUID(string _pageName)
        {
            Page page = status.pages.Find((_item)=>{
                return _item.profile.name.Equals(_pageName);
            });
            if(null == page)
                return "";
            return page.uuid;
        }

        public void UpdateNewPage()
        {
            Page page = new Page();
            page.uuid = newUUID();

            status.pages.Add(page);

            controller.RefreshNewPage(page);

            if (null != OnNewPage)
                OnNewPage(page);
            if (null != OnSync)
                OnSync(status.pages);
        }

        public void UpdatePages(List<Page> _pages)
        {
            status.pages.Clear();
            status.pages.AddRange(_pages);
            controller.RefreshPages(_pages);

            if (null != OnSync)
                OnSync(status.pages);
        }

        public void UpdateDeletePage(string _uuid)
        {
            Page page = status.pages.Find((_item) =>
            {
                return _item.uuid.Equals(_uuid);
            });

            if (null == page)
                return;

            status.pages.Remove(page);
            controller.RefreshDeletePage(page);

            if (null != OnDeletePage)
                OnDeletePage(page);
            if (null != OnSync)
                OnSync(status.pages);

            property[PropertyActiveUUID] = "";
        }

        public void UpdateOpenPageTools(string _uuid)
        {
            Page page = status.pages.Find((_item) =>
            {
                return _item.uuid.Equals(_uuid);
            });

            if (null == page)
                return;

            controller.OpenTools(page);
        }

        public void UpdateRenamePage(string _uuid, string _name)
        {
            Page page = status.pages.Find((_item) =>
            {
                return _item.uuid.Equals(_uuid);
            });

            if (null == page)
                return;

            page.profile.name = _name;
            controller.RefreshPage(page);

            if (null != OnUpdatePage)
                OnUpdatePage(page);
            if (null != OnSync)
                OnSync(status.pages);
        }

        public void UpdateRunAwakePage(string _uuid, bool _isRunAwake)
        {
            Page page = status.pages.Find((_item) =>
            {
                return _item.uuid.Equals(_uuid);
            });

            if (null == page)
                return;

            page.profile.runAwake = _isRunAwake;

            if (null != OnUpdatePage)
                OnUpdatePage(page);
        }

        public void UpdateActivePage(string _uuid)
        {
            property[PropertyActiveUUID] = _uuid;

            Page page = status.pages.Find((_item) =>
            {
                return _item.uuid.Equals(_uuid);
            });

            controller.ActivatePage(status, page);

            if (null != OnActivatePage)
                OnActivatePage(page);
        }

        public void UpdateAddUnit(Unit _unit)
        {
            string activePage = (string)property[PropertyActiveUUID];
            Page page = status.pages.Find((_item) =>
            {
                return _item.uuid.Equals(activePage);
            });

            if (null == page)
                return;

            page.units.Add(_unit);
            controllerWorkbench.AddUnit(status, _unit);

            if (null != OnUnitsSync)
                OnUnitsSync(page);
        }

        public void UpdateVariantValue(string _uuid, string _variant, string _text)
        {
            string activePage = (string)property[PropertyActiveUUID];
            Page page = status.pages.Find((_item) =>
            {
                return _item.uuid.Equals(activePage);
            });

            if (null == page)
                return;

            Unit unit = page.units.Find((_item) =>
            {
                return _item.uuid.Equals(_uuid);
            });
            if (null == page)
                return;

            unit.variants[_variant] = _text;

            if (null != OnUnitsSync)
                OnUnitsSync(page);
        }
        

        private static string newUUID()
        {
            string guid = System.Guid.NewGuid().ToString();
            return toUUID(guid);
        }

        private static string toUUID(string _text)
        {
            MD5 md5 = MD5.Create();
            byte[] byteOld = Encoding.UTF8.GetBytes(_text);
            byte[] byteNew = md5.ComputeHash(byteOld);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }//class GroupModel
}//namespace
