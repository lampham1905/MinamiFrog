using Lam.zGame.Core_game.Core_DataDefinition.Data;
using UnityEngine;

namespace Lam.zGame.Core_game.Core_DataDefinition.Manager
{
    public class SceneHome : MonoBehaviour
    {
        //[SerializeField] private SplashScreen splashScreen;

        private void Start()
        {
            Init();
            //Vibration.Initialize();
            //GameInit.Instance.Init((isInit) => {
            //    if(isInit)
            //    {
            //        if(GameData.USE_CLOUD_SAVE && !GPGS.Instance.IsInitialized)
            //        {
            //            CloudSave.Instance.Init();
            //            splashScreen.Enable(true).Run(4.0f);
            //            GPGS.Instance.Init(SyncData);
            //        }
            //        else Init();
            //    }
            //    else Init();
            //});
        }

        private void Init()
        {
            GameConfigGroup gameConfigGroup = Data.GameData.Instance.GameConfigGroup;
            //if(gameConfigGroup.IsFirstBattle)
            //{
            if (gameConfigGroup.CampaignSession == -1)
            {
                gameConfigGroup.CampaignSession = 2;
                Data.GameData.Instance.TutorialGroup.SetPassAllTutorial();
            }
            //WorldMapViewNormal.Instance.Init(IDs.TYPE_LIST_MAP_NORMAL);
            //WorldMapViewHard.Instance.Init(IDs.TYPE_LIST_MAP_HARD);
            //MainMenuPanel.Instance.Init();
            //}
            //else
            //{
            //    gameConfigGroup.CampaignSession += 1;
            //    MainMenuPanel.Instance.gameObject.SetActive(false);
            //    StartCoroutine(StartFirstBattle());
            //}
            //splashScreen.IsDone = true;
        }

        //private void SyncData(bool status)
        //{
        //    if(status)
        //    {
        //        string localUserId = GPGS.Instance.GetUserId();
        //        CloudSave.Instance.LoadData(localUserId, (jsonData) => {
        //            GameConfigGroup gameConfigGroup = GameData.Instance.GameConfigGroup;
        //            if(string.IsNullOrWhiteSpace(jsonData))
        //            {
        //                switch(gameConfigGroup.UserStates)
        //                {
        //                    case UserStates.OldUser:
        //                        if(string.IsNullOrWhiteSpace(gameConfigGroup.UserId))
        //                        {
        //                            gameConfigGroup.UserId = localUserId;
        //                            GameData.Instance.Save();
        //                            PushData("First time to push old user's data to server.");
        //                        }
        //                        else if(!String.Equals(gameConfigGroup.UserId, localUserId, StringComparison.Ordinal))
        //                        {
        //                            PushData("Backup data for other account. Create new account.");
        //                            GameData.Instance.ResetToDefaultData();
        //                            gameConfigGroup.UserId = localUserId;
        //                            GameData.Instance.Save();
        //                        }
        //                        break;
        //                    case UserStates.NewUser:
        //                        gameConfigGroup.UserId = localUserId;
        //                        break;
        //                }
        //                Init();
        //            }
        //            else
        //            {
        //                DataSaverContainer.SetData("cloud", jsonData);
        //                DataSaver cloudSaver = DataSaverContainer.CreateSaver("cloud", GameData.ENCRYPT_SAVER ? GameData.FILE_ENRYPTION : null, false);
        //                if(String.Equals(gameConfigGroup.UserId, localUserId, StringComparison.Ordinal))
        //                {
        //                    string playTimeData = cloudSaver.Get("8.15", out int result_2);
        //                    float serverPlayTime = float.TryParse(playTimeData, out float _playTime) ? _playTime : 0;
        //                    if(serverPlayTime > gameConfigGroup.LocalPlayTime)
        //                    {
        //                        OverrideData(jsonData, cloudSaver);
        //                        print("Replace data from server to local.");
        //                    }
        //                    else PushData("Push local data to server.");
        //                }
        //                else
        //                {
        //                    PushData("Backup data for other account. Switch account.");
        //                    OverrideData(jsonData, cloudSaver);
        //                }
        //                DataSaverContainer.RemoveSaverKey("cloud");
        //                Init();
        //            }
        //        });
        //    }
        //    else Init();
        //}

        //private void PushData(string log = default)
        //{
        //    GameData.Instance.PushLocalDataToServer();
        //    if(string.IsNullOrWhiteSpace(log)) return;
        //    Debug.Log($"<color=yellow>[CloudSave]:</color>{log}");
        //}

        //private void OverrideData(string jsonData, DataSaver dataSaver)
        //{
        //    DataSaverContainer.SetData("main", jsonData);
        //    DataSaverContainer.ReplaceSaver("main", dataSaver);
        //    GameData.Instance.Reload(true);
        //    GameData.Instance.BaseInit();
        //}

        //private IEnumerator StartFirstBattle()
        //{
        //    Config.worldIdSelect = Constants.WORLD_ID_TUT;
        //    if(splashScreen.IsRun)
        //    {
        //        while(splashScreen.IsRun)
        //            yield return null;
        //    }
        //    else yield return new WaitForEndOfFrame();
        //    SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
        //}
    }
}
