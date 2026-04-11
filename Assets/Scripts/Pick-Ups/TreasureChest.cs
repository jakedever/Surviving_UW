using System.Diagnostics;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    InventoryManager inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<InventoryManager>();
    }

    private void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OpenTreasureChest();
            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest()
    {
        UnityEngine.Debug.Log("Chest Opened");
    }
}
