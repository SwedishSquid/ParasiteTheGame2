using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    [SerializeField] private List<TypeObjectPair<string, MonoBehaviour>> factoryList;

    private Dictionary<string, MonoBehaviour> typeToGameObject = new Dictionary<string, MonoBehaviour>();

    private void Start()
    {
        
    }

    public MonoBehaviour GenerateGameObjectByName(string typeName)
    {
        typeToGameObject.Clear();
        foreach (var pair in factoryList)
        {
            typeToGameObject.Add(pair.Type, pair.Object);
        }

        if (!typeToGameObject.ContainsKey(typeName))
        {
            Debug.LogError($"Factory cannot generate {typeName} - it is not in the factory list");
            return null;
        }
        return Instantiate(typeToGameObject[typeName]);
    }
}
