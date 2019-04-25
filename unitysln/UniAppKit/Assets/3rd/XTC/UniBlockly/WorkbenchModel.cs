using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

using XTC.Types;
using XTC.MVCS;
using LitJson;


namespace XTC.Blockly
{

    public class WorkbenchModel : Model
    {
        public const string NAME = "WorkbenchModel";

        
        public class WorkbenchStatus : Model.Status
        {
        }

        protected override void setup()
        {
            status_ = new WorkbenchStatus();
        }

        protected override void dismantle()
        {

        }

        private WorkbenchStatus status
        {
            get
            {
                return status_ as WorkbenchStatus;
            }
        }

        private WorkbenchController controller
        {
            get
            {
                return controllerCenter_.FindController(WorkbenchController.NAME) as WorkbenchController;
            }
        }

        private PageModel modelPage
        {
            get
            {
                return center_.FindModel(PageModel.NAME) as PageModel;
            }
        }

        public void UpdateNewUnit(string _method, int _depth)
        {
            PageModel.Unit unit = new PageModel.Unit();
            unit.uuid = newUUID();
            unit.block = _method;
            unit.depth = _depth;
            modelPage.UpdateAddUnit(unit);
        }

        private string newUUID()
        {
            string guid = Guid.NewGuid().ToString();
            return toUUID(guid);
        }

        private string toUUID(string _text)
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
