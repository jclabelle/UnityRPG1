using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PersistentMonobehaviour : MonoBehaviour, WorldPersistence.IPersistentController
{
    public abstract Dictionary<string, string> GetPersistentData();

    public abstract void SetPersistentData(Dictionary<string, string> pData);
}