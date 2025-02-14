using System.Data;
using System.IO;
using System;
using System.Collections.Generic;

namespace WorldPersistence { 

public class WorldData
{
    private DataSet state;
    private string world_file_path = "WorldData.xml";
    public static string stringSceneTable = "SceneTable";
    static public string stringPObjectTable = "PObjectTable";

    public string WORLD_FILE_PATH{set => world_file_path = value; get => world_file_path;}
    public DataSet State{set => state = value; get => state;}
    public SceneTable ScenesTBL{get => State.Tables[stringSceneTable] as SceneTable;}
    public PObjectTable PObjectsTBL{get => State.Tables[stringPObjectTable] as PObjectTable;}

    public void Init()
    {
        State = new DataSet();

        SceneTable Scenes = new SceneTable();
        Scenes.TableName = stringSceneTable;
        State.Tables.Add(Scenes);

        PObjectTable PObjects = new PObjectTable();
        PObjects.TableName = stringPObjectTable;
        State.Tables.Add(PObjects);

        DataRelation relation = new DataRelation("ScenesToPObjects", ScenesTBL.IDColumn, PObjectsTBL.SceneIDColumn);
        PObjectsTBL.ParentRelations.Add(relation);
    }

    // Commit the current state of the databases to disk
    public void Save()
    {
        var xml = State.GetXml();

        File.WriteAllText(WORLD_FILE_PATH, xml);
    }

    // Load the datatables from disk
    public void Load()
    {
        FileInfo fInfo = new FileInfo("WorldData.XML");
        if(File.Exists(WORLD_FILE_PATH) is true && WorldDataTools.IsTextFileEmpty(WORLD_FILE_PATH) is false)
        {
            State.ReadXml(WORLD_FILE_PATH);
        }
        else
        {
            using (var file = File.Create(WORLD_FILE_PATH))
            {
                file.Close();
            }
        }
        fInfo = new FileInfo("WorldData.XML");
    }

    public List<PObjectData> GetPObjectsInScene(string sceneName)
    {
        SceneRow scene;
        int id;
        try
        {
            if((scene = FindScene(sceneName)) is null)
            {
                throw new NullReferenceException("GetPObjectsInScene: Scene does not exist"); 
            }
            else
            {
                id = scene.ID;
            }
        }
        catch
            {
                return null;
            }

        var filteredTable = from r in PObjectsTBL.AsEnumerable() where (r as PObjectRow).SceneID == id select r;
        var castTable = filteredTable.Cast<PObjectRow>();
        List<PObjectData> pObjectsList = new List<PObjectData>();
        foreach(var row in castTable)
        {
            pObjectsList.Add(new PObjectData(row.PObjectID, row.POGUID, row.SerializedData));
        }
        return pObjectsList;
    }

    public void SetPObjectsInScene(string sceneName, List<PObjectData> pObjects)
    {
        var pobjects = PObjectsTBL.Rows;
        foreach(var po in pObjects)
        {
            if(pobjects.Find(po.GUID) is PObjectRow row)
            {
                row.SerializedData = po.SerializedData;
            }
            else
            {
                AddPObject(po.GUID, sceneName, po.SerializedData);
            }
        }
    }     

    public bool IsSceneInWorldData(string sceneName)
    {
        SceneRow[] rows = (SceneRow[])(ScenesTBL.Select(""));
        foreach(var row in rows){
            if(row.SceneName == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    private bool AddScene(string sceneName)
    {   // Todo: Add a check here instead of a try/catch
        try
        {
            SceneRow row = ScenesTBL.NewSceneRow();
            row.SceneName = sceneName;
            ScenesTBL.Add(row);
            return true;
        }
        catch (ConstraintException except)
        {
            Console.WriteLine($"Caught: {except.Message}");
            return false;
        }
    }

    private SceneRow FindScene(string sceneName)
    {
        SceneRow[] rows = (SceneRow[])(ScenesTBL.Select(""));
        foreach(var row in rows){
            if(row.SceneName == sceneName)
            {
                return row;
            }
        }
        return null;
    }

    private SceneRow FindOrAddScene(string sceneName)
    {
        if(FindScene(sceneName) is SceneRow r)
        {
            return r;
        }
        else
        {
            AddScene(sceneName);
            return FindScene(sceneName);
        }

        throw new InvalidOperationException("Scene not found or created");
    }

    private bool AddPObject(string POGUID, string sceneName, string serializedData)
    {   // Todo: Add a check here instead of a try/catch
        try
        {
            PObjectRow row = PObjectsTBL.NewObjectRow();
            row.POGUID = POGUID;
            row.SerializedData = serializedData;

            // Find the scene, and if it does not exist create it.
            var scene = FindOrAddScene(sceneName);
            row.SceneID = scene.ID;
            PObjectsTBL.Add(row);
            return true;
        }
        catch (ConstraintException except)
        {
            Console.WriteLine($"Caught: {except.Message}");
            return false;
        }
    }

    private PObjectRow FindPObject(string POGUID)
    {
        PObjectRow[] rows = (PObjectRow[])(PObjectsTBL.Select(""));
        foreach(var row in rows){
            if(row.POGUID == POGUID)
            {
                return row;
            }
        }
        return null;
    }

    
    private PObjectRow FindOrAddPObject(string POGUID, string sceneName, string serializedData)
    {
        PObjectRow[] row = (PObjectRow[])(PObjectsTBL.Select(""));
        foreach(var r in row)
        {
            if(r.POGUID == POGUID)
            {
                return r;
            }
        }
        AddPObject(POGUID, sceneName, serializedData);
        return FindPObject(POGUID);
        throw new InvalidOperationException("PObject not found or created");
    }


}


}