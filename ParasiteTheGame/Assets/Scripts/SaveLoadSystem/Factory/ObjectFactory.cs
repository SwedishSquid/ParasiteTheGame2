using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    [SerializeField] private List<TypeObjectPair<string, MonoBehaviour>> factoryList;

    public MonoBehaviour GenerateGameObjectByName(string typeName, string GUID)
    {
        var obj = GetObjectByName(typeName);

        if (obj == null)
        {
            Debug.LogError($"Factory cannot create objects of typeName = {typeName}");
            return null;
        }

        var instance = Instantiate(obj);
        (instance as ISavable)?.SetGUID(GUID);

        return instance;
    }

    private MonoBehaviour GetObjectByName(string typeName)
    {
        return factoryList.FirstOrDefault(p => p.Type == typeName)?.Object;
    }
}
