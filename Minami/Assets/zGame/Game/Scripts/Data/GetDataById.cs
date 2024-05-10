using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDataProvider<T>
{
    T GetItemByID(int id);
}
public class ItemById
{
    public int id;
}
public class ItemList<T> :List<ItemById>, IDataProvider<T> where T : ItemById 
{
    public List<T> items; 

   

    public T GetItemByID(int id)
    {
        foreach (T item in items)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return default(T);
    }

    public List<T> Values
    {
        set
        {
            if (value != items)
            {
                items = value;
            }
        }
        get
        {
            return items;
        }
    }
}




