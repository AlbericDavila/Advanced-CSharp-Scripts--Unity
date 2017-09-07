
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// Property for accessing single instance, there should only be 1 instance of the inventory at all times.
    /// This allows us to set it to static to be used accros all scripts without a need to reference it.
    /// </summary>
    public static Inventory Instance
	{
		get
		{
			if(thisInstance==null)
			{
				GameObject inventoryObject = new GameObject("Inventory");
				thisInstance = inventoryObject.AddComponent<Inventory>();
			}

			return thisInstance;
		}
	}

	private static Inventory thisInstance = null;   // Reference to singleton object
    public RectTransform itemList = null;           // Root object of item list

    void Awake ()
	{
		// If single object already exists then destroy
		if(thisInstance!=null)
		{
			DestroyImmediate(gameObject);
			return;
		}

		// Make this single instance
		thisInstance = this;
	}

    public static void addItem(GameObject GO)
	{
        // Destroy picked up item
        Destroy(GO.gameObject);

		// Add to first available slot
		for(int i=0; i<thisInstance.itemList.childCount; i++)
		{
			Transform item = thisInstance.itemList.GetChild(i);

			// If Item is not active, then it becomes new slot
			if(!item.gameObject.activeSelf)
			{
				item.GetComponent<Image>().sprite = GO.GetComponent<InventoryItem>().GUI_Icon;
				item.gameObject.SetActive(true);
				return;
			}
		}
	}
}