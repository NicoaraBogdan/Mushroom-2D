using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField]
    private GameObject afterImagePref;

    private Queue<GameObject> availableObject = new Queue<GameObject>();

    public static PlayerAfterImagePool Instace;
    private void Awake()
    {
        Instace = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for(int i = 0; i < 10; i++)
        {
            var objToAdd = Instantiate(afterImagePref);
            objToAdd.transform.SetParent(transform);
            AddToPool(afterImagePref);
        }
    }

    public void AddToPool(GameObject objToAdd)
    {
        objToAdd.SetActive(false);
        availableObject.Enqueue(objToAdd);
    }

    public GameObject GetFromPool()
    {
        if (availableObject.Count == 0)
        {
            GrowPool();
        }

        var obj = availableObject.Dequeue();
        obj.SetActive(true);
        return obj;
    }
}
