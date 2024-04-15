using System;
using System.Collections;
using System.Collections.Generic;
using Lam;
using Lam.zGame.Core_game.Core.Utilities.Components.UI;
using Lam.zGame.Core_game.Core.Utilities.Frameworks.UI;
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
   private int index = 0;
   protected override void Awake()
   {
      base.Awake();
      
   }

   private void Start()
   {
      btnNext.onClick.AddListener(Next);
      btnPre.onClick.AddListener(Prev);
   }

   public void ShowAddBuild(int index)
   {
      BuildingInfo info = BuildData.Instance.ListBuildingInfos[index];
      txtName.text = info.name;
      txtDes.text = info.des;
      txtPrice.text = info.price.ToString();
      Debug.LogWarning(index);
   } 
   
   internal override void Init()
   {
      // btnNext.onClick.AddListener(Next);
      // btnPre.onClick.AddListener(Prev);
   }

   void Next()
   {
      index += 1;
      if (index >=  BuildData.Instance.ListBuildingInfos.Count)
      {
         index = 0;
      }
      ShowAddBuild(index);
      Debug.Log("next");
   }

   void Prev()
   {
      index -= 1;
      if (index < 0)
      {
         index =  BuildData.Instance.ListBuildingInfos.Count-1;
      }
      ShowAddBuild(index);
      Debug.Log("prev");
   }
}
