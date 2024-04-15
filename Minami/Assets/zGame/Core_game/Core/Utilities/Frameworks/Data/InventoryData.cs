using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Frameworks.Data
{
    public interface IInventoryItem : IComparable<IInventoryItem>
    {
        int Id { get; set; }
        int BaseId { get; set; }
        int Rarity { get; set; }
        int Level { get; set; }
        int Quantity { get; set; }
    }

    [System.Serializable]
    public class InventoryItem : IInventoryItem
    {
        [SerializeField] protected int b;
        [SerializeField] protected int id;
        [SerializeField] protected int r;
        [SerializeField] protected int l;
        [SerializeField] protected int q;

        public int Id { get { return id; } set { id = value; } }
        public int BaseId { get { return b; } set { b = value; } }
        public int Rarity { get { return r; } set { r = value; } }
        public int Level { get { return l; } set { l = value; } }
        public int Quantity { get { return q; } set { q = value; } }

        public int CompareTo(IInventoryItem pOther)
        {
            if (Id == pOther.Id)
            {
                if (Rarity == pOther.Rarity)
                    return Level.CompareTo(pOther.Level);
                else
                    return Rarity.CompareTo(pOther.Rarity);
            }
            else
                return Id.CompareTo(pOther.Id);
        }
    }

    public class InventoryData<T> : DataGroup where T : IInventoryItem
    {
        protected ListData<T> mItems;
        protected IntegerData mLastItemId;
        protected ListData<int> mDeletedIds;
        protected ListData<int> mBuzzIds;

        public int Count => mItems.Count;
        public T this[int index]
        {
            get { return mItems[index]; }
            set { mItems[index] = value; }
        }

        public InventoryData(int pId) : base(pId)
        {
            mItems = AddData(new ListData<T>(0));
            mLastItemId = AddData(new IntegerData(1));
            mDeletedIds = AddData(new ListData<int>(2));
            mBuzzIds = AddData(new ListData<int>(3));
        }
        public override bool Stage()
        {
            //Debug.LogError("Inventory check change : "+ base.Stage());
            //Debug.LogError("Inventory check change 11 : " + DataSaver.Get(Key,out int index));
            //Debug.LogError("Inventory check change 11 : " + mItems.GetStringValue());
            return base.Stage();
        }
        public bool Insert(T pInvItem)
        {
            if (pInvItem.Id > 0)
            {
                for (int i = 0; i < mItems.Count; i++)
                    if (mItems[i].Id == pInvItem.Id)
                    {
                        Debug.LogError("Id of inventory item must be unique!");
                        return false;
                    }
            }
            else
            {
                int newId = mLastItemId.Value += 1;
                if (mDeletedIds.Count > 0)
                {
                    newId = mDeletedIds[mDeletedIds.Count - 1];
                    mDeletedIds.RemoveAt(mDeletedIds.Count - 1);
                }

                pInvItem.Id = newId;
                mBuzzIds.Add(newId);
            }

            mItems.Add(pInvItem);

            if (pInvItem.Id > mLastItemId.Value)
                mLastItemId.Value = pInvItem.Id;

            return true;
        }

        public bool Insert(List<T> pInvItems)
        {
            return Insert(pInvItems.ToArray());
        }

        public bool Insert(params T[] pInvItems)
        {
            for (int j = 0; j < pInvItems.Length; j++)
            {
                if (pInvItems[j].Id > 0)
                {
                    for (int i = 0; i < mItems.Count; i++)
                        if (mItems[i].Id == pInvItems[j].Id)
                        {
                            Debug.LogError("Id of inventory item must be unique!");
                            return false;
                        }
                }
                else
                {
                    int newId = mLastItemId.Value += 1;
                    if (mDeletedIds.Count > 0)
                    {
                        newId = mDeletedIds[mDeletedIds.Count - 1];
                        mDeletedIds.RemoveAt(mDeletedIds.Count - 1);
                    }
                    pInvItems[j].Id = newId;
                    mBuzzIds.Add(newId);
                }
            }

            for (int j = 0; j < pInvItems.Length; j++)
            {
                mItems.Add(pInvItems[j]);
            }
            return true;
        }

        public bool Update(T pInvItem)
        {
            for (int i = 0; i < mItems.Count; i++)
                if (mItems[i].Id == pInvItem.Id)
                {
                    mItems[i] = pInvItem;
                    return true;
                }
            Debug.LogError("Could not update item, because Id is not found!");
            return false;
        }

        public bool Delete(T pInvItem)
        {
            for (int i = 0; i < mItems.Count; i++)
                if (mItems[i].Id == pInvItem.Id)
                {
                    mDeletedIds.Add(mItems[i].Id);
                    mItems.RemoveAt(i);
                    return true;
                }
            Debug.LogError("Could not delete item, because Id is not found!");
            return false;
        }

        public bool Delete(int id)
        {
            for (int i = 0; i < mItems.Count; i++)
                if (mItems[i].Id == id)
                {
                    mDeletedIds.Add(mItems[i].Id);
                    mItems.Remove(mItems[i]);
                    return true;
                }
            Debug.LogError("Could not delete item, because Id is not found!");
            return false;
        }

        public T GetItemByIndex(int pIndex)
        {
            return mItems[pIndex];
        }

        public T GetItemById(int pId)
        {
            if (pId > 0)
            {
                for (int i = 0; i < mItems.Count; i++)
                    if (mItems[i].Id == pId)
                        return mItems[i];
            }
            return default(T);
        }

        public void RemoveBuzzId(int pId)
        {
            mBuzzIds.Remove(pId);
        }

        public bool Buzzed(int pId)
        {
            return mBuzzIds.Contain(pId);
        }

        public bool HasBuzz()
        {
            return mBuzzIds.Count > 0;
        }

        public List<T> GetBuzzedItems()
        {
            var list = new List<T>();
            for (int i = 0; i < mItems.Count; i++)
            {
                if (mBuzzIds.Contain(mItems[i].Id))
                    list.Add(mItems[i]);
            }
            return list;
        }

        public void SortyById(bool des = false)
        {
            for (int i = 0; i < mItems.Count - 1; ++i)
            {
                for (int j = i + 1; j < mItems.Count; ++j)
                {
                    if ((mItems[i].Id > mItems[j].Id && !des)
                        || (mItems[i].Id < mItems[j].Id && des))
                    {
                        var temp = mItems[i];
                        mItems[i] = mItems[j];
                        mItems[j] = temp;
                    }
                }
            }
        }

        public void SortyByRarity(bool des = false)
        {
            for (int i = 0; i < mItems.Count - 1; ++i)
            {
                for (int j = i + 1; j < mItems.Count; ++j)
                {
                    if ((mItems[i].Rarity > mItems[j].Rarity && !des)
                        || (mItems[i].Rarity < mItems[j].Rarity && des))
                    {
                        var temp = mItems[i];
                        mItems[i] = mItems[j];
                        mItems[j] = temp;
                    }
                }
            }
        }

        public void SortByLevel(bool des = false)
        {
            for (int i = 0; i < mItems.Count - 1; ++i)
            {
                for (int j = i + 1; j < mItems.Count; ++j)
                {
                    if ((mItems[i].Level > mItems[j].Level && !des)
                        || (mItems[i].Level < mItems[j].Level && des))
                    {
                        var temp = mItems[i];
                        mItems[i] = mItems[j];
                        mItems[j] = temp;
                    }
                }
            }
        }

        public void SortByQuantity(bool des = false)
        {
            for (int i = 0; i < mItems.Count - 1; ++i)
            {
                for (int j = i + 1; j < mItems.Count; ++j)
                {
                    if ((mItems[i].Quantity > mItems[j].Quantity && !des)
                        || (mItems[i].Quantity < mItems[j].Quantity && des))
                    {
                        var temp = mItems[i];
                        mItems[i] = mItems[j];
                        mItems[j] = temp;
                    }
                }
            }
        }

        public void SortByBaseId(bool des = false)
        {
            for (int i = 0; i < mItems.Count - 1; ++i)
            {
                for (int j = i + 1; j < mItems.Count; ++j)
                {
                    if ((mItems[i].BaseId > mItems[j].BaseId && !des)
                        || (mItems[i].BaseId < mItems[j].BaseId && des))
                    {
                        var temp = mItems[i];
                        mItems[i] = mItems[j];
                        mItems[j] = temp;
                    }
                }
            }
        }

        public void InternalChange()
        {
            mItems.MarkChange();
        }
    }
}