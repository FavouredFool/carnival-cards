using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
{
    public T prefab;


    public virtual T CreateNewInstance()
    {
        return Instantiate(prefab);
    }
}
