using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour
{
	public string NPCName;
	public Texture NPCPortrait;
	
	public GameObject PlayerCharacter;
	
	[SerializeField]
	public DialogueResponse DialogueShown;
	
	private DialogueResponse _originalDialogue;
	
	private bool _inRange = false;
	private bool _talking = false;
	private Rect talkWindowRect = new Rect(10, Screen.height - 200, 300, 300);
	
	private GUIStyle listStyle = new GUIStyle();
	private List<ComboBox> _comboBoxes = new List<ComboBox>();
	private List<GUIContent[]> _comboLists = new List<GUIContent[]>();
	private List<Rect> _comboRects = new List<Rect>();
	
	private GUIContent[] cbList_event = new GUIContent[] {
			new GUIContent("..."),	
			new GUIContent("I killed Joe"),
			new GUIContent("I ate the cake")
	};
	
	private GUIContent[] cbList_char = new GUIContent[] {
			new GUIContent("..."),	
			new GUIContent("Thomas"),
			new GUIContent("Joe")
	};
	
	private GUIContent[] cbList_item = new GUIContent[] {
			new GUIContent("..."),
			new GUIContent("Excalibur")
	};
	
	private GUIContent[] cbList_char_item = new GUIContent[] {
			new GUIContent("..."),	
			new GUIContent("Thomas"),
			new GUIContent("Joe"),
			new GUIContent("Excalibur")
	};
	
	private GUIContent[] cbList_place_group = new GUIContent[] {
			new GUIContent("..."),	
			new GUIContent("Copenhagen"),
			new GUIContent("The Raiders")
	};
	
	private GUIContent[] cbList_char_item_place_group = new GUIContent[] {
			new GUIContent("..."),	
			new GUIContent("Thomas"),
			new GUIContent("Joe"),
			new GUIContent("Excalibur"),
			new GUIContent("Copenhagen"),
			new GUIContent("The Raiders")
	};
	
	private GUIContent[] cbList_healer_shopkeeper = new GUIContent[] {
			new GUIContent("..."),	
			new GUIContent("healer"),
			new GUIContent("merchant"),
			new GUIContent("blacksmith"),
			new GUIContent("medic")
	};
	
	void Start()
	{
		_originalDialogue = DialogueShown;
		
		listStyle.normal.textColor = Color.black;
		listStyle.hover.textColor =
		listStyle.onHover.textColor = Color.red;
	    listStyle.padding.left =
	    listStyle.padding.right =
	    listStyle.padding.top =
	    listStyle.padding.bottom = 4;		
    	listStyle.normal.background =
		listStyle.onHover.background =
	    listStyle.hover.background = new Texture2D(2, 2);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Vector3.Distance(transform.position, PlayerCharacter.transform.position) < 2)
		{
			_inRange = true;
		}
		else
		{
			_inRange = false;
		}
	}
	
	void OnGUI()
	{
		if (_talking)
		{
			//talkWindowRect = GUI.Window (0, talkWindowRect, TalkWindow, "Talking to: " + NPCName);
			
			TalkWindow();
			
			for (int i = _comboBoxes.Count - 1; i >= 0; i--)
			{
				int selectedItemIndex = _comboBoxes[i].GetSelectedItemIndex();
				selectedItemIndex = _comboBoxes[i].List(_comboRects[i], _comboLists[i][selectedItemIndex].text, _comboLists[i], listStyle);
			}
		}
		else if (_inRange)
		{
			Vector3 screenPos = Camera.mainCamera.WorldToScreenPoint(transform.position);
			
			if (GUI.Button(new Rect(screenPos.x - 19, screenPos.y + 20, 38, 18), "Talk"))
			{
				_talking = true;
				PlayerCharacter.GetComponent<PlatformController>().ToggleOnOff();
			}
		}
	}
	
	void TalkWindow()
	{
		GUI.skin.box.alignment = TextAnchor.MiddleLeft;
		
		_comboBoxes = new List<ComboBox>();
		_comboRects = new List<Rect>();
		_comboLists = new List<GUIContent[]>();
		
		GUIStyle style = new GUIStyle(GUI.skin.box);
		style.normal.background = GUI.skin.window.onNormal.background;
		
		GUI.Box(talkWindowRect, "", style);
		
		int yCount = Response(DialogueShown, 0, 0);
		
		talkWindowRect = new Rect(10, Screen.height - yCount * 25 - 20, Screen.width - 20, yCount * 25 + 10);
	}
	
	int Response(DialogueResponse currentResponse, int xCount, int yCount)
	{
		GUI.DrawTexture(new Rect(15, talkWindowRect.y - 65, 50, 50), NPCPortrait);
		GUI.Box(new Rect(75, talkWindowRect.y - 52, GUI.skin.box.CalcSize(new GUIContent(NPCName + ": " + currentResponse.NPCResponse)).x, 25), NPCName + ": " + currentResponse.NPCResponse);
		
		foreach (DialogueNode dn in currentResponse.SubNodes)
		{
			DialogueNode newDN = new DialogueNode();
			newDN.FoldOut = true;
			newDN.SubNodes = new List<DialogueNode>() { dn };
			yCount--;
			yCount += Talk (newDN, xCount, yCount);
		}
		return yCount;
	}
	
	int Talk(DialogueNode currentNode, int xCount, int yCount)
	{
		if (currentNode.FoldOut)
		{
			yCount++;
			int subCount = 0;
			for (int i = 0; i < currentNode.SubNodes.Count; i++)
			{
				string nodeText = currentNode.SubNodes[i].Text;
				float xPos = 0;
				int selectedItemIndex = 1;
				
				if (nodeText.Contains("*character/item/place/group*"))
				{
					xPos = GUI.skin.box.CalcSize(new GUIContent(nodeText.Substring(0, nodeText.IndexOf('*')))).x;
					nodeText = nodeText.Replace("*character/item/place/group*", "                         ");
					_comboBoxes.Add(currentNode.SubNodes[i].comboBoxControl);
					_comboRects.Add(new Rect(35 + xPos + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 100, 25));
					_comboLists.Add(cbList_char_item_place_group);
					selectedItemIndex = currentNode.SubNodes[i].comboBoxControl.GetSelectedItemIndex();
				}
				else if (nodeText.Contains("*healer/shopkeeper/..*"))
				{
					xPos = GUI.skin.box.CalcSize(new GUIContent(nodeText.Substring(0, nodeText.IndexOf('*')))).x;
					nodeText = nodeText.Replace("*healer/shopkeeper/..*", "                         ");
					_comboBoxes.Add(currentNode.SubNodes[i].comboBoxControl);
					_comboRects.Add(new Rect(35 + xPos + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 100, 25));
					_comboLists.Add(cbList_healer_shopkeeper);
					selectedItemIndex = currentNode.SubNodes[i].comboBoxControl.GetSelectedItemIndex();
				}
				else if (nodeText.Contains("*character/item*"))
				{
					xPos = GUI.skin.box.CalcSize(new GUIContent(nodeText.Substring(0, nodeText.IndexOf('*')))).x;
					nodeText = nodeText.Replace("*character/item*", "                         ");
					_comboBoxes.Add(currentNode.SubNodes[i].comboBoxControl);
					_comboRects.Add(new Rect(35 + xPos + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 100, 25));
					_comboLists.Add(cbList_char_item);
					selectedItemIndex = currentNode.SubNodes[i].comboBoxControl.GetSelectedItemIndex();
				}
				else if (nodeText.Contains("*character*"))
				{
					xPos = GUI.skin.box.CalcSize(new GUIContent(nodeText.Substring(0, nodeText.IndexOf('*')))).x;
					nodeText = nodeText.Replace("*character*", "                         ");
					_comboBoxes.Add(currentNode.SubNodes[i].comboBoxControl);
					_comboRects.Add(new Rect(35 + xPos + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 100, 25));
					_comboLists.Add(cbList_char);
					selectedItemIndex = currentNode.SubNodes[i].comboBoxControl.GetSelectedItemIndex();
				}
				else if (nodeText.Contains("*item*"))
				{
					xPos = GUI.skin.box.CalcSize(new GUIContent(nodeText.Substring(0, nodeText.IndexOf('*')))).x;
					nodeText = nodeText.Replace("*item*", "                         ");
					_comboBoxes.Add(currentNode.SubNodes[i].comboBoxControl);
					_comboRects.Add(new Rect(35 + xPos + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 100, 25));
					_comboLists.Add(cbList_item);
					selectedItemIndex = currentNode.SubNodes[i].comboBoxControl.GetSelectedItemIndex();
				}
				else if (nodeText.Contains("*place/group*"))
				{
					xPos = GUI.skin.box.CalcSize(new GUIContent(nodeText.Substring(0, nodeText.IndexOf('*')))).x;
					nodeText = nodeText.Replace("*place/group*", "                         ");
					_comboBoxes.Add(currentNode.SubNodes[i].comboBoxControl);
					_comboRects.Add(new Rect(35 + xPos + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 100, 25));
					_comboLists.Add(cbList_place_group);
					selectedItemIndex = currentNode.SubNodes[i].comboBoxControl.GetSelectedItemIndex();
				}
				else if (nodeText.Contains("*event*"))
				{
					xPos = GUI.skin.box.CalcSize(new GUIContent(nodeText.Substring(0, nodeText.IndexOf('*')))).x;
					nodeText = nodeText.Replace("*event*", "                         ");
					_comboBoxes.Add(currentNode.SubNodes[i].comboBoxControl);
					_comboRects.Add(new Rect(35 + xPos + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 100, 25));
					_comboLists.Add(cbList_event);
					selectedItemIndex = currentNode.SubNodes[i].comboBoxControl.GetSelectedItemIndex();
				}
					
				string buttonText = "+";
				if (currentNode.SubNodes[i].FoldOut)
					buttonText = "-";
				if (currentNode.SubNodes[i].SubNodes.Count == 0)
					buttonText = ">";
				
				GUI.enabled = (selectedItemIndex != 0);
				
				if (GUI.Button(new Rect(5 + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, 25, 25), buttonText))
				{
					if (currentNode.SubNodes[i].SubNodes.Count == 0)
					{
						if (currentNode.SubNodes[i].Response.SubNodes.Count > 0)
						{
							DialogueShown = currentNode.SubNodes[i].Response;
						}
						else if (_originalDialogue != null)
						{
							DialogueShown = _originalDialogue;
						}
					}
					else
					{
						currentNode.SubNodes[i].FoldOut = !currentNode.SubNodes[i].FoldOut;
					}
					
					ComboBox.UnSelect();
				}
				
				GUI.enabled = true;
				
				GUI.Box(new Rect(35 + xCount * 20 + talkWindowRect.x, 5 + yCount * 25 + talkWindowRect.y, GUI.skin.box.CalcSize(new GUIContent(nodeText)).x, 25), nodeText);
				int result = Talk(currentNode.SubNodes[i], xCount + 1, yCount);
				
				yCount += result;
				subCount += result;
			}
		
			return subCount + 1;
		}
		return 1;
	}
}

[System.Serializable]
public class DialogueNode
{
	public string Text;
	public bool FoldOut;
	
	public ComboBox comboBoxControl = new ComboBox();
	
	[SerializeField]
	public List<DialogueNode> SubNodes = new List<DialogueNode>();
	
	[SerializeField]
	public DialogueResponse Response;
}

[System.Serializable]
public class DialogueResponse
{
	public string NPCResponse;
	
	[SerializeField]
	public List<DialogueNode> SubNodes = new List<DialogueNode>();
}