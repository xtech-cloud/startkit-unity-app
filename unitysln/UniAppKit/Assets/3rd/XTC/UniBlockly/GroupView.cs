using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

namespace XTC.Blockly
{
    public class GroupView : View
    {
        public const string NAME = "GroupView";

        private BlocklyUI uiBlockly
        {
            get
            {
                return (UIFacade.Find(BlocklyFacade.NAME) as BlocklyFacade).uiBlockly;
            }
        }

        private ToggleGroup toggleGroup {get;set;}

        protected override void setup()
        {
            toggleGroup = uiBlockly.tsTempalteGroup.GetComponent<Toggle>().group;
			uiBlockly.tsTempalteGroup.gameObject.SetActive(false);
			uiBlockly.tsTempalteSection.gameObject.SetActive(false);
            uiBlockly.rootBlockBar.gameObject.SetActive(false);
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

        private GroupModel model
        {
            get
            {
                return modelCenter_.FindModel(GroupModel.NAME) as GroupModel;
            }
        }


        public void RefreshGroups(GroupModel.GroupStatus _status)
        {
            foreach (GroupModel.Group group in _status.groups)
            {
                addGroup(group);
            }
        }

		public void RefreshSections(GroupModel.GroupStatus _status)
		{
            foreach (GroupModel.Group group in _status.groups)
            {
				foreach(GroupModel.Section section in group.sections)
				{
					addSection(section);
				}
            }
		}

        private void addGroup(GroupModel.Group _group)
        {
            GameObject clone = GameObject.Instantiate(uiBlockly.tsTempalteGroup.gameObject);
            clone.transform.SetParent(uiBlockly.tsTempalteGroup.parent);
            clone.transform.localScale = Vector3.one;
            clone.SetActive(true);
            clone.name = _group.name;

			Image icon = clone.transform.Find("icon").GetComponent<Image>();
            icon.color = FacadeUtility.HexToColor(_group.color);

			UnityEngine.UI.Text name = clone.transform.Find("name").GetComponent<UnityEngine.UI.Text>();
            name.text = _group.name;

            Toggle toggle = clone.GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(
				(_toggled)=>{
					onGroupChanged(_toggled, toggle);
				}
			);
        }

		private void addSection(GroupModel.Section _section)
		{
			GameObject clone = GameObject.Instantiate(uiBlockly.tsTempalteSection.gameObject);
            clone.transform.SetParent(uiBlockly.tsTempalteSection.parent);
            clone.transform.localScale = Vector3.one;
            clone.SetActive(true);
            clone.name = _section.path;

			UnityEngine.UI.Text name = clone.transform.Find("name").GetComponent<UnityEngine.UI.Text>();
            name.text = _section.name;

			Toggle toggle = clone.GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(
				(_toggled)=>{
					onSectionChanged(_toggled, toggle);
				}
			);
		}

		private void onGroupChanged(bool _toggled, Toggle _sender)
		{
            (center_.FindView(BlockView.NAME) as BlockView).active = _toggled;

            if(!_toggled)
                return;

            model.UpdateFilterSections(_sender.name);
		}

		private void onSectionChanged(bool _toggled, Toggle _sender)
		{
		}

    }//class PageView
}//namespace
