using System.Data;
using System;

namespace WorldPersistence
{ 

public class SceneRow : DataRow
{
    public int ID
    {
        get {return (int)base[SceneTable.stringID];}
        set {base[SceneTable.stringID] = value;}
    }
    
    public string SceneName
    {
        get {return (string)base[SceneTable.stringSceneName];}
        set {base[SceneTable.stringSceneName] = value;}
    }

    internal SceneRow(DataRowBuilder builder) : base(builder)
    {
        SceneName = String.Empty;
    }
}
}