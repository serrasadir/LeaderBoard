using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour //entryleri (özellikle çok fazla player olduğunda)
{                                                  //sürekli instantiate-destroy etmemek için pooling system
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    public ObjectPool(T prefab, Transform parent, int initialSize)//poolu verilen prefanler ile oluşturuyoruz
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject() //obje yaratıp poola atıyoruz
    {
        T obj = Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public T Get()//pooldan obje çekiyoruz
    {
        if (pool.Count == 0)
        {
            CreateNewObject();
        }

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)//objeyi poola geri veriyoruz
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public void ReturnAll(List<T> objects)//listedeki tüm objeleri poola geri veriyoruz
    {
        foreach (T obj in objects)
        {
            Return(obj);
        }
        objects.Clear();
    }
}
