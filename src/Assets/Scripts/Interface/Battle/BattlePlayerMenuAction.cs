using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu]
public class BattlePlayerMenuAction: ScriptableObject, IDisplayableInformation
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Texture2D Icon { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual string GetDisplayableName()
    {
        return Name;
    }

    public string GetDisplayableStats()
    {
        throw new System.NotImplementedException();
    }

    public virtual string GetDisplayableDescription()
    {
        throw new System.NotImplementedException();
    }

    public Texture2D GetDisplayableIcon()
    {
        return Icon;
    }
}