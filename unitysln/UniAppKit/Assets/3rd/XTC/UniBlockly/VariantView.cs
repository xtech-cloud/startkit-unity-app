using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

namespace XTC.Blockly
{
    public class VariantView : View
    {
        public const string NAME = "VariantView";

        private BlocklyUI uiBlockly
        {
            get
            {
                return (UIFacade.Find(BlocklyFacade.NAME) as BlocklyFacade).uiBlockly;
            }
        }

        protected override void setup()
        {
            uiBlockly.rootVariantBar.gameObject.SetActive(false);
        }
        protected override void bindEvents()
        {
            uiBlockly.tgVariant.onValueChanged.AddListener(onVariantChanged);
        }

        protected override void unbindEvents()
        {
            uiBlockly.tgVariant.onValueChanged.RemoveListener(onVariantChanged);
        }

        protected override void dismantle()
        {

        }

        public bool active {
            set{
                uiBlockly.rootVariantBar.gameObject.SetActive(value);
            }
        }

		private void onVariantChanged(bool _toggled)
		{
            active = _toggled;
		}
    }//class PageView
}//namespace
