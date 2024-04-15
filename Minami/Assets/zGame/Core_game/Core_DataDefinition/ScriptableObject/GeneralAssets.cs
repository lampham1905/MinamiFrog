using Lam.zGame.Core_game.Core.Utilities.Common;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.ScriptableObject
{


    public class IconNames
    {
        public const string ICON_LOCK_FEATURE = "button_lock";
        public const string ICON_AMULET = "";
        public const string ICON_ARMOR = "";
        public const string ICON_BOOT = "";
        public const string ICON_HELM = "";
        public const string ICON_PIECE = "";
        public const string ICON_WEAPON = "";
    }

    [CreateAssetMenu(fileName = "GeneralAssets", menuName = "OTS/Create General Assets")]
    public class GeneralAssets : UnityEngine.ScriptableObject
    {
        #region Members

        private static GeneralAssets m_Instance;
        public static GeneralAssets Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = Resources.Load<GeneralAssets>($"GeneralAssets");
                return m_Instance;
            }
        }

        public SpritesList heroIcons;
        public SpritesList heroIconLocks;
        public SpritesList heroIconSmall;
        public SpritesList enemyIcons;
        public SpritesList currencyIcons;
        public SpritesList commonIcons;
        public SpritesList skillIcons;
        public SpritesList equipmentIcons;
        public SpritesList mapIcons;
        public SpritesList rarityWeaponSlots;
        public SpritesList raritySlots;
        public SpritesList raritySlotsUpLayout;
        public SpritesList weaponRarityIcons;
        public SpritesList shopItemIcons;
        public SpritesList chestIcons;
        public SpritesList talentIcons;
        public Color[] rarityLightColors;
        public Color[] rarityDarkColors;
        public SpritesList perkGroupIcons;

        #endregion

        //=====================================

        #region Public

        #endregion

        //=====================================

        #region Private

        #endregion

        //=====================================

#if UNITY_EDITOR
        [CustomEditor(typeof(GeneralAssets))]
        private class GeneralAssetsEditor : Editor
        {
            private GeneralAssets m_Script;
            private string m_Tab;

            private void OnEnable()
            {
                m_Script = target as GeneralAssets;
            }

            public override void OnInspectorGUI()
            {
                m_Tab = EditorHelper.Tabs(m_Script.name, "Default", "Custom");
                switch (m_Tab)
                {
                    case "Default":
                        base.OnInspectorGUI();
                        break;

                    case "Custom":
                        m_Script.enemyIcons.DrawInEditor("Enemy Icons");
                        m_Script.heroIcons.DrawInEditor("Hero Icons");
                        m_Script.heroIconSmall.DrawInEditor("Hero Icons Small");
                        m_Script.currencyIcons.DrawInEditor("Currency Icons");
                        m_Script.commonIcons.DrawInEditor("Common Icons");
                        m_Script.skillIcons.DrawInEditor("Skill Icons");
                        m_Script.equipmentIcons.DrawInEditor("Equipment Icons");
                        m_Script.mapIcons.DrawInEditor("Map Icons");
                        m_Script.raritySlots.DrawInEditor("Rarity Slots");
                        m_Script.raritySlotsUpLayout.DrawInEditor("Rarity Slots Up Layout");
                        
                        m_Script.rarityWeaponSlots.DrawInEditor("Rarity Weapon Slots");
                        m_Script.weaponRarityIcons.DrawInEditor("Weapon Rarity Icons");
                        m_Script.shopItemIcons.DrawInEditor("Shop Item Icons");
                        m_Script.chestIcons.DrawInEditor("Chest Icons");
                        m_Script.talentIcons.DrawInEditor("Talent Icons");
                        m_Script.perkGroupIcons.DrawInEditor("Perk Group Icons");
                        break;
                }

                if (EditorHelper.ButtonColor("Save", Color.green))
                {
                    EditorUtility.SetDirty(m_Script);
                    AssetDatabase.SaveAssets();
                }
            }

            [MenuItem("OTS/General Assets")]
            private static void Open()
            {
                Selection.activeObject = GeneralAssets.Instance;
            }
        }
#endif
    }
}