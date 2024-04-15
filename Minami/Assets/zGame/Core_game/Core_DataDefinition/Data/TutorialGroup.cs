using System;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;

namespace Lam.zGame.Core_game.Core_DataDefinition.Data
{
    public class TutorialGroup : DataGroup
    {
        public Action OnTutorialChanged;

        private BoolData m_UpgradeTalent;
        private BoolData m_SelectShop;
        private BoolData m_OpenTreasure;
        private BoolData m_SelectEquipment;
        private BoolData m_UpgradeWeapon;

        public bool UpgradeTalent => m_UpgradeTalent.Value;
        public bool SelectShop => m_SelectShop.Value;
        public bool OpenTreasure => m_OpenTreasure.Value;
        public bool SelectEquipment => m_SelectEquipment.Value;
        public bool UpgradeWeapon => m_UpgradeWeapon.Value;

        public TutorialGroup(int pId) : base(pId)
        {
            m_UpgradeTalent = AddData(new BoolData(1));
            m_SelectShop = AddData(new BoolData(2));
            m_OpenTreasure = AddData(new BoolData(3));
            m_SelectEquipment = AddData(new BoolData(4));
            m_UpgradeWeapon = AddData(new BoolData(5));
        }

        public bool IsPassAllTutorial()
        {
            return m_UpgradeTalent.Value && m_OpenTreasure.Value && m_SelectEquipment.Value && m_UpgradeWeapon.Value;
        }

        public void SetPassAllTutorial()
        {
            m_UpgradeTalent.Value = true;
            m_OpenTreasure.Value = true;
            m_SelectEquipment.Value = true;
            m_UpgradeWeapon.Value = true;
        }

        public void SetPassTutorialUpgradeTalent()
        {
            if(!m_UpgradeTalent.Value)
            {
                m_UpgradeTalent.Value = true;
                OnTutorialChanged?.Invoke();
            }    
        }

        public void SetPassTutorialSelectShop()
        {
            if(!m_SelectShop.Value)
            {
                m_SelectShop.Value = true;
                OnTutorialChanged?.Invoke();
            }    
        }

        public void SetPassTutorialOpenTreasure()
        {
            m_OpenTreasure.Value = true;
            OnTutorialChanged?.Invoke();
        }

        public void SetPassTutorialSelectEquipment()
        {
            if (!m_SelectEquipment.Value)
            {
                m_SelectEquipment.Value = true;
                OnTutorialChanged?.Invoke();
            }
        }

        public void SetPassTutorialUpgradeWeapon()
        {
            if(!m_UpgradeWeapon.Value)
            {
                m_UpgradeWeapon.Value = true;
                OnTutorialChanged?.Invoke();
            }    
        }
    }
}
