using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class BlockModel : Model
    {
        public const string NAME = "BlockModel";

        public class Parameter
        {
            public string type = "";
            public string name = "";
            public string defaultValue = "";
        }

        public class Element
        {
            public string type = "";
            public string value = "";
        }

        public class Block
        {
            public string method = "";
            public string ns = ""; //namespace
            public string color = "";

            public List<Parameter> parameters = new List<Parameter>();
            public List<List<Element>> elements = new List<List<Element>>();
        }

        public class BlockStatus : Model.Status
        {
            public const string NAME = "BlockStatus";
            public List<Block> blocks = new List<Block>();
        }

        protected override void setup()
        {
            status_ = new BlockStatus();
        }

        protected override void dismantle()
        {

        }

        private BlockStatus status
        {
            get
            {
                return status_ as BlockStatus;
            }
        }

        private BlockController controller
        {
            get
            {
                return controllerCenter_.FindController(BlockController.NAME) as BlockController;
            }
        }

        public Error MergeBDL(string _bdl)
        {
            Error err = Error.OK;
            try
            {
                JSONArray aryBDL = JSON.Parse(_bdl).AsArray;
                foreach (JSONNode nBDL in aryBDL)
                {
                    JSONClass cBDL = nBDL.AsObject;
                    Block block = new Block();
                    block.method = cBDL["method"].Value;
                    block.ns = cBDL["namespace"].Value;
                    block.color = cBDL["color"].Value;
                    foreach (JSONNode nParam in cBDL["parameters"].AsArray)
                    {
                        JSONClass cParam = nParam.AsObject;
                        Parameter param = new Parameter();
                        param.name = cParam["name"].Value;
                        param.type = cParam["type"].Value;
                        param.defaultValue = cParam["defaultValue"].Value;
                        block.parameters.Add(param);
                    }
                    foreach (JSONNode nElements in cBDL["elements"].AsArray)
                    {
                        List<Element> elements = new List<Element>();
                        foreach (JSONNode nElement in nElements.AsArray)
                        {
                            JSONClass cElement = nElement.AsObject;
                            Element element = new Element();
                            element.type = cElement["type"].Value;
                            element.value = cElement["value"].Value;
                            elements.Add(element);
                        }
                        block.elements.Add(elements);
                    }

                    status.blocks.Add(block);
                }
            }
            catch (System.Exception e)
            {
                err = Error.NewException(e);
            }
            return err;
        }

        public void FetchBlocks()
        {
            controller.RefreshBlocks(status);
        }

        public void SaveSyncColor()
        {
            GroupModel.GroupStatus groupStatus = status.Access(GroupModel.GroupStatus.NAME) as GroupModel.GroupStatus;

            foreach(Block block in status.blocks)
            {
                if(!string.IsNullOrEmpty(block.color))
                    continue;
                    
                GroupModel.Group group = groupStatus.groups.Find((_item)=>{
                    bool found = false;
                    foreach(GroupModel.Section section in _item.sections)
                    {
                        if(block.ns.Equals(section.path))
                            found = true;
                    }
                    return found;
                });

                if(null == group)
                    continue;

                block.color = group.color;
            }
        }

    }//class GroupModel
}//namespace
