using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Misc
{
    [System.Serializable]
    public class Stats
    {
        public Dictionary<int, float[]> mods = new Dictionary<int, float[]>();
        public Action<Mod> onModChanged;
        public Action<List<Mod>> onModsChanged;
        public virtual void AddMods(List<Mod> pMods, bool pSendEvent = true, bool pPostProress = true)
        {
            for (int i = 0; i < pMods.Count; i++)
                AddMod(pMods[i], false, pPostProress);

            if (pSendEvent)
                onModsChanged?.Invoke(pMods);
        }
        public virtual bool AddMod(Mod pMod, bool pSendEvent = true, bool pPostProress = true)
        {
            if (pPostProress)
            {
                if (mods.ContainsKey(pMod.id))
                {
                    for (int i = 0; i < pMod.values.Length; i++)
                    {
                        mods[pMod.id][i] += pMod.values[i];
                    }
                }
                else
                    mods.Add(pMod.id, pMod.values);
            }

            if (pSendEvent)
                onModChanged?.Invoke(pMod);

            return false;
        }
        public virtual void RemoveMods(List<Mod> pMods, bool pSendEvent = true, bool pPostProress = true)
        {
            for (int i = 0; i < pMods.Count; i++)
                RemoveMod(pMods[i], false, pPostProress);

            if (pSendEvent)
                onModsChanged?.Invoke(pMods);
        }
        public virtual bool RemoveMod(Mod pMod, bool pSendEvent = true, bool pPostProress = true)
        {
            if (pPostProress)
            {
                if (mods.ContainsKey(pMod.id))
                    for (int i = 0; i < pMod.values.Length; i++)
                        mods[pMod.id][i] -= pMod.values[i];
            }
            if (pSendEvent)
            {
                if (pSendEvent)
                    onModChanged?.Invoke(pMod);
            }

            return true;
        }
    }

    public class TotalStats : Stats
    {
        //this is a custom mod, because of atk formula is so complicated
        //that break the consistent of Mods System
        public const int MOD_ATK_MULTIPLIER_FROM_WEAPON = 1000;
        //----------------------------------------------------
        public float maximumHPBase;
        public float maximumHPInc;
        public float MaximumHPBonus => maximumHPBase * maximumHPInc / 100f;
        public float MaximumHPTotal => maximumHPBase + MaximumHPBonus;
        //----------------------------------------------------
        //Attack is very complicated so we dont calculate here
        public float atkBase;
        public float atkInc;
        public float AtkBonus => atkBase * atkInc / 100f;
        public float atkPlus; //+#Total
        public float atkPlusToMeleeUnit; //+#Total
        public float atkPlusToRangedUnit; //+#Total
        public float atkPlusToFlyUnit; //+#Total
        public float atkPlusToNomralUnit; //+#Total
        public float atkPlusToBossUnit; //+#Total
        public float atkPlusWithShotgun; //+#Total
        public float atkPlusWithAssRifle; //+#Total
        public float atkPlusWithRevolver; //+#Total
        public float[] atkIncWithinRange = new float[2] { 0, 0 };
        public float AtkBonusWithinRange => atkBase * atkIncWithinRange[0] / 100f;
        public float atkIncWhenFullHP;
        public float AtkBonusWhenFullHP => atkBase * atkIncWhenFullHP / 100;
        //This is a custom mod, because of atk formula is so complicated, it break the consistent of Mods System
        //Therefore, it required a custom mod
        public float atkMultiplierFromWeapon;
        //----------------------------------------------------
        public float critChanceBase;
        public float critChanceInc;
        public float CritChanceBonus => critChanceBase * critChanceInc / 100f;
        public float CritChanceTotal => critChanceBase + CritChanceBonus;
        //----------------------------------------------------
        public float critMultiplierBase;
        public float critMultiplierInc;
        public float CritMultiplierBonus => critMultiplierBase * critMultiplierInc / 100f;
        public float CritMultiplierTotal => critMultiplierBase + CritMultiplierBonus;
        //----------------------------------------------------
        public float moveSpeedBase;
        public float moveSpeedInc;
        public float moveSpeedMultiplier;
        public float MoveSpeedBonus => moveSpeedBase * moveSpeedInc / 100f;
        public float MoveSpeedTotal => (moveSpeedBase + MoveSpeedBonus) * moveSpeedMultiplier;
        //----------------------------------------------------
        public float attackSpeedBase;
        public float attackSpeedInc;
        public float AttackSpeedBonus => attackSpeedBase * attackSpeedInc / 100f;
        public float AttackSpeedTotal => attackSpeedBase + AttackSpeedBonus;
        //----------------------------------------------------
        public float dogeChance;
        public float dmgResistance;
        public float coinDropInc;
        public float healingEffency;
        public float velocity;
        //----------------------------------------------------
        public float droneAtk;
        public float droneAtkInc;
        public float DroneAtkBonus => droneAtk * droneAtkInc / 100f;
        public float DroneAtkTotal => droneAtk + DroneAtkBonus;
        //----------------------------------------------------
        public float attackRange;
        public float attackRangeInc;
        public float AttackRangeBonus => attackRange * attackRangeInc / 100;
        public float AttackRangeTotal => attackRange + AttackRangeBonus;
        //----------------------------------------------------
        public float fireRate;
        public float fireRateInc;
        public float FireRateBonus => fireRate * fireRateInc / 100;
        public float FireRateTotal => fireRate + FireRateBonus;
        //----------------------------------------------------
        public float accuracy;
        public float accuracyInc;
        public float AccuracyBonus => accuracy * accuracyInc / 100;
        public float AccuracyTotal => accuracy + AccuracyBonus;
        //----------------------------------------------------
        public int magazine;
        public float magazineInc;
        public int MagazineBonus => Mathf.RoundToInt(magazine * magazineInc / 100);
        public float MagazineTotal => magazine + MagazineBonus;
        //----------------------------------------------------
        public float reloadTime;
        public float reloadTimeInc;
        public float ReloadTimeBonus => reloadTime * reloadTimeInc / 100;
        public float ReloadTimeTotal => reloadTime + ReloadTimeBonus;
        //----------------------------------------------------
        public int pellet;
        public int pelletInc;
        public int PeleetBonus => pellet * pelletInc / 100;
        public int PeleetTotal => pellet + PeleetBonus;
        //----------------------------------------------------
        public float[] shockwaveStrikeBack = new float[2] { 0, 0 };
        public float[] lightningStrikeRandom = new float[2] { 0, 0 };
        public float[] attackSpeedIncWhenCrit = new float[2] { 0, 0 };
        public float aoeAtkInc;
        public float[] burn = new float[3] { 0, 0, 0 };
        public float[] frezee = new float[2] { 0, 0 };
        public int explodeOneMoreTime;
        public float[] burnTheGround = new float[2] { 0, 0 };
        public float slowedTargetDmgResistanceReduced;
        //----------------------------------------------------
        public float burstCooldown;
        public int burst;
        //----------------------------------------------------
        public int exchangePelletsForAtk;
        public float instantReloadWhenNoShoot;
        public float[] theOutlaw = new float[2] { 0, 0 };
        public float[] reloadTimeIncWhenHpUnder = new float[2] { 0, 0 }; //reload time inc, % hp required
        public float[] theRampage = new float[3] { 0, 0, 0 };
        public float[] theRoadborn = new float[2] { 0, 0 };
        public float meleeArmor;
        public float stageRecover;
        public float itemRecover; //BASE VALUE: Increase the % amount of recoved
        public float baseEquipmentModInc; //INC VALUE: Increase basic stats of character by %
        public float coinOffline; //BASE VALUE: Increase coin gain per minute
        public float startingCoin; //BASE VALUE:Increase the amount of fixed coin gain
        public float revivedHp; //BASE VALUE: Increase the amount of % revived total hp

        public void AddMods(List<Mod> pMods)
        {
            for (int i = 0; i < pMods.Count; i++)
                AddMod(pMods[i]);
        }

        public override bool AddMod(Mod pMod, bool pSendEvent = true, bool pPostProress = true)
        {
            bool added = base.AddMod(pMod, false, false);
            if (!added)
            {
                added = true;
                bool duplicated = false;
                //switch (pMod.id)
                //{
                //    case IDs.MOD_MAXIMUM_HP: maximumHPBase += pMod.value; break;
                //    case IDs.MOD_MAXIMUM_HP_INC: maximumHPInc += pMod.value; break;
                //    case IDs.MOD_ATK: atkBase += pMod.value; break;
                //    case IDs.MOD_ATK_INC: atkInc += pMod.value; break;
                //    case IDs.MOD_CRITICAL_CHANCE: critChanceBase += pMod.value; break;
                //    case IDs.MOD_CRITICAL_CHANCE_INC: critChanceInc += pMod.value; break;
                //    case IDs.MOD_CRITICAL_ATK: critMultiplierBase += pMod.value; break;
                //    case IDs.MOD_CRITICAL_ATK_INC: critMultiplierInc += pMod.value; break;
                //    case IDs.MOD_MOVE_SPEED: moveSpeedBase += pMod.value; break;
                //    case IDs.MOD_MOVE_SPEED_INC: moveSpeedInc += pMod.value; break;
                //    case IDs.MOD_DODGE_CHANCE: dogeChance += pMod.value; break;
                //    case IDs.MOD_ATTACK_SPEED: attackSpeedBase += pMod.value; break;
                //    case IDs.MOD_ATTACK_SPEED_INC: attackSpeedInc += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS: atkPlus += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_MELEE_UNITS: atkPlusToMeleeUnit += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_RANGED_UNITS: atkPlusToRangedUnit += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_FLYING_UNITS: atkPlusToFlyUnit += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_NORMAL_UNITS: atkPlusToNomralUnit += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_BOSS: atkPlusToBossUnit += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_BY_SHOTGUN: atkPlusWithShotgun += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_BY_ASS_RIFLE: atkPlusWithAssRifle += pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_BY_REVOLVER: atkPlusWithRevolver += pMod.value; break;
                //    case IDs.MOD_RESIST_DMG: dmgResistance += pMod.value; break;
                //    case IDs.MOD_COIN_DROP_INC: coinDropInc += pMod.value; break;
                //    case IDs.MOD_HEALING_EFFIENCY: healingEffency += pMod.value; break;
                //    case IDs.MOD_ATK_INC_WITHIN_RANGE: // NOTE: this mod should not be stackable
                //        if (atkIncWithinRange[0] > 0) duplicated = true;
                //        atkIncWithinRange[0] += pMod.values[0]; // % increased value to base attack
                //        atkIncWithinRange[1] += pMod.values[1]; // % attack range in total attack range
                //        break;
                //    case IDs.MOD_DRONE_ATK: droneAtk += pMod.value; break;
                //    // % increased value to base drone attack
                //    case IDs.MOD_DRONE_ATK_INC: droneAtkInc += pMod.value; break;
                //    case IDs.MOD_SHOCKWAVE_STRIKE_BACK: // NOTE: this mod should not be stackable
                //        if (shockwaveStrikeBack[0] > 0) duplicated = true;
                //        shockwaveStrikeBack[0] += pMod.values[0]; // % of hp in total Hp which converts to atk value
                //        shockwaveStrikeBack[1] += pMod.values[1]; // cooldown time in seconds
                //        break;
                //    case IDs.MOD_LIGHTNING_STRIKE_RANDOM:
                //        if (lightningStrikeRandom[0] > 0) duplicated = true;
                //        lightningStrikeRandom[0] += pMod.values[0]; // % of attack in total attack value
                //        lightningStrikeRandom[1] += pMod.values[1]; // cooldown time in seconds
                //        break;
                //    case IDs.MOD_ATTACK_SPEED_INC_WHEN_CRIT:
                //        if (attackSpeedIncWhenCrit[0] > 0) duplicated = true;
                //        attackSpeedIncWhenCrit[0] += pMod.values[0]; // % increased value to base attack speed
                //        attackSpeedIncWhenCrit[1] += pMod.values[1]; // duration in seconds
                //        break;
                //    // % increased value to base aoe attack
                //    case IDs.MOD_AOE_OF_ATTACK_INC: aoeAtkInc += pMod.value; break;
                //    case IDs.MOD_BURN:
                //        if (burn[0] > 0) duplicated = true;
                //        burn[0] += pMod.values[0]; // % of attack in total burn attack value
                //        burn[1] += pMod.values[1]; // max ticks
                //        burn[2] += pMod.values[2]; // tick delay in seconds
                //        break;
                //    case IDs.MOD_FREZEE:
                //        frezee[0] += pMod.values[0]; // reduced dmg resistance of freezed/slowed target
                //        frezee[1] += pMod.values[1]; // frezee duration in seconds
                //        break;
                //    case IDs.MOD_EXPLODE_ONE_MORE_TIME: explodeOneMoreTime += (int)pMod.value; break;
                //    case IDs.MOD_BURN_THE_GROUND:
                //        if (burnTheGround[0] > 0) duplicated = true;
                //        burnTheGround[0] += pMod.values[0]; // % of attack in total burn attack value
                //        burnTheGround[1] += pMod.values[1]; // burn duration in seconds
                //        break;
                //    // dmg resistance reduced of freezed / slowed target
                //    case IDs.MOD_SLOWED_UNIT_TAKE_MORE_DAMAGE: slowedTargetDmgResistanceReduced += pMod.value; break;
                //    case IDs.MOD_ATTACK_RANGE: attackRange += pMod.value; break;
                //    case IDs.MOD_ATTACK_RANGE_INC: attackRangeInc += pMod.value; break;
                //    case IDs.MOD_FIRE_RATE: fireRate += pMod.value; break;
                //    case IDs.MOD_FIRE_RATE_INC: fireRateInc += pMod.value; break;
                //    case IDs.MOD_ACCURACY: accuracy += pMod.value; break;
                //    case IDs.MOD_ACCURACY_INC: accuracyInc += pMod.value; break;
                //    case IDs.MOD_VELOCITY: velocity += pMod.value; break;
                //    case IDs.MOD_MAGAZINE: magazine += Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_MAGAZINE_INC: magazineInc += pMod.value; break;
                //    case IDs.MOD_RELOAD_TIME: reloadTime += pMod.value; break;
                //    case IDs.MOD_RELOAD_TIME_INC: reloadTimeInc += pMod.value; break;
                //    case IDs.MOD_BURST_COOLDOWN: burstCooldown += pMod.value; break;
                //    case IDs.MOD_BURST: burst += Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_PELLET: pellet += Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_PELLET_INC: pelletInc += Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_EXCHANGE_PELETS_FOR_MORE_ATK: exchangePelletsForAtk += (int)pMod.value; break;
                //    case IDs.MOD_ATK_INC_WHEN_FULL_HP: atkIncWhenFullHP += pMod.value; break;
                //    case IDs.MOD_INSTANT_RELOAD_WHEN_NO_SHOOT:
                //        if (instantReloadWhenNoShoot > 0) duplicated = true;
                //        instantReloadWhenNoShoot += pMod.value;
                //        break;
                //    case IDs.MOD_OUTLAW:
                //        if (theOutlaw[0] > 0) duplicated = true;
                //        theOutlaw[0] = pMod.values[0];
                //        theOutlaw[1] = pMod.values[1];
                //        break;
                //    case IDs.MOD_RELOAD_TIME_INC_WHEN_HP_UNDER:
                //        reloadTimeIncWhenHpUnder[0] += pMod.values[0];
                //        reloadTimeIncWhenHpUnder[1] += pMod.values[1];
                //        break;
                //    case IDs.MOD_RAMPAGE:
                //        if (theRampage[0] > 0) duplicated = true;
                //        theRampage[0] += pMod.values[0];
                //        theRampage[1] += pMod.values[1];
                //        theRampage[2] += pMod.values[2];
                //        break;
                //    case IDs.MOD_ROADBORN:
                //        if (theRoadborn[0] > 0) duplicated = true;
                //        theRoadborn[0] += pMod.values[0];
                //        theRoadborn[1] += pMod.values[1];
                //        break;
                //    case IDs.MOD_ARMOR_MELEE: meleeArmor += pMod.value; break;
                //    case IDs.MOD_STAGE_RECOVER: stageRecover += pMod.value; break;
                //    case IDs.MOD_ITEM_RECOVER: itemRecover += pMod.value; break;
                //    case IDs.MOD_EQUIPMENT_STATS_INC: baseEquipmentModInc += pMod.value; break;
                //    case IDs.MOD_COIN_OFFLINE: coinOffline += pMod.value; break;
                //    case IDs.MOD_STARTING_COIN: startingCoin += pMod.value; break;
                //    case IDs.MOD_REVIVED_HP: revivedHp += pMod.value; break;
                //    case IDs.MOD_MOVE_SPEED_MULTIPLIER: moveSpeedMultiplier += pMod.value; break;
                //    case TotalStats.MOD_ATK_MULTIPLIER_FROM_WEAPON: atkMultiplierFromWeapon += pMod.value; break;
                //    default: added = false; break;
                //}

                if (duplicated)
                    Debug.LogError("This mod should not be stackable, please make a little check!");
            }

            //if (pPostProress)
            //{
            //    if (mods.ContainsKey(pMod.id))
            //    {
            //        for (int i = 0; i < pMod.values.Length; i++)
            //            mods[pMod.id][i] += pMod.values[i];
            //    }
            //    else
            //        mods.Add(pMod.id, pMod.values);
            //}

            //if (pSendEvent)
            //    onModChanged?.Invoke(pMod);

            return added;
        }

        public override bool RemoveMod(Mod pMod, bool pSendEvent = true, bool pPostProress = true)
        {
            bool removed = base.RemoveMod(pMod, false, false);
            if (!removed)
            {
                //removed = true;
                //switch (pMod.id)
                //{
                //    case IDs.MOD_MAXIMUM_HP: maximumHPBase -= pMod.value; break;
                //    case IDs.MOD_MAXIMUM_HP_INC: maximumHPInc -= pMod.value; break;
                //    case IDs.MOD_ATK: atkBase -= pMod.value; break;
                //    case IDs.MOD_ATK_INC: atkInc -= pMod.value; break;
                //    case IDs.MOD_CRITICAL_CHANCE: critChanceBase -= pMod.value; break;
                //    case IDs.MOD_CRITICAL_CHANCE_INC: critChanceInc -= pMod.value; break;
                //    case IDs.MOD_CRITICAL_ATK: critMultiplierBase -= pMod.value; break;
                //    case IDs.MOD_CRITICAL_ATK_INC: critMultiplierInc -= pMod.value; break;
                //    case IDs.MOD_MOVE_SPEED: moveSpeedBase -= pMod.value; break;
                //    case IDs.MOD_MOVE_SPEED_INC: moveSpeedInc -= pMod.value; break;
                //    case IDs.MOD_DODGE_CHANCE: dogeChance -= pMod.value; break;
                //    case IDs.MOD_ATTACK_SPEED: attackSpeedBase -= pMod.value; break;
                //    case IDs.MOD_ATTACK_SPEED_INC: attackSpeedInc -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS: atkPlus -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_MELEE_UNITS: atkPlusToMeleeUnit -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_RANGED_UNITS: atkPlusToRangedUnit -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_FLYING_UNITS: atkPlusToFlyUnit -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_NORMAL_UNITS: atkPlusToNomralUnit -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_TO_BOSS: atkPlusToBossUnit -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_BY_SHOTGUN: atkPlusWithShotgun -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_BY_ASS_RIFLE: atkPlusWithAssRifle -= pMod.value; break;
                //    case IDs.MOD_ATK_PLUS_BY_REVOLVER: atkPlusWithRevolver -= pMod.value; break;
                //    case IDs.MOD_RESIST_DMG: dmgResistance -= pMod.value; break;
                //    case IDs.MOD_COIN_DROP_INC: coinDropInc -= pMod.value; break;
                //    case IDs.MOD_HEALING_EFFIENCY: healingEffency -= pMod.value; break;
                //    case IDs.MOD_ATK_INC_WITHIN_RANGE: // NOTE: this mod should not be stackable
                //        atkIncWithinRange[0] -= pMod.values[0]; // % increased value to base attack
                //        atkIncWithinRange[1] -= pMod.values[1]; // % attack range in total attack range
                //        break;
                //    case IDs.MOD_DRONE_ATK: droneAtk -= pMod.value; break;
                //    // % increased value to base drone attack
                //    case IDs.MOD_DRONE_ATK_INC: droneAtkInc -= pMod.value; break;
                //    case IDs.MOD_SHOCKWAVE_STRIKE_BACK: // NOTE: this mod should not be stackable
                //        shockwaveStrikeBack[0] -= pMod.values[0]; // % of hp in total Hp which converts to atk value
                //        shockwaveStrikeBack[1] -= pMod.values[1]; // cooldown time in seconds
                //        break;
                //    case IDs.MOD_LIGHTNING_STRIKE_RANDOM:
                //        lightningStrikeRandom[0] -= pMod.values[0]; // % of attack in total attack value
                //        lightningStrikeRandom[1] -= pMod.values[1]; // cooldown time in seconds
                //        break;
                //    case IDs.MOD_ATTACK_SPEED_INC_WHEN_CRIT:
                //        attackSpeedIncWhenCrit[0] -= pMod.values[0]; // % increased value to base attack speed
                //        attackSpeedIncWhenCrit[1] -= pMod.values[1]; // duration in seconds
                //        break;
                //    // % increased value to base aoe attack
                //    case IDs.MOD_AOE_OF_ATTACK_INC: aoeAtkInc -= pMod.value; break;
                //    case IDs.MOD_BURN:
                //        burn[0] -= pMod.values[0]; // % of attack in total burn attack value
                //        burn[1] -= pMod.values[1]; // max ticks
                //        burn[2] -= pMod.values[2]; // tick delay in seconds
                //        break;
                //    case IDs.MOD_FREZEE:
                //        frezee[0] -= pMod.values[0]; // reduced dmg resistance of freezed/slowed target
                //        frezee[1] -= pMod.values[1]; // frezee duration in seconds
                //        break;
                //    case IDs.MOD_EXPLODE_ONE_MORE_TIME: explodeOneMoreTime -= (int)pMod.value; break;
                //    case IDs.MOD_BURN_THE_GROUND:
                //        burnTheGround[0] -= pMod.values[0]; // % of attack in total burn attack value
                //        burnTheGround[1] -= pMod.values[1]; // burn duration in seconds
                //        break;
                //    // dmg resistance reduced of freezed / slowed target
                //    case IDs.MOD_SLOWED_UNIT_TAKE_MORE_DAMAGE: slowedTargetDmgResistanceReduced -= pMod.value; break;
                //    case IDs.MOD_ATTACK_RANGE: attackRange -= pMod.value; break;
                //    case IDs.MOD_ATTACK_RANGE_INC: attackRangeInc -= pMod.value; break;
                //    case IDs.MOD_FIRE_RATE: fireRate -= pMod.value; break;
                //    case IDs.MOD_FIRE_RATE_INC: fireRateInc -= pMod.value; break;
                //    case IDs.MOD_ACCURACY: accuracy -= pMod.value; break;
                //    case IDs.MOD_ACCURACY_INC: accuracyInc -= pMod.value; break;
                //    case IDs.MOD_VELOCITY: velocity -= pMod.value; break;
                //    case IDs.MOD_MAGAZINE: magazine -= Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_MAGAZINE_INC: magazineInc -= pMod.value; break;
                //    case IDs.MOD_RELOAD_TIME: reloadTime -= pMod.value; break;
                //    case IDs.MOD_RELOAD_TIME_INC: reloadTimeInc -= pMod.value; break;
                //    case IDs.MOD_BURST_COOLDOWN: burstCooldown -= pMod.value; break;
                //    case IDs.MOD_BURST: burst -= Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_PELLET: pellet -= Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_PELLET_INC: pelletInc -= Mathf.FloorToInt(pMod.value); break;
                //    case IDs.MOD_EXCHANGE_PELETS_FOR_MORE_ATK: exchangePelletsForAtk -= (int)pMod.value; break;
                //    case IDs.MOD_ATK_INC_WHEN_FULL_HP: atkIncWhenFullHP -= pMod.value; break;
                //    case IDs.MOD_INSTANT_RELOAD_WHEN_NO_SHOOT: instantReloadWhenNoShoot -= pMod.value; break;
                //    case IDs.MOD_OUTLAW:
                //        theOutlaw[0] = pMod.values[0];
                //        theOutlaw[1] = pMod.values[1];
                //        break;
                //    case IDs.MOD_RELOAD_TIME_INC_WHEN_HP_UNDER:
                //        reloadTimeIncWhenHpUnder[0] -= pMod.values[0];
                //        reloadTimeIncWhenHpUnder[1] -= pMod.values[1];
                //        break;
                //    case IDs.MOD_RAMPAGE:
                //        theRampage[0] -= pMod.values[0];
                //        theRampage[1] -= pMod.values[1];
                //        theRampage[2] -= pMod.values[2];
                //        break;
                //    case IDs.MOD_ROADBORN:
                //        theRoadborn[0] -= pMod.values[0];
                //        theRoadborn[1] -= pMod.values[1];
                //        break;
                //    case IDs.MOD_ARMOR_MELEE: meleeArmor -= pMod.value; break;
                //    case IDs.MOD_STAGE_RECOVER: stageRecover -= pMod.value; break;
                //    case IDs.MOD_ITEM_RECOVER: itemRecover -= pMod.value; break;
                //    case IDs.MOD_EQUIPMENT_STATS_INC: baseEquipmentModInc -= pMod.value; break;
                //    case IDs.MOD_COIN_OFFLINE: coinOffline -= pMod.value; break;
                //    case IDs.MOD_STARTING_COIN: startingCoin -= pMod.value; break;
                //    case IDs.MOD_REVIVED_HP: revivedHp -= pMod.value; break;
                //    case IDs.MOD_MOVE_SPEED_MULTIPLIER: moveSpeedMultiplier -= pMod.value; break;
                //    case TotalStats.MOD_ATK_MULTIPLIER_FROM_WEAPON: atkMultiplierFromWeapon -= pMod.value; break;
                //    default: removed = false; break;
                //}
            }
            if (pPostProress)
            {
                if (mods.ContainsKey(pMod.id))
                    for (int i = 0; i < pMod.values.Length; i++)
                        mods[pMod.id][i] -= pMod.values[i];
            }
            if (pSendEvent)
            {
                if (pSendEvent)
                    onModChanged?.Invoke(pMod);
            }
            return removed;
        }

        /// <summary>
        /// @To anh Đại
        /// Công thức tính atk
        /// Công thức này chỉ bao gồm các chỉ số đá biết trước, được thu thập từ nhân vật và trang bị
        /// Công thức này / hàm này có thể sử dụng lại trong battle, với điều kiện là các mod TotalStats cũng phải được cập nhật theo battle
        /// </summary>
        public float GetAtk(int pEType, int pECompbatType, int pEMovementType, int pWeaponId)
        {
            float totalAtk = atkBase + atkBase * atkInc / 100;

            //if (pEType == IDs.E_BOSS)
            //    totalAtk += atkPlusToBossUnit;

            //if (pECompbatType == IDs.CB_MELEE)
            //    totalAtk += atkPlusToMeleeUnit;
            //else if (pECompbatType == IDs.CB_RANGE)
            //    totalAtk += atkPlusToRangedUnit;

            //if (pEMovementType == IDs.MOVE_FLY)
            //    totalAtk += atkPlusToFlyUnit;

            //if (pWeaponId == IDs.WP_ASS_RIFLE)
            //    totalAtk += atkPlusWithAssRifle;
            //else if (pWeaponId == IDs.WP_REVOLVER)
            //    totalAtk += atkPlusWithRevolver;
            //else if (pWeaponId == IDs.WP_SHOTGUN)
            //    totalAtk += atkPlusWithShotgun;

            totalAtk += atkPlus;
            totalAtk *= atkMultiplierFromWeapon;
            return totalAtk;
        }

        public float GetTotalAtkToDisplay(int pWeaponId)
        {
            float totalAtk = atkBase;
            //float totalAtk = atkBase + atkBase * atkInc / 100;

            //if (pWeaponId == IDs.WP_ASS_RIFLE)
            //    totalAtk += atkPlusWithAssRifle;
            //else if (pWeaponId == IDs.WP_REVOLVER)
            //    totalAtk += atkPlusWithRevolver;
            //else if (pWeaponId == IDs.WP_SHOTGUN)
            //    totalAtk += atkPlusWithShotgun;

            //totalAtk += atkPlus;

            //if (atkMultiplierFromWeapon > 0)
            //    totalAtk = totalAtk * atkMultiplierFromWeapon;

            return totalAtk;
        }
    }

    public class EnemyStats : TotalStats
    {
        public int combatType;
        public int movementType;
        public int enemyType;
        public float attackCooldownBase;
        public float[] attackRanges;

        //public EnemyStats(EnemyDefinition pData)
        //{
        //    combatType = pData.combatType;
        //    movementType = pData.movementType;
        //    enemyType = pData.enemyType;
        //    maximumHPBase = pData.hp;
        //    atkBase = pData.atk;
        //    moveSpeedBase = pData.moveSpeed;
        //    attackCooldownBase = pData.atkCooldown;
        //    attackRanges = pData.atkRanges;
        //}
    }

    public class HeroStats : TotalStats
    {
    }
}