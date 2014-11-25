using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour 
{
	public GameObject player;

	public InventoryItem[] Inventory = new InventoryItem[Game.MAXINVENTORY];
	public GameObject[] Cells = new GameObject[Game.MAXINVENTORY];
	public GameObject EmptySpaceGameObject;

	public GameObject InventoryBackground;
	public GameObject Cell;

	private bool inventoryUp = false;
	private bool setCells = false;

	private List<GameObject> TabMenuList = new List<GameObject> ();

	void Start()
	{
		TabMenuList.Add (InventoryBackground);

		Debug.Log ("Started!");
		for(int i = 0; i<Inventory.Length; i++)
		{
			InventoryItem invitem = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
			invitem.init(EmptySpaceGameObject, getNextOpenSlot());
			Inventory[i] = invitem;
		}

		inventoryUp = true;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			if(inventoryUp == false)
				inventoryUp = true;
			else
				inventoryUp = false;
		}

		if(inventoryUp)
		{
			foreach(GameObject obj in TabMenuList)
			{
				if(obj.GetComponent<GUITexture>() != null)
				{
					obj.GetComponent<GUITexture>().enabled = true;
				}
			}

			if(setCells == false)
			{
				for(int i = 0; i < player.GetComponent<Player>().inventorySize; i++)
				{
					if(i < 8)
					{
						GameObject cell = Instantiate(Cell, new Vector3(0.23f + (0.077f * i), 0.72f, 0f), Cell.transform.rotation) as GameObject;
						cell.transform.localScale = Cell.transform.localScale;

						TabMenuList.Add(cell);

						cell.GetComponent<CellHandler>().id = i;

						cell.GetComponent<CellHandler>().player = player;

						Cells[i] = cell;
					}	                                          
				}
			}
			setCells = true;
		}
		else
		{
			foreach(GameObject obj in TabMenuList)
			{
				if(obj.GetComponent<GUITexture>() != null)
				{
					obj.GetComponent<GUITexture>().enabled = false;
				}
			}
		}
	}

	private int getEmptySlots()
	{
		int i = 0;
		foreach (InventoryItem item in Inventory) 
		{
			if(!item.isFilled)
				i++;
		}

		return i;
	}

	public void AddItem(GameObject item)
	{
		if(getSlotOfItem(item.GetComponent<Item>().UIName) == -1 || !itemStackable(item)) //item to be added not in inventory or not stackable
		{
			if (getNextOpenSlot() != -1) //inventory has space
			{
				if (item.GetComponent<Item> () != null) 
				{
					InventoryItem invitem = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
					invitem.init(item, getNextOpenSlot (), item.GetComponent<Item> ().Stackable);
					Inventory [getNextOpenSlot ()] = invitem;

					Debug.Log (item.GetComponent<Item>().UIName + " added. Its ID is " + invitem.getId() + ", and the position is " + (getNextOpenSlot()));

					invitem.isFilled = true;

					updateCells();
				} 
				else 
				{
					Debug.LogWarning ("The object you are trying to add the the inventory is not an item.");
				}
			}
			else
			{
				Debug.LogWarning ("Your inventory is full!");
			}
		}
		else //item is already in inventory and is stackable
		{
			int slotToBePlaced = getSlotOfItem(item.GetComponent<Item>().UIName);
			if (item.GetComponent<Item>() != null)
			{
				itemInSlot(slotToBePlaced).Count++;

				Debug.Log (item.GetComponent<Item>().UIName + " added. Its ID is " + itemInSlot(slotToBePlaced).getId() + ", and the position is " + slotToBePlaced);

				updateCells();
			}
			else
			{
				Debug.LogWarning ("The object you are trying to add the the inventory is not an item.");
			}
		}
	}

	public void updateCells()
	{
		for (int i = 0; i < Game.MAXINVENTORY; i++)
		{
			if(i >= player.GetComponent<Player>().inventorySize)
			{
				break;
			}
			Cells[i].GetComponent<CellHandler>().ItemHeld = Inventory[i];
			Cells[i].GetComponent<CellHandler>().Icon = Inventory[i].getObject().GetComponent<Item>().Icon;
			Debug.Log(Inventory[i].getObject().GetComponent<Item>().Icon);
			Cells[i].GetComponent<CellHandler>().UpdateView();

			TabMenuList.Add(Cells[i].GetComponent<CellHandler>().iconShower);
		}
	}

	public void updateInventoryFromCells()
	{
		for (int i = 0; i < Game.MAXINVENTORY; i++)
		{
			if(i >= player.GetComponent<Player>().inventorySize)
			{
				break;
			}
			Inventory[i] = Cells[i].GetComponent<CellHandler>().ItemHeld;
			Cells[i].GetComponent<CellHandler>().UpdateView();
		}
	}

	public int getNextOpenSlot()
	{
		int id = 0;
		foreach (InventoryItem item in Inventory) 
		{
			try
			{
				if(item.isFilled)
					id++;
				else
					return id;
			}
			catch(System.NullReferenceException)
			{
				return id;
			}
		}
		return -1;
	}

	public int getSlotOfItem(string UINameSearch)
	{
		int id = 0;
		foreach(InventoryItem item in Inventory)
		{
			if(item.getObject().GetComponent<Item>().UIName == UINameSearch)
				return id;
			else
				id++;
		}
		return -1;
	}

	public InventoryItem itemInSlot(int slot)
	{
		return Inventory[slot];
	}

	public bool itemStackable(GameObject obj)
	{
		if (obj.GetComponent<Item> ().Stackable)
		{
			if (itemInSlot (getSlotOfItem (obj.GetComponent<Item> ().UIName)).Count < obj.GetComponent<Item> ().MaxStacks)
			{
				return true;
			}
		}
		return false;
	}
}