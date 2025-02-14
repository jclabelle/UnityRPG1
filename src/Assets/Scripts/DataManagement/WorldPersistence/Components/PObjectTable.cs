using System.Data;
using System;
namespace WorldPersistence { 

public class PObjectTable : DataTable
{
    static public string stringPObjectID = "PObjectID";
    static public string stringPOGUID = "POGUID";
    static public string stringSceneID = "SceneID";
    static public string stringSerializedData = "SerializedData";

    public DataColumn IDColumn {get => Columns[stringPObjectID];}
    public DataColumn POGUIDColumn {get => Columns[stringPOGUID];}
    public DataColumn SceneIDColumn {get => Columns[stringSceneID];}

    public DataColumn SerializedDataColumn {get => Columns[stringSerializedData];}

    public PObjectTable()
    {
        DataColumn poID = new DataColumn(stringPObjectID, typeof(int));
        poID.Caption = "DataTable ID";
        poID.AutoIncrement = true;
        poID.ReadOnly = true;
        poID.Unique = true;
        Columns.Add(poID);

        DataColumn poguid = new DataColumn(stringPOGUID, typeof(string));
        poguid.AutoIncrement = false;
        poguid.Caption = "The global game unique ID of this PObject";
        poguid.ReadOnly = false;
        poguid.Unique = true;
        Columns.Add(poguid);

        DataColumn seridata = new DataColumn(stringSerializedData, typeof(string));
        seridata.AutoIncrement = false;
        seridata.Caption = "The persistent serialized state data of this PObject";
        seridata.ReadOnly = false;
        seridata.Unique = false;
        Columns.Add(seridata);

        DataColumn sceID = new DataColumn(stringSceneID, typeof(int));
        sceID.Caption = "ID of the Scene on the Scene DataTable";
        sceID.AutoIncrement = false;
        sceID.ReadOnly = false;
        sceID.Unique = false;
        Columns.Add(sceID);

        DataColumn[] PrimaryKeyColumns = new DataColumn[1];
        PrimaryKeyColumns[0] = Columns[stringPOGUID];
        this.PrimaryKey = PrimaryKeyColumns;


    }

    protected override Type GetRowType()
    {
        return typeof(PObjectRow);
    }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
    {
        return new PObjectRow(builder);
    }

    //Indexer, use as such: ObjectRow row = SceneTable[n];
    public PObjectRow this[int idx]
    {
        get {return (PObjectRow)Rows[idx];}
    }

    public void Add(PObjectRow row)
    {
        Rows.Add(row);
    }

    public void Remove(PObjectRow row)
    {
        Rows.Remove(row);
    }
    
    public PObjectRow NewObjectRow()
    {
        PObjectRow row = (PObjectRow)NewRow();
        return row;
    }


}
}