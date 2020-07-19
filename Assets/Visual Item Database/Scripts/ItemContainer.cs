using UnityEngine;
using ItemSystem;

public class ItemContainer : MonoBehaviour
{
    public ItemBase item;

	void Reset(){
		Updated ();
	}

	public void Updated(){
		if (GetComponent<WorldItem> () != null) {
			GetComponent<WorldItem> ().SetItem (item, 1);
		}
	}

}