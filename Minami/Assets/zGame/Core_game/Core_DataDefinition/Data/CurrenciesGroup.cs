using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core_DataDefinition.Misc;
using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.Data;
using UnityEngine;
using Lam.zGame.Core_game.Core.Utilities.Common;


namespace Lam.zGame.Core_game.Core_DataDefinition.Data
{
    public class CurrencyData : DataGroup
    {
        public IntegerData mValue;
        public int Value => mValue.Value;

        public CurrencyData(int pId, int pDefaultVal = 0) : base(pId)
        {
            mValue = AddData(new IntegerData(0, pDefaultVal));
        }

        public void Add(int pValue)
        {
            mValue.Value += pValue;
            EventDispatcher.Raise(new CurrencyChangedEvent(Id, pValue, mValue.Value));
        }

        public bool Pay(int pValue)
        {
            if (mValue.Value >= pValue)
            {
                Add(-pValue);
                return true;
            }

            return false;
        }

        public void Set(int pValue)
        {
            if (mValue.Value == pValue)
                return;

            mValue.Value = pValue;
            EventDispatcher.Raise(new CurrencyChangedEvent(Id, pValue, mValue.Value));
        }
    }

    public class CurrenciesGroup : DataGroup
    {
        #region Members

        private DataGroup m_Currencies;
        private TimerTask m_StaminaRegenTimer;

        public long StaminaRegenCountdown => m_StaminaRegenTimer.RemainSeconds;

        #endregion

        //=====================================

        #region Public

        public CurrenciesGroup(int pId) : base(pId)
        {
            m_Currencies = AddData(new DataGroup(1));
            //m_Currencies.AddData(new CurrencyData(IDs.CURRENCY_GOLD, 500));
            //m_Currencies.AddData(new CurrencyData(IDs.CURRENCY_GEM));
            //m_Currencies.AddData(new CurrencyData(IDs.CURRENCY_STAMINA, Constants.MAX_STAMINA));
            //m_Currencies.AddData(new CurrencyData(IDs.MATERIAL_ARMOR));
            //m_Currencies.AddData(new CurrencyData(IDs.MATERIAL_ARMOR_PART));
            //m_Currencies.AddData(new CurrencyData(IDs.MATERIAL_WEAPON));
            //m_Currencies.AddData(new CurrencyData(IDs.MATERIAL_DRONE));
            //m_Currencies.AddData(new CurrencyData(IDs.CURRENCY_REVIVAL));
            //m_Currencies.AddData(new CurrencyData(IDs.MATERIAL_HERO));
            m_StaminaRegenTimer = AddData(new TimerTask(2, false));
        }

        public override void PostLoad()
        {
            base.PostLoad();

            //UnityMainThreadDispatcher.Instance().Enqueue(CheckFullStamina);
            CheckFullStamina();
        }

        #endregion

        //COMMON
        public void Set(int pId, int pValue)
        {
            foreach (CurrencyData currency in m_Currencies.Children)
            {
                if (currency.Id == pId)
                    currency.Set(pValue);
            }
        }

        public void Add(int pId, int pValue)
        {
            foreach (CurrencyData currency in m_Currencies.Children)
            {
                if (currency.Id == pId)
                    currency.Add(pValue);
            }
        }

        public bool Pay(int pId, int pValue)
        {
            foreach (CurrencyData currency in m_Currencies.Children)
            {
                if (currency.Id == pId)
                    return currency.Pay(pValue);
            }

            return false;
        }

        public int Value(int pId)
        {
            foreach (CurrencyData currency in m_Currencies.Children)
                if (currency.Id == pId)
                    return currency.Value;
            return 0;
        }

        public bool CanPay(int pId, int pValue)
        {
            //if (pId == IDs.CURRENCY_MONEY)
            //    return true;
            return Value(pId) >= pValue;
        }

        //REVIVAL
        public void AddRevival(int pValue)
        {
            //Add(IDs.CURRENCY_REVIVAL, pValue);
        }

        public void SetRevival(int pValue)
        {
            //Set(IDs.CURRENCY_REVIVAL, pValue);
        }

        public bool PayRevival(int pValue)
        {
            //return Pay(IDs.CURRENCY_REVIVAL, pValue);
            return false;
        }

        public bool CanPayRevival(int pValue)
        {
            //return CanPay(IDs.CURRENCY_REVIVAL, pValue);
            return false;
        }

        public int TotalRevival()
        {
            //return Value(IDs.CURRENCY_REVIVAL);
            return 0;
        }

        //GOLD
        public void AddGold(int pValue)
        {
            //Add(IDs.CURRENCY_GOLD, pValue);
        }

        public void SetGold(int pValue)
        {
            //Set(IDs.CURRENCY_GOLD, pValue);
        }

        public bool PayGold(int pValue)
        {
            //return Pay(IDs.CURRENCY_GOLD, pValue);
            return false;
        }

        public bool CanPayGold(int pValue)
        {
            //return CanPay(IDs.CURRENCY_GOLD, pValue);
            return false;
        }

        public int TotalGold()
        {
            //return Value(IDs.CURRENCY_GOLD);
            return 0;
        }

        //GEM
        public void AddGem(int pValue)
        {
            //Add(IDs.CURRENCY_GEM, pValue);
        }

        public void SetGem(int pValue)
        {
            //Set(IDs.CURRENCY_GEM, pValue);
        }

        public bool PayGem(int pValue)
        {
            //return Pay(IDs.CURRENCY_GEM, pValue);
            return false;
        }

        public bool CanPayGem(int pValue)
        {
            //return CanPay(IDs.CURRENCY_GEM, pValue);
            return false;
        }

        public int TotalGem()
        {
            //return Value(IDs.CURRENCY_GEM);
            return 0;
        }

        //STAMINA
        public void AddStamina(int pValue, bool pLimited)
        {
            if (pLimited)
            {
                int totalStamina = TotalStamina();
                //if (totalStamina + pValue > Constants.MAX_STAMINA)
                //    pValue = Constants.MAX_STAMINA - totalStamina;
            }

            //Add(IDs.CURRENCY_STAMINA, pValue);
            CheckFullStamina();
        }

        public void SetStamina(int pValue)
        {
            //if (pValue > Constants.MAX_STAMINA)
            //    pValue = Constants.MAX_STAMINA;
            //Set(IDs.CURRENCY_STAMINA, pValue);
            CheckFullStamina();
        }

        public bool PayStamina(int pValue)
        {
            return false;
            //bool paid = Pay(IDs.CURRENCY_STAMINA, pValue);
            //if (paid)
            //    CheckFullStamina();
            //return paid;
        }

        public bool CanPayStamina(int pValue)
        {
            //return CanPay(IDs.CURRENCY_STAMINA, pValue);
            return false;
        }

        public int TotalStamina()
        {
            //return Value(IDs.CURRENCY_STAMINA);
            return 0;
        }

        public void CheckFullStamina()
        {
            int stamina = TotalStamina();
            if (stamina <
                //Constants.MAX_STAMINA
                10
               )
            {

                //UnityEngine.Debug.LogError(m_StaminaRegenTimer.RemainSeconds + "  checkkkkk");
                if (!m_StaminaRegenTimer.IsRunning)
                {

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        //m_StaminaRegenTimer.Start(Constants.SECONDS_PER_STAMINA);
                        //int secondsTillFullStamina = Constants.SECONDS_PER_STAMINA * (Constants.MAX_STAMINA - stamina);
                        ////Should send 1 time per hour
                        //if (secondsTillFullStamina < 3600)
                        //    secondsTillFullStamina = 3600;
                        //SendReviveNotification(DateTime.Now.AddSeconds(secondsTillFullStamina));
                    });
                    //m_StaminaRegenTimer.Start(Constants.SECONDS_PER_STAMINA);
                    //int secondsTillFullStamina = Constants.SECONDS_PER_STAMINA * (Constants.MAX_STAMINA - stamina);
                    ////Should send 1 time per hour
                    //if (secondsTillFullStamina < 3600)
                    //    secondsTillFullStamina = 3600;
                    //SendReviveNotification(DateTime.Now.AddSeconds(secondsTillFullStamina));
                }

                m_StaminaRegenTimer.SetOnComplete((task, seconds) =>
                {
                    long modSeconds = 0;
                    //int passedSteps = 1 + TimeHelper.CalcTimeStepsPassed(-seconds, Constants.SECONDS_PER_STAMINA, out modSeconds);
                    //AddStamina(passedSteps, true);
                });
            }
            else
            {
                SendReviveNotification(DateTime.Now.AddHours(1));
                m_StaminaRegenTimer.Stop();
            }
        }

        public void SendReviveNotification(DateTime pTime)
        {
            //if (UnmanagedData.FullReviveNotification != -1)
            //{
            //    LocalNotificationHelper.Instance.CancelNotification(UnmanagedData.FullReviveNotification);
            //    UnmanagedData.FullReviveNotification = -1;
            //}
            //UnmanagedData.FullReviveNotification = LocalNotificationHelper.Instance.SendNotification("Fully recovered", "Our Hero, your Energy has refilled!", pTime);
            //UnmanagedData.LastTimeSendNotification = pTime;
        }

        //ARMOR MATERIAL
        public void AddArmorMat(int pValue)
        {
            //Add(IDs.MATERIAL_ARMOR, pValue);
        }

        public void SetArmorMat(int pValue)
        {
            //Set(IDs.MATERIAL_ARMOR, pValue);
        }

        public bool PayArmorMat(int pValue)
        {
            return false;
            //return Pay(IDs.MATERIAL_ARMOR, pValue);
        }

        public bool CanPayArmorMat(int pValue)
        {
            return false;
            //return CanPay(IDs.MATERIAL_ARMOR, pValue);
        }

        public int TotalArmorMat()
        {
            //return Value(IDs.MATERIAL_ARMOR);
            return 0;
        }

        //ARMOR PART MATERIAL
        public void AddArmorPartMat(int pValue)
        {
            //Add(IDs.MATERIAL_ARMOR_PART, pValue);
        }

        public void SetArmorPartMat(int pValue)
        {
            //Set(IDs.MATERIAL_ARMOR_PART, pValue);
        }

        public bool PayArmorPartMat(int pValue)
        {
            //return Pay(IDs.MATERIAL_ARMOR_PART, pValue);
            return false;
        }

        public bool CanPayArmorPartMat(int pValue)
        {
            //return CanPay(IDs.MATERIAL_ARMOR_PART, pValue);
            return false;
        }

        public int TotalArmorPartMat()
        {
            //return Value(IDs.MATERIAL_ARMOR_PART);
            return 0;
        }

        //WEAPON MATERIAL
        public void AddWeaponMat(int pValue)
        {
            //Add(IDs.MATERIAL_WEAPON, pValue);
        }

        public void SetWeaponMat(int pValue)
        {
            //Set(IDs.MATERIAL_WEAPON, pValue);
        }

        public bool PayWeaponMat(int pValue)
        {
            //return Pay(IDs.MATERIAL_WEAPON, pValue);
            return false;
        }

        public bool CanPayWeaponMat(int pValue)
        {
            //return CanPay(IDs.MATERIAL_WEAPON, pValue);
            return false;
        }

        public int TotalWeaponMat()
        {
            //return Value(IDs.MATERIAL_WEAPON);
            return 0;
        }

        //DRONE MATERIAL
        public void AddDroneMat(int pValue)
        {
            //Add(IDs.MATERIAL_DRONE, pValue);
        }

        public void SetDroneMat(int pValue)
        {
            //Set(IDs.MATERIAL_DRONE, pValue);
        }

        public bool PayDroneMat(int pValue)
        {
            //return Pay(IDs.MATERIAL_DRONE, pValue);
            return false;
        }

        public bool CanPayDroneMat(int pValue)
        {
            //return CanPay(IDs.MATERIAL_DRONE, pValue);
            return false;
        }

        public int TotalDroneMat()
        {
            //return Value(IDs.MATERIAL_DRONE);
            return 0;
        }

        // HERO MATERIAL
        public void AddHeroMat(int pValue)
        {
            //Add(IDs.MATERIAL_HERO, pValue);
        }

        public void SetHeroMat(int pValue)
        {
            //Set(IDs.MATERIAL_HERO, pValue);
        }

        public bool PayHeroMat(int pValue)
        {
            //return Pay(IDs.MATERIAL_HERO, pValue);
            return false;
        }

        public bool CanPayHeroMat(int pValue)
        {
            //return CanPay(IDs.MATERIAL_HERO, pValue);
            return false;
        }

        public int TotalHeroMat()
        {
            //return Value(IDs.MATERIAL_HERO);
            return 0;
        }

        //=====================================

        #region Private

        #endregion
    }
}