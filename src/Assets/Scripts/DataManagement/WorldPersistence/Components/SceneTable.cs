using System.Data;
using System;

namespace WorldPersistence
{

    public class SceneTable : DataTable
    {
        static public string stringID = "ID";
        static public string stringSceneName = "SceneName";
        public DataColumn IDColumn { get => Columns[stringID]; }
        public DataColumn SceneNameColumn { get => Columns[stringSceneName]; }

        public SceneTable()
        {
            DataColumn ID = new DataColumn(stringID, typeof(int));
            ID.AutoIncrement = true;
            ID.ReadOnly = true;
            ID.Unique = true;
            ID.Caption = "ID on DataTable";
            Columns.Add(ID);

            DataColumn SceneName = new DataColumn(stringSceneName, typeof(string));
            SceneName.AutoIncrement = false;
            SceneName.Caption = "Name of the Scene";
            SceneName.ReadOnly = false;
            SceneName.Unique = true;
            Columns.Add(SceneName);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = Columns[stringID];
            this.PrimaryKey = PrimaryKeyColumns;
        }

        protected override Type GetRowType()
        {
            return typeof(SceneRow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new SceneRow(builder);
        }

        //Indexer, use as such: SceneRow row = SceneTable[n];
        public SceneRow this[int idx]
        {
            get { return (SceneRow)Rows[idx]; }
        }

        public void Add(SceneRow row)
        {
            Rows.Add(row);
        }

        public void Remove(SceneRow row)
        {
            Rows.Remove(row);
        }

        public SceneRow NewSceneRow()
        {
            SceneRow row = (SceneRow)NewRow();
            return row;
        }


    }
}