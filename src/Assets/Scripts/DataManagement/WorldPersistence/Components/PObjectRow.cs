using System.Data;

namespace WorldPersistence
{

    public class PObjectRow : DataRow
    {
        public int PObjectID
        {
            get { return (int)base[PObjectTable.stringPObjectID]; }
            set { base[PObjectTable.stringPObjectID] = value; }
        }

        public int SceneID
        {
            get { return (int)base[PObjectTable.stringSceneID]; }
            set { base[PObjectTable.stringSceneID] = value; }
        }

        public string POGUID
        {
            get { return (string)base[PObjectTable.stringPOGUID]; }
            set { base[PObjectTable.stringPOGUID] = value; }
        }

        public string SerializedData
        {
            get { return (string)base[PObjectTable.stringSerializedData]; }
            set { base[PObjectTable.stringSerializedData] = value; }
        }

        internal PObjectRow(DataRowBuilder builder) : base(builder)
        {

        }
    }
}