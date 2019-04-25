using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace XTC.Blockly
{
    public class LuaCompiler : ICompiler
    {
        private class SyntaxNode
        {

            public PageModel.Unit unit = null;
            public SyntaxNode parent = null;
            public List<SyntaxNode> children = new List<SyntaxNode>();
        }

        public string Compile(string _dump)
        {
            List<PageModel.Unit> units = JsonMapper.ToObject<List<PageModel.Unit>>(_dump);
            if (null == units)
                return "";

            List<string> requires = new List<string>();
            // key is depth
            Dictionary<int, SyntaxNode> parents = new Dictionary<int, SyntaxNode>();
            SyntaxNode root = new SyntaxNode();
            // the parent of depth 0 is root node
            parents[0] = root;
            SyntaxNode latest = root;


            int depth = 0;
            foreach (PageModel.Unit unit in units)
            {
                if (!requires.Contains(unit.block))
                    requires.Add(unit.block);

                if (unit.depth > depth)
                {
                    depth = unit.depth;
                    parents[depth] = latest;
                }
                else if (unit.depth < depth)
                {
                    depth = unit.depth;
                }
                SyntaxNode current = new SyntaxNode();
                current.unit = unit;
                parents[depth].children.Add(current);
                latest = current;
            }

            StringBuilder sb = new StringBuilder();
            foreach (string require in requires)
            {
                sb.AppendLine(string.Format("require('svm/{0}')", require));
            }
            sb.AppendLine("");
            sb.AppendLine("function run()");
            compileSyntaxNode(root, sb);
            sb.AppendLine("end");
            string code = sb.ToString();
            return code;
        }

        private void compileSyntaxNode(SyntaxNode _node, StringBuilder _builder)
        {
            if (null != _node.unit)
            {
                for (int i = 0; i < _node.unit.depth; i++)
                    _builder.Append("    ");
                _builder.Append(string.Format("{0}.Execute({1})\n", _node.unit.block, formatParameters(_node.unit.variants)));
            }

            foreach (SyntaxNode child in _node.children)
                compileSyntaxNode(child, _builder);
        }

        private string formatParameters(Dictionary<string, string> _paramters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (KeyValuePair<string, string> pair in _paramters)
            {
                sb.Append(string.Format("{0}=\"{1}\",", pair.Key, pair.Value));
            }
            sb.Append("}");
            return sb.ToString();
        }

    }
}//namespace

