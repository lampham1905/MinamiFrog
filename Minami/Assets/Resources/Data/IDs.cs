using System;
using System.Collections.Generic;

    public class IDTool {
		public string key;
		public int value;
		public string des;
		public IDTool(string key,int value,string des="") {
			this.key = key;
			this.value = value;
			this.des = des;
		}
	}

    public class IDs
    {
	#region building
	public const int BUILDING_1_MODERNHOUSE = 1;
	public const int BUILDING_2_PARK = 2;
	public const int BUILDING_3_RAMENSHOP = 3;
	public const int BUILDING_4_BOBACAFE = 4;
	public const int BUILDING_5_ELDERMASION = 5;
	public const int BUILDING_6_ONSEN = 6;
	public const int BUILDING_7_BOOKSTORE = 7;
	public const int BUILDING_8_CONVENIENCESTORE = 8;
	public const int BUILDING_9_KARAOKEBAR = 9;
	public const int BUILDING_10_FLORIST = 10;
	public const int BUILDING_11_YOKAITREE = 11;
	public const int BUILDING_12_SERVICECENTER = 12;
	public const int BUILDING_HOUSE = 1;
	public const int BUILDING_SHOP = 2;
	public const int BUILDING_SERVICE = 3;
	public const int BUILDING_SPECIAL = 4;
	#endregion
	#region type_upgrade
	public const int TYPE_UPGRADE_NORMAL = 1;
	public const int TYPE_UPGRADE_FOREVER = 2;
	public const int TYPE_UPGRAD_SPECIAL = 3;
	#endregion
	#region type_shop
	public const int TYPE_SHOP_1 = 1;
	public const int TYPE_SHOP_2 = 2;
	#endregion
	#region id_shop
	public const int ID_NONE_SHOP = 1;
	public const int ID_SHOP_RAMEN = 2;
	public const int ID_SHOP_BOBA = 3;
	public const int D_SHOP_FLORIST = 4;
	public const int ID_SHOP_BOOK = 5;
	public const int ID_SHOP_KONBINI = 6;
	public const int ID_SHOP_KARAOKE = 7;
	#endregion
	#region stats_upgrade
	public const int UPGRADE_YOUTH = 1;
	public const int UPGRADE_ELDERS = 2;
	public const int UPGRADE_GOBLIN = 3;
	public const int UPGRADE_BEAUTY = 4;
	public const int UPGRADE_LEVEL_SHOP = 5;
	#endregion

    }
