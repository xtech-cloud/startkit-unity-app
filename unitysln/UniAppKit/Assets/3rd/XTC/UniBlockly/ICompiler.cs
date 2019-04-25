using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTC.Blockly
{
    public interface ICompiler
    {
        string Compile(string _dump);
    }
}//namespace
