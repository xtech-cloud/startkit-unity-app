using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XTC.MVCS;

namespace XTC.Blockly
{
    public class PageView : View
    {
        public const string NAME = "PageView";

        private string activePage { get; set; }

        private BlocklyUI uiBlockly
        {
            get
            {
                return (UIFacade.Find(BlocklyFacade.NAME) as BlocklyFacade).uiBlockly;
            }
        }
        protected override void setup()
        {
            uiBlockly.pageTools.gameObject.SetActive(false);
        }
        protected override void bindEvents()
        {
            uiBlockly.btnNewPage.onClick.AddListener(onNewPageClick);
            uiBlockly.btnDeletePage.onClick.AddListener(onDeletePageClick);
            uiBlockly.tgRunAwake.onValueChanged.AddListener(onRunAwakeChanged);
            uiBlockly.btnFoldPageTools.onClick.AddListener(onFoldPageToolsClick);
            uiBlockly.inputPageName.onEndEdit.AddListener(onRenamePageSubmit);
        }

        protected override void unbindEvents()
        {
            uiBlockly.btnNewPage.onClick.RemoveListener(onNewPageClick);
            uiBlockly.btnDeletePage.onClick.RemoveListener(onDeletePageClick);
            uiBlockly.tgRunAwake.onValueChanged.RemoveListener(onRunAwakeChanged);
            uiBlockly.inputPageName.onEndEdit.RemoveListener(onRenamePageSubmit);
            uiBlockly.btnFoldPageTools.onClick.RemoveListener(onFoldPageToolsClick);
        }

        protected override void dismantle()
        {

        }

        private PageModel model
        {
            get
            {
                return modelCenter_.FindModel(PageModel.NAME) as PageModel;
            }
        }

        public void RefreshNewPage(PageModel.Page _page)
        {
            addPage(_page);
        }

        public void RefreshDeletePage(PageModel.Page _page)
        {
            removePage(_page);
        }

        public void RefreshPages(List<PageModel.Page> _pages)
        {
            destroyActiveChildren(uiBlockly.tsTempaltePage.parent.gameObject);
            uiBlockly.pageTools.gameObject.SetActive(false);
            foreach (PageModel.Page page in _pages)
            {
                addPage(page);
            }
        }

        public void RefreshPage(PageModel.Page _page)
        {
            refreshPage(_page);
        }

        public void ActivatePage(PageModel.Page _page)
        {
            activePage = _page.uuid;
            uiBlockly.pageTools.gameObject.SetActive(false);
        }

        public void OpenTools(PageModel.Page _page)
        {
            uiBlockly.pageTools.gameObject.SetActive(true);
            uiBlockly.inputPageName.text = _page.profile.name;
            uiBlockly.tgRunAwake.isOn = _page.profile.runAwake;
        }

        private void addPage(PageModel.Page _page)
        {
            if (null == _page)
                return;

            GameObject clone = GameObject.Instantiate(uiBlockly.tsTempaltePage.gameObject);
            clone.transform.SetParent(uiBlockly.tsTempaltePage.parent);
            clone.transform.localScale = Vector3.one;
            clone.name = _page.uuid;
            clone.SetActive(true);

            Button btnMore = clone.transform.Find("btnMore").GetComponent<Button>();
            btnMore.onClick.AddListener(() =>
            {
                model.UpdateOpenPageTools(clone.name);
            });
            btnMore.gameObject.SetActive(false);

            Toggle toggle = clone.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((_toggled) =>
            {
                btnMore.gameObject.SetActive(_toggled);
                if (!_toggled)
                    return;
                model.UpdateActivePage(clone.name);
            });

            refreshPage(_page);
        }

        private void refreshPage(PageModel.Page _page)
        {
            if (null == _page)
                return;

            Transform target = uiBlockly.tsTempaltePage.parent.Find(_page.uuid);
            if (null == target)
                return;

            UnityEngine.UI.Text name = target.Find("txtName").GetComponent<UnityEngine.UI.Text>();
            if (string.IsNullOrEmpty(_page.profile.name))
                name.text = uiBlockly.blankPageName;
            else
                name.text = _page.profile.name;
        }

        private void removePage(PageModel.Page _page)
        {
            if (null == _page)
                return;

            Transform target = uiBlockly.tsTempaltePage.parent.Find(_page.uuid);
            if (null == target)
                return;

            GameObject.Destroy(target.gameObject);
        }


        private void onNewPageClick()
        {
            model.UpdateNewPage();
        }

        private void onDeletePageClick()
        {
            uiBlockly.pageTools.gameObject.SetActive(false);
            model.UpdateDeletePage(activePage);
            activePage = "";
        }

        private void onRunAwakeChanged(bool _toggled)
        {
            model.UpdateRunAwakePage(activePage, _toggled);
        }

        private void onRenamePageSubmit(string _text)
        {
            model.UpdateRenamePage(activePage, _text);
        }

        private void onFoldPageToolsClick()
        {
            uiBlockly.pageTools.gameObject.SetActive(false);
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
