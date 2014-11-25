using UnityEngine;
using System.Collections;

public class ItemReachHandler : MonoBehaviour {

	public InventoryManager manager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GetComponent<UITextHolder>() != null)
		{
			GetComponent<UITextHolder>().texts[0].text = "";
		}

		RaycastHit info;

		if (Physics.Raycast (new Ray (Camera.main.transform.position, Camera.main.transform.forward), out info, Game.REACHRANGE))
		{
			GameObject obj = info.transform.gameObject;

			if(obj.GetComponent<Item>() != null)
			{
				obj.GetComponent<Item>().recieveRaycast();

				if(Input.GetKeyUp(KeyCode.E))
				{
					manager.AddItem(obj);
					obj.GetComponent<Item>().recievePickup();
				}

				if(GetComponent<UITextHolder>() != null)
				{
					GetComponent<UITextHolder>().texts[UIList.ItemName].text = obj.GetComponent<Item>().UIName;
				}
			}
		}
	}
}
