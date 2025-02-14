namespace WorldPersistence
{ 

    public class PObjectData
    {
        int databaseID;
        string guid;
        string serializedData;

        public int DatabaseID {set => databaseID = value; get => databaseID;}
        public string GUID {set => guid = value; get => guid; }
        public string SerializedData {set => serializedData = value; get => serializedData; }

        public PObjectData(int _id, string _guid, string _seri)
        {
            DatabaseID = _id;
            GUID = _guid;
            SerializedData = _seri;
        }

        public PObjectData(string _guid, string _seri)
        {
        GUID = _guid;
        SerializedData = _seri;
        }
    }
}
