using System;
using UnityEngine;

public class InventoryItem : ScriptableObject 
{
	public int Id;
	public int Count;
	public GameObject reference;
	public bool isStackable = true;
	public bool isFilled = false;
	
	public InventoryItem() { }

	public void init(GameObject obj, int id, bool stackable=true, int stack=1)
	{
		Debug.Log ("ID recieved: " + id);
		reference = obj;
		Id = id;
		isStackable = stackable;
		Count = stack;
	}
	
	public GameObject getObject()
	{
		return reference;
	}
	
	public int getId()
	{
		return Id;
	}
	
	public int getCount()
	{
		return Count;
	}
	
	public void recalculateFill()
	{
		if (reference == null)
			isFilled = false;
		else
			isFilled = true;
	}
}