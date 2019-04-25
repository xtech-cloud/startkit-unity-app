using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTC.Blockly
{
    public class BlockBuilder
    {

        public enum Symbol
        {
            Single,     // ;
            Left,       // {
            Middle,     // }{
            Right,      // }
            Blank,
        }

        public class ObjectElement
        {
            public string uuid = "";
            public string alias = "";
            public Sprite icon = null;
        }

        public delegate void OnInputUpdatedCallback(string _uuid, string _variant, string _value);
        public delegate void OnDropdownUpdatedCallback(string _uuid, string _variant, int _index);
        public delegate void OnDropObjectUpdatedCallback(string _uuid, string _variant, string _value);
        public static OnInputUpdatedCallback OnInputUpdated;
        public static OnDropdownUpdatedCallback OnDropdownUpdated;
        public static OnDropObjectUpdatedCallback OnDropObjectUpdated;

        public static Sprite imgBlockSingle = null;
        public static Sprite imgBlockHead = null;
        public static Sprite imgBlockMiddle = null;
        public static Sprite imgBlockFoot = null;
        public static Sprite imgBlockBlank = null;

        public static Transform templateElementText = null;
        public static Transform templateElementInput = null;
        public static Transform templateElementDropdown = null;
        public static Transform templateElementObject = null;

        private static Dictionary<string, ObjectElement> objectElements = new Dictionary<string, ObjectElement>();

        public static void AddObjectElement(string _uuid, string _alias, Sprite _icon)
        {
            if(objectElements.ContainsKey(_uuid))
                return;

            ObjectElement obj = new ObjectElement();
            obj.uuid = _uuid;
            obj.alias = _alias;
            obj.icon = _icon;
            objectElements[_uuid] = obj;
        }

        public static void RemoveObjectElement(string _uuid)
        {
            if(!objectElements.ContainsKey(_uuid))
                return;
            objectElements.Remove(_uuid);
        }

        public static void CleanObjectElements()
        {
            objectElements.Clear();
        }

        public static void BuildBlock(BlockModel.Block _block, GameObject _obj, CustomElementModel.CustomElementStatus _elementStatus)
        {
            Transform tsExpression = _obj.transform.Find("expression");
            Color color = FacadeUtility.HexToColor(_block.color);
            GameObject expressionClone = null;

            int line = _block.elements.Count;
            expressionClone = cloneExpression(tsExpression.gameObject, color, line > 1 ? imgBlockHead : imgBlockSingle, line == 1 ? Symbol.Single : Symbol.Left);
            decorateElements(expressionClone, _block.elements[0], new Dictionary<string, string>(), _elementStatus);

            for (int i = 1; i < _block.elements.Count; ++i)
            {
                Symbol symbol = Symbol.Blank;
                Sprite sprite = imgBlockBlank;
                // blank
                cloneExpression(tsExpression.gameObject, color, sprite, symbol);

                if (i == line - 1)
                {
                    // last one
                    symbol = Symbol.Right;
                    sprite = imgBlockFoot;
                }
                else
                {
                    symbol = Symbol.Middle;
                    sprite = imgBlockMiddle;
                }

                expressionClone = cloneExpression(tsExpression.gameObject, color, sprite, symbol);
                decorateElements(expressionClone, _block.elements[i], new Dictionary<string, string>(), _elementStatus);
            }

            Transform tsDrag = _obj.transform.Find("#drag");
            tsDrag.SetAsLastSibling();

            DragBlock drag = tsDrag.GetComponent<DragBlock>();
            drag.method = _block.method;
        }

        public static void BuildExpression(List<BlockModel.Element> _elements, Dictionary<string, string> _variants, GameObject _obj, CustomElementModel.CustomElementStatus _elementStatus, Symbol _symbol, string _color)
        {
            Color color = FacadeUtility.HexToColor(_color);
            Sprite sprite = imgBlockBlank;
            if (Symbol.Single == _symbol)
                sprite = imgBlockSingle;
            else if (Symbol.Left == _symbol)
                sprite = imgBlockHead;
            else if (Symbol.Right == _symbol)
                sprite = imgBlockFoot;
            else if (Symbol.Middle == _symbol)
                sprite = imgBlockMiddle;

            Image img = _obj.transform.Find("__img__").GetComponent<Image>();
            img.color = color;
            img.sprite = sprite;
            decorateElements(_obj, _elements, _variants, _elementStatus);
        }

        public static Symbol LineToSymbol(BlockModel.Block _block, int _line)
        {
            if (_block.elements.Count == 1)
                return Symbol.Single;

            if (_line == 0)
                return Symbol.Left;

            if (_line == _block.elements.Count - 1)
                return Symbol.Right;

            return Symbol.Middle;
        }

        private static GameObject cloneExpression(GameObject _expression, Color _color, Sprite _sprite, Symbol _symbol)
        {
            GameObject clone = GameObject.Instantiate(_expression);
            clone.name = SymbolToString(_symbol);
            clone.transform.SetParent(_expression.transform.parent);
            clone.transform.localScale = Vector3.one;
            clone.SetActive(true);
            Image img = clone.transform.Find("__img__").GetComponent<Image>();
            img.color = _color;
            img.sprite = _sprite;
            return clone;
        }

        private static void decorateElements(GameObject _expression, List<BlockModel.Element> _elements, Dictionary<string, string> _variants, CustomElementModel.CustomElementStatus _elementStatus)
        {
            RectTransform space = _expression.transform.Find("__space__").GetComponent<RectTransform>();
            space.gameObject.SetActive(_elements.Count == 0);

            foreach (BlockModel.Element element in _elements)
            {
                GameObject clone = null;
                if (element.type.Equals("text"))
                {
                    clone = GameObject.Instantiate(templateElementText.gameObject);
                    UnityEngine.UI.Text text = clone.GetComponent<UnityEngine.UI.Text>();
                    text.text = element.value;
                    FacadeUtility.StretchText(text);
                }
                else if (element.type.Equals("input"))
                {
                    clone = GameObject.Instantiate(templateElementInput.gameObject);
                    InputField input = clone.GetComponent<InputField>();
                    input.text = element.value;
                    if (_variants.ContainsKey(element.value))
                        input.text = _variants[element.value];
                    FacadeUtility.StretchInput(input);
                    input.onEndEdit.AddListener((_text) =>
                    {
                        if (null != OnInputUpdated)
                            OnInputUpdated(clone.transform.parent.name, element.value, _text);
                    });
                }
                else if (element.type.Equals("dropdown"))
                {
                    clone = GameObject.Instantiate(templateElementDropdown.gameObject);
                    Dropdown dropdown = clone.GetComponent<Dropdown>();
                    FacadeUtility.StretchDropdown(dropdown);
                    dropdown.onValueChanged.AddListener((_value) =>
                    {
                        if (null != OnDropdownUpdated)
                            OnDropdownUpdated(clone.transform.parent.name, element.value, dropdown.value);
                    });
                }
                else if (element.type.Equals("object"))
                {
                    clone = GameObject.Instantiate(templateElementObject.gameObject);
                    DropObject drop = clone.AddComponent<DropObject>();
                    FacadeUtility.StretchDropObject(drop);
                    if (_variants.ContainsKey(element.value))
                    {
                        string uuid = _variants[element.value];
                        if(!string.IsNullOrEmpty(uuid))
                        {
                            if(objectElements.ContainsKey(uuid))
                            {
                                ObjectElement oe = objectElements[uuid];
                                drop.captionText.text = oe.alias;
                                drop.imgIcon.gameObject.SetActive(true);
                                drop.imgIcon.sprite = oe.icon;
                            }
                        }
                    }
                    drop.onObjectDrop = (_dragObject) =>
                    {
                        if (null != OnDropObjectUpdated)
                            OnDropObjectUpdated(clone.transform.parent.name, element.value, _dragObject.name);
                    };
                }
                else
                {
                    clone = buildCustomElement(element.type, element.value, _variants, _elementStatus);
                }

                if (null == clone)
                    continue;

                clone.name = element.value;
                clone.transform.SetParent(_expression.transform);
                clone.transform.localScale = Vector3.one;
            }
        }

        private static GameObject buildCustomElement(string _type, string _value, Dictionary<string, string> _variants, CustomElementModel.CustomElementStatus _elementStatus)
        {
            CustomElementModel.CustomElement element = _elementStatus.elements.Find((_item) =>
            {
                return _item.name.Equals(_type);
            });

            if (null == element)
                return null;

            GameObject clone = null;
            if (element.type.Equals("dropdown"))
            {
                clone = GameObject.Instantiate(templateElementDropdown.gameObject);
                Dropdown dd = clone.GetComponent<Dropdown>();
                dd.options.Clear();
                foreach (string value in element.values)
                    dd.options.Add(new Dropdown.OptionData(value));

                if (element.values.Count > 0)
                    dd.captionText.text = element.values[0];
                FacadeUtility.StretchDropdown(dd);
                dd.onValueChanged.AddListener((_ddValue) =>
                {
                    if (null != OnDropdownUpdated)
                        OnDropdownUpdated(clone.transform.parent.name, _value,  dd.value);
                });
            }
            return clone;
        }

        private static string SymbolToString(Symbol _symbol)
        {
            if (Symbol.Single == _symbol)
                return ";";

            if (Symbol.Left == _symbol)
                return "{";

            if (Symbol.Right == _symbol)
                return "}";

            if (Symbol.Middle == _symbol)
                return "}{";

            return "";
        }
    }

}//namespace
