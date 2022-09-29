using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonReader
{

    public CardContext ReadJsonForCardContext(TextAsset jsonText)
    {
        return JsonConvert.DeserializeObject<CardContext>(jsonText.text);

    }
}
