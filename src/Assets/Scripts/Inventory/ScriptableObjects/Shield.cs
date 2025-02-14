using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(menuName = "Equipment/Shield"), System.Serializable]
public class Shield : Equipment, IHandHeld
{
    [SerializeField] private bool isTwoHander;
    public bool IsTwoHander { get => isTwoHander; set => isTwoHander = value; }
}
