using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class GroupModel : Model
    {
        public const string NAME = "GroupModel";

        public class Section
        {
            public string name = "";
            public string path = "";
        }
        public class Group
        {
            public string name = "";
            public string color = "";
            public List<Section> sections = new List<Section>(0);
        }

        public class GroupStatus : Model.Status
        {
            public const string NAME = "GroupStatus";
            public List<Group> groups = new List<Group>();
            public Group activeGroup = null;
        }

        protected override void setup()
        {
            status_ = new GroupStatus();
        }

        protected override void dismantle()
        {

        }

		private GroupStatus status{
			get{
				return status_ as GroupStatus;
			}
		}

		private GroupController controller{
			get {
				return controllerCenter_.FindController(GroupController.NAME) as GroupController;
			}
		}

        public Error MergeGDL(string _gdl)
        {
			Error err = Error.OK;
            try
            {
                JSONArray aryGDL = JSON.Parse(_gdl).AsArray;
                foreach (JSONNode nGDL in aryGDL)
                {
					JSONClass cGDL = nGDL.AsObject;
                    Group group = new Group();
                    group.name = cGDL["name"].Value;
                    group.color = cGDL["color"].Value;
                    foreach (JSONNode nSection in cGDL["sections"].AsArray)
					{
                        Section section = new Section();
                        section.name = nSection.Value;
                        section.path = string.Format("{0}/{1}", group.name, section.name);
                        group.sections.Add(section);
					}
					
					status.groups.Add(group);
                }
            }
            catch (System.Exception e)
            {
				err = Error.NewException(e);
            }
			return err;
        }

		public void FetchGroups()
		{
			controller.RefreshGroups(status);
		}

        public void FetchSections()
		{
			controller.RefreshSections(status);
		}

        public void UpdateFilterSections(string _group)
        {
            //controller.RefreshSections(status, _group);
        }

/*
        public void SaveActiveGroup(string _group)
        {
             GroupModel.Group group = status.groups.Find((_item)=>{
                return _item.name.Equals(_group);
            });

            if(null == group)
                return;

            status.activeGroup = group;
        }
         */
    }//class GroupModel
}//namespace
