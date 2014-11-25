using UnityEngine;
using System.Collections;

public class DragDropHandler : MonoBehaviour 
{
	public bool dragging = false;

	private GameObject iconShower;
	private InventoryItem heldItem;
	private int currentCellID;
	private Rect resetInset;
	private Vector3 resetPos;

	// Update is called once per frame
	void Update () 
	{
		if (dragging) 
		{
			iconShower.GetComponent<GUITexture>().pixelInset = new Rect(
				Input.mousePosition.x - 20, 
			    Input.mousePosition.y - 20, 
			    iconShower.GetComponent<GUITexture>().pixelInset.width, 
			    iconShower.GetComponent<GUITexture>().pixelInset.height);

			iconShower.transform.position = new Vector3(0f, 0f, iconShower.transform.position.z);

			iconShower.GetComponent<GUITexture>().color = new Color(iconShower.GetComponent<GUITexture>().color.r,
			                                                        iconShower.GetComponent<GUITexture>().color.g,
			                                                        iconShower.GetComponent<GUITexture>().color.r,
			                                                        0.2f);
		}
	}

	public void recieveDragDrop(GameObject iconShower, InventoryItem heldItem, int currentCellID, Rect resetInset, Vector3 resetPos)
	{
		this.iconShower = iconShower;
		this.heldItem = heldItem;
		this.currentCellID = currentCellID;
		this.resetInset = resetInset;
		this.resetPos = resetPos;

		dragging = true;
	}

	public void recieveDrop(int newCellId)
	{
		iconShower.GetComponent<GUITexture> ().pixelInset = gameObject.GetComponent<InventoryManager>().Cells[newCellId].GetComponent<GUITexture>().pixelInset;
		iconShower.GetComponent<GUITexture> ().color = new Color(iconShower.GetComponent<GUITexture>().color.r,
		                                                         iconShower.GetComponent<GUITexture>().color.g,
		                                                         iconShower.GetComponent<GUITexture>().color.r,
		                                                         0.6f);

		#region mapper
		InventoryItem switch_ItemHeld = gameObject.GetComponent<InventoryManager> ().Cells [currentCellID].GetComponent<CellHandler> ().ItemHeld;
		Texture switch_Icon = gameObject.GetComponent<InventoryManager> ().Cells [currentCellID].GetComponent<CellHandler> ().Icon;
		Texture switch_Show = gameObject.GetComponent<InventoryManager> ().Cells [currentCellID].GetComponent<CellHandler> ().iconShower.GetComponent<GUITexture>().texture;

		gameObject.GetComponent<InventoryManager> ().Cells [currentCellID].GetComponent<CellHandler> ().ItemHeld = 
			gameObject.GetComponent<InventoryManager> ().Cells [newCellId].GetComponent<CellHandler> ().ItemHeld;

		gameObject.GetComponent<InventoryManager> ().Cells [currentCellID].GetComponent<CellHandler> ().iconShower.GetComponent<GUITexture>().texture = 
			gameObject.GetComponent<InventoryManager> ().Cells [newCellId].GetComponent<CellHandler> ().iconShower.GetComponent<GUITexture>().texture;

		gameObject.GetComponent<InventoryManager> ().Cells [currentCellID].GetComponent<CellHandler> ().iconShower.transform.position = new Vector3(
			resetPos.x,
			resetPos.y,
			1f);

		gameObject.GetComponent<InventoryManager> ().Cells [currentCellID].GetComponent<CellHandler> ().Icon = 
			gameObject.GetComponent<InventoryManager> ().Cells [newCellId].GetComponent<CellHandler> ().Icon;

		gameObject.GetComponent<InventoryManager> ().Cells [newCellId].GetComponent<CellHandler> ().ItemHeld = 
			switch_ItemHeld;

		gameObject.GetComponent<InventoryManager> ().Cells [newCellId].GetComponent<CellHandler> ().Icon = 
			switch_Icon;

		gameObject.GetComponent<InventoryManager> ().Cells [newCellId].GetComponent<CellHandler> ().iconShower.GetComponent<GUITexture>().texture = 
			switch_Show;
		#endregion

		gameObject.GetComponent<InventoryManager> ().updateInventoryFromCells ();
		dragging = false;
	}
}
