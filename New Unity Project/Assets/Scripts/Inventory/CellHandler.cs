using UnityEngine;
using System.Collections;

public class CellHandler : MonoBehaviour 
{
	public GameObject player;
	public InventoryItem ItemHeld;
	public int id = 0;

	private bool dragSwtich = false;

	private Color original;

	public Texture Icon;

	[HideInInspector]
	public GameObject iconShower;

	public GameObject Cloner;

	void Start()
	{
		original = gameObject.GetComponent<GUITexture> ().color;

		iconShower = Instantiate (Cloner, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		iconShower.GetComponent<GUITexture> ().texture = Icon;

		iconShower.transform.position = new Vector3 (iconShower.transform.position.x, iconShower.transform.position.y, 1f );
	}

	public void UpdateView()
	{
		iconShower.GetComponent<GUITexture> ().texture = Icon;
	}

	void OnMouseOver()
	{
		gameObject.GetComponent<GUITexture> ().color = new Color (original.r + 0.15f, original.g + 0.15f, original.b);

		if(player.GetComponent<DragDropHandler>().dragging == true) //go into when the object is being dragged
		{
			if (Input.GetMouseButtonDown (1))
			{
				player.GetComponent<DragDropHandler>().dragging = false;
				Debug.Log("not dragging!");
				player.GetComponent<DragDropHandler>().recieveDrop(id);
				return;
			}
		}
		if(player.GetComponent<DragDropHandler>().dragging == false) //pickup
		{
			if(ItemHeld.getObject().GetComponent<Item>().UIName != "")
			{
				if (Input.GetMouseButtonDown (1))
				{
					player.GetComponent<DragDropHandler>().dragging = true;
					Debug.Log("dragging!");
					player.GetComponent<DragDropHandler>().recieveDragDrop(iconShower, ItemHeld, id, iconShower.GetComponent<GUITexture>().pixelInset, gameObject.transform.position);
				}
			}
		}
	}

	void OnMouseExit()
	{
		//Debug.Log ("asdf");
		gameObject.GetComponent<GUITexture> ().color = new Color(original.r, original.g, original.b);
	}
}
