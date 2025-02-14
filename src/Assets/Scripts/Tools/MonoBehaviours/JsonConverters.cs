using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Newtonsoft;

namespace Examples { 

public class JsonConverters : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class PlayerDataConverter : Newtonsoft.Json.JsonConverter
{
    private readonly System.Type[] _types;

    public PlayerDataConverter(params System.Type[] types)
    {
        _types = types;
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
    {
        Newtonsoft.Json.Linq.JToken t = Newtonsoft.Json.Linq.JToken.FromObject(value);

        if (t.Type != Newtonsoft.Json.Linq.JTokenType.Object)
        {
            t.WriteTo(writer);
        }
        else
        {
            Newtonsoft.Json.Linq.JObject o = (Newtonsoft.Json.Linq.JObject)t;
            IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

            //o.AddFirst(new Newtonsoft.Json.Linq.JProperty("Keys", new Newtonsoft.Json.Linq.JArray(propertyNames)));

            o.WriteTo(writer);
        }
    }

    public override object ReadJson(Newtonsoft.Json.JsonReader reader, System.Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
    }

    public override bool CanRead
    {
        get { return false; }
    }

    public override bool CanConvert(Type objectType)
    {
        return _types.Any(t => t == objectType);
    }
}



}