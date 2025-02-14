using System.Data;
using System.Text;
using System.IO;
using System.Linq;


namespace WorldPersistence {
    static class WorldDataTools
{
    public static void Print(DataTable table)
    {
        var writer = new StringWriter();    // Note: 
        table.WriteXml(writer);
    }

    public static void Print(DataSet set)
    {
        var writer = new StringWriter();    // Note: 
        set.WriteXml(writer);
    }

    public static void Print(DataRow row)
    {
        if(row is not null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", row.ItemArray));
        }
    }

    public static DataRow GetRowByID(DataTable table, int id)
    {
        var row = table.Select("").FirstOrDefault(x => (int)x["id"] == 0);
        return row;
    }

    public static string GetValueAsString(DataTable table, int rowindex, string columnName)
    {
        string s = (string)table.Rows[rowindex][columnName];
        return s;
    }

    public static System.Object GetValue(DataTable table, int rowindex, string columnName)
    {
        var s = table.Rows[rowindex][columnName];
        return s;
    }

    public static bool IsTextFileEmpty(string fileName)
{
    var info = new FileInfo(fileName);
    if (info.Length == 0)
        return true;

    // If file is extremely small (Byte Order is 6 bytes see https://en.wikipedia.org/wiki/Byte_order_mark).
    if (info.Length < 6)
    {
        // Read the file to verify if Length > 0 because of a byte order or if there is real Text content
        var content = File.ReadAllText(fileName);   
        return content.Length == 0;
    }
    return false;
}

    public static bool IsFileLocked(FileInfo file)
    {
        try
        {
            using(FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }

    
}
}