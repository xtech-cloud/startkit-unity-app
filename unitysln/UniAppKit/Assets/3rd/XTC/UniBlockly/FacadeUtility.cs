using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XTC.Blockly
{
	public class FacadeUtility
	{
		public static void DestroyActiveChildren(GameObject _gameobject)
		{
			List<GameObject> children = new List<GameObject>();
			foreach(Transform child in _gameobject.transform)
			{
				if(!child.gameObject.activeSelf)
					continue;
				children.Add(child.gameObject);
			}

			foreach(GameObject child in children)
			{
				GameObject.Destroy(child);
			}
		}

		public static Color HexToColor(string _hex)
        {
            if (string.IsNullOrEmpty(_hex))
            {
                return Color.white;
            }
            int colorInt = int.Parse(_hex, System.Globalization.NumberStyles.AllowHexSpecifier);
            float basenum = 255;

            int b = 0xFF & colorInt;
            int g = 0xFF00 & colorInt;
            g >>= 8;
            int r = 0xFF0000 & colorInt;
            r >>= 16;
            return new Color((float)r / basenum, (float)g / basenum, (float)b / basenum, 1);
        }

		public static void StretchText(UnityEngine.UI.Text _text)
		{
			TextGenerator generator = new TextGenerator();
			var settings = _text.GetGenerationSettings(Vector2.zero);
            settings.generateOutOfBounds = true;
			float offsetWidth = _text.fontSize/2 ;
			float width = generator.GetPreferredWidth(_text.text, settings) / _text.pixelsPerUnit + offsetWidth;
			RectTransform rt = _text.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
		}

		public static void StretchInput(InputField _input)
		{
			TextGenerator generator = new TextGenerator();
			UnityEngine.UI.Text textComponent = _input.textComponent;
			var settings = textComponent.GetGenerationSettings(Vector2.zero);
            settings.generateOutOfBounds = true;
			float offsetWidth = textComponent.fontSize/2 + Mathf.Abs(textComponent.GetComponent<RectTransform>().sizeDelta.x);
			float width = generator.GetPreferredWidth(_input.text, settings) / textComponent.pixelsPerUnit + offsetWidth;
			RectTransform rt = _input.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
		}

		public static void StretchDropdown(Dropdown _dropdown)
		{
			TextGenerator generator = new TextGenerator();
			UnityEngine.UI.Text textComponent = _dropdown.captionText;
			var settings = textComponent.GetGenerationSettings(Vector2.zero);
            settings.generateOutOfBounds = true;
			float offsetWidth = textComponent.fontSize/2 + Mathf.Abs(textComponent.GetComponent<RectTransform>().sizeDelta.x);
			float width = generator.GetPreferredWidth(textComponent.text, settings) / textComponent.pixelsPerUnit + offsetWidth;
			RectTransform rt = _dropdown.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
		}

		public static void StretchDropObject(DropObject _dropobject)
		{
			TextGenerator generator = new TextGenerator();
			Transform captionText = _dropobject.transform.Find("text");
			UnityEngine.UI.Text textComponent = captionText.GetComponent<UnityEngine.UI.Text>();
			var settings = textComponent.GetGenerationSettings(Vector2.zero);
            settings.generateOutOfBounds = true;
			float offsetWidth = textComponent.fontSize/2 + Mathf.Abs(textComponent.GetComponent<RectTransform>().sizeDelta.x);
			float width = generator.GetPreferredWidth(textComponent.text, settings) / textComponent.pixelsPerUnit + offsetWidth;
			RectTransform rt = _dropobject.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
		}
		
	}//class
}//namespace

