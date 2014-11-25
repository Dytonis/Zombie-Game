using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	public string UIName;
	public bool Stackable;
	public int MaxStacks;
	public Texture Icon;

	private Color[] originals = new Color[Game.MAXMATERIALS];
	private MeshRenderer mesh;

	private bool hitting = false;

	private string oldName;

	// Use this for initialization
	void Start () 
	{
		oldName = gameObject.name;
		#region Get-Original-Color
		mesh = this.transform.gameObject.GetComponent<MeshRenderer> ();

		int i = 0;
		foreach (Material mat in mesh.materials) 
		{
			originals[i] = mat.color;

			i++;
		}
		#endregion
	}
	
	// Update is called once per frame
	void Update () 
	{
		#region Reset-Color
		if (!hitting) 
		{
			int i = 0;
			foreach (Material mat in mesh.materials) 
			{
				mat.color = originals [i];
				i++;
			}
		}

		hitting = false;
		#endregion
	}

	public void recieveRaycast()
	{
		#region Highlight-Color
		hitting = true;

		int i = 0;
		foreach (Material mat in mesh.materials) 
		{
			var tempRed = originals[i].r;
			var tempGreen = originals[i].g;
			var tempBlue = originals[i].b;
			
			mat.color = new Color(tempRed + 0.15f, tempGreen + 0.15f, tempBlue + 0.15f);

			i++;
		}
		#endregion
	}

	public void recievePickup()
	{
		gameObject.transform.position = new Vector3 (-9999, -9999, -9999);
		gameObject.name = "Inventory - " + gameObject.name;
	}

	public void recieveDrop(Vector3 pos)
	{
		gameObject.transform.position = pos;
	}
}
