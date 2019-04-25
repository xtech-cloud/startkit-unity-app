using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using SimpleJSON;


namespace XTC.Blockly
{

    public class CustomElementModel : Model
    {
        public const string NAME = "CustomElementModel";

        
        public class CustomElement
        {
            public string name = "";
            public string type = "";
            public List<string> values = new List<string>(0);
        }

        public class CustomElementStatus : Model.Status
        {
            public const string NAME = "CustomElementStatus";
            public List<CustomElement> elements = new List<CustomElement>();
        }

        protected override void setup()
        {
            status_ = new CustomElementStatus();
        }

        protected override void dismantle()
        {

        }

		private CustomElementStatus status{
			get{
				return status_ as CustomElementStatus;
			}
		}

        public Error MergeEDL(string _edl)
        {
			Error err = Error.OK;
            try
            {
                JSONArray aryEDL = JSON.Parse(_edl).AsArray;
                foreach (JSONNode nEDL in aryEDL)
                {
					JSONClass cEDL = nEDL.AsObject;
                    CustomElement element = new CustomElement();
                    element.name = cEDL["name"].Value;
                    element.type = cEDL["type"].Value;
                    foreach (JSONNode nValue in cEDL["values"].AsArray)
					{
                        element.values.Add(nValue.Value);
					}
					status.elements.Add(element);
                }
            }
            catch (System.Exception e)
            {
				err = Error.NewException(e);
            }
			return err;
        }

    }//class GroupModel
}//namespace
