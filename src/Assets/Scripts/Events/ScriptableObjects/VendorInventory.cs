using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Equipment/Vendor Inventory"), System.Serializable]
public class VendorInventory : ScriptableObject
{
    [SerializeField] private List<Item> items;

    public List<Item> Items { get => items; set => items = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}