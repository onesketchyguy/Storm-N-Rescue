using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private static List<GameObject> objects = new List<GameObject>();

    public static GameObject GetObject(GameObject @object)
    {
        foreach (var item in objects)
        {
            if (item.activeSelf == false && item.CompareTag(@object.tag) && item.name.Contains(@object.name))
            {
                // this is likely the same object
                return item;
            }
        }

        // We need to create one
        var go = Instantiate(@object, instance.transform);
        go.name = @object.name;
        objects.Add(go);

        return go;
    }

    public static void ReturnObject(GameObject @object)
    {
        if (objects.Contains(@object) == false)
        {
            objects.Add(@object);
        }

        @object.SetActive(false);

        var rigidBody = @object.GetComponent<Rigidbody>();

        if (rigidBody != null)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }
    }
}