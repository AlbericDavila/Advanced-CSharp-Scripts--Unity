using UnityEngine;
using System.Collections;

/// <summary>
/// This script goes attached to the gameObject you wish to make an item
/// </summary>
public class InventoryItem : MonoBehaviour
{
	// This enum can be scaled to fit any type of item (e.g. Food, Weapon, Clothing, etc.)
	public enum ITEMTYPE {SPHERE, BOX, CYLINDER};
	public ITEMTYPE Type;
	public Sprite GUI_Icon;
	
	void OnTriggerEnter(Collider collingObject)
	{
        // If item collides with anything that is not the player, then return.
		if(!collingObject.CompareTag("Player"))
			return;

		// Add this item to the inventory
		Inventory.addItem(gameObject);
	}
}
