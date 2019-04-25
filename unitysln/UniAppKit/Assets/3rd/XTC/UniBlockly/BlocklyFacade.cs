using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;
using XTC.Blockly;

[System.Serializable]
public class WorkbenchUI
{
	public GameObject root;
	public Transform board;
	public Transform container;
	public Transform templateExpression;

}

[System.Serializable]
public class BlocklyUI
{
	public string blankPageName = "";
	public GameObject root;
	public Button btnNewPage;

	public Button btnDeletePage;
	public InputField inputPageName;
	public Transform pageTools;
	public Toggle tgRunAwake;
	public Button btnFoldPageTools;


	public Transform tsTempaltePage;
	public Transform tsTempalteGroup;
	public Transform tsTempalteSection;
	public Transform tsTempalteBlock;

	public Transform rootBlockBar;
	public Transform rootVariantBar;

	public Button btnFold;
	public Button btnSearchBlock;
	public Toggle tgVariant;

	public Sprite imgBlockBlank;
	public Sprite imgBlockFoot;
	public Sprite imgBlockHead;
	public Sprite imgBlockMiddle;
	public Sprite imgBlockSingle;

	public Transform templateElementText;
	public Transform templateElementInput;
	public Transform templateElementDropdown;
	public Transform templateElementObject;

}

public class BlocklyFacade : UIFacade
{
	public const string NAME = "BlocklyFacade";
    public BlocklyUI uiBlockly;
	public WorkbenchUI uiWorkbench;

	void Awake()
	{
		BlockBuilder.imgBlockHead = uiBlockly.imgBlockHead;
		BlockBuilder.imgBlockMiddle = uiBlockly.imgBlockMiddle;
		BlockBuilder.imgBlockFoot = uiBlockly.imgBlockFoot;
		BlockBuilder.imgBlockSingle = uiBlockly.imgBlockSingle;
		BlockBuilder.imgBlockBlank = uiBlockly.imgBlockBlank;

		BlockBuilder.templateElementText = uiBlockly.templateElementText;
		BlockBuilder.templateElementInput = uiBlockly.templateElementInput;
		BlockBuilder.templateElementDropdown = uiBlockly.templateElementDropdown;
		BlockBuilder.templateElementObject = uiBlockly.templateElementObject;

	}
}
