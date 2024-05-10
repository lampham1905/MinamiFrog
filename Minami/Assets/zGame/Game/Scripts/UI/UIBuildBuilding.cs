using System;
using System.Collections;
using System.Collections.Generic;
using Lam;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
using ntDev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildBuilding : PanelController
{
   
   [SerializeField] private TextMeshProUGUI txtName;
   [SerializeField] private TextMeshProUGUI txtDes;
   [SerializeField] private TextMeshProUGUI txtPrice;

   [SerializeField] private JustButton btnBuild;
   [SerializeField] private JustButton btnNext;
   [SerializeField] private JustButton btnPre;
   [SerializeField] private JustButton btnClose;
   private int index = 0;
   protected override void Awake()
   {
      base.Awake();
      
   }

   

   private void Start()
   {
      btnNext.onClick.AddListener(Next);
      btnPre.onClick.AddListener(Prev);
      btnBuild.onClick.AddListener(Build);
      btnClose.onClick.AddListener(Close);
   }

   public void ShowAddBuild(int index)
   {
      ManagerEvent.RaiseEvent(EventCMD.HIDE_BTN);
      this.index = index;
      BuildingDefinition definition = BuildData.Instance.ListBuildingDefinition[index];
      txtName.text = definition.name;
      txtDes.text = definition.des;
      txtPrice.text = definition.price.ToString();
      ManagerEvent.RaiseEvent(EventCMD.SHOWADDBUILDING, index);
   } 
   
   internal override void Init()
   {
      // btnNext.onClick.AddListener(Next);
      // btnPre.onClick.AddListener(Prev);
   }

   void Next()
   {
      index += 1;
      if (index >=  BuildData.Instance.ListBuildingDefinition.Count)
      {
         index = 0;
      }
      ShowAddBuild(index);
   }

   void Prev()
   {
      index -= 1;
      if (index < 0)
      {
         index =  BuildData.Instance.ListBuildingDefinition.Count-1;
      }
      ShowAddBuild(index);
   }

   void Build()
   {
      DataInGame.Instance.BuildingGroup.AddDataBuilding(BuildData.Instance.ListBuildingDefinition[this.index]);
      ManagerEvent.RaiseEvent(EventCMD.BUILDBUILDING,   DataInGame.Instance.BuildingGroup.GetLastBuilding());
      ManagerEvent.RaiseEvent(EventCMD.SHOW_BTN);
      Back();
   }

   void Close()
   {
      ManagerEvent.RaiseEvent(EventCMD.SHOW_BTN);
      base.Back();
      ManagerEvent.RaiseEvent(EventCMD.HIDEADDBUILDING);
   }
}
