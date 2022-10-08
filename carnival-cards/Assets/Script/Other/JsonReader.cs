using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonReader
{

    public Context ReadJsonForContext(TextAsset jsonText)
    {
        return JsonConvert.DeserializeObject<Context>(jsonText.text);

    }
}
