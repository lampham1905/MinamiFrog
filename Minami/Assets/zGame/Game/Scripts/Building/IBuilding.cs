using System.Collections;
using System.Collections.Generic;
using Lam;
using UnityEngine;

public class IBuilding : MonoBehaviour
{
    public Transform Achor;
    public GameObject ModelBuilding;
    public int size;
    public int id;
    public int idBuilding;
    public bool isUpgrade;
    public bool isShop;
    public ShopData shopData;
    public BuildingData buildingData;
    public Transform posSpawVillager => this.transform;
    public Node node;
    public virtual void SetData(BuildingData data)
    {
        this.idBuilding = data.idBulding;
        this.id = data.id;
        this.isUpgrade = data.isUpgrade;
        this.isShop = (data.idShop != 1);
        this.shopData = data.shopData;
        this.buildingData = data;
    }
    public virtual void Build()
    {

    }

    public virtual void Interact()
    {

    }
}
