using Lam.zGame.Core_game.Core.Utilities.Common.Other;

namespace Lam.zGame.Core_game.Core_DataDefinition.Misc
{
    public class CurrencyChangedEvent : BaseEvent
    {
        public int id;
        public int addedValue;
        public int newValue;
        public CurrencyChangedEvent(int pId, int pAddedValue, int pNewValue)
        {
            id = pId;
            addedValue = pAddedValue;
            newValue = pNewValue;
        }
    }
    public class MapUnlockedEvent : BaseEvent
    {
        public int id;
        public MapUnlockedEvent(int pId) { id = pId; }
    }
    public class MapCompletedEvent : BaseEvent
    {
        public int id;
        public MapCompletedEvent(int pId) { id = pId; }
    }
    public class HeroUnlockedEvent : BaseEvent
    {
        public int id;
        public HeroUnlockedEvent(int pId) { id = pId; }
    }
    public class EquipmentCraftedEvent : BaseEvent
    {
        public int id;
        public EquipmentCraftedEvent(int pId) { id = pId; }
    }
    public class EquipmentUpgradedEvent : BaseEvent
    {
        public int id;
        public EquipmentUpgradedEvent(int pId) { id = pId; }
    }
    public class EquipmentEquippedEvent : BaseEvent
    {
        public int id;
        public int slot;
        public EquipmentEquippedEvent(int pId, int pSlot) { id = pId; slot = pSlot; }
    }
    public class EquipmentUnEquippedEvent : BaseEvent
    {
        public int id;
        public int slot;
        public EquipmentUnEquippedEvent(int pId, int pSlot) { id = pId; slot = pSlot; }
    }
    public class FeatureUnlockedEvent : BaseEvent
    {
        public int id;
        public FeatureUnlockedEvent(int pId) { id = pId; }
    }
    public class PlayerLevelChangedEvent : BaseEvent { }
    public class PlayerExpChangedEvent : BaseEvent { }
    public class NewDayCheckinEvent : BaseEvent { }
    public class MissionChangedEvent : BaseEvent { }
}