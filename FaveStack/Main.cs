using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using FaveStack.Utils;
using System;
using System.Collections;
using System.IO;
using System.Net;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace FaveStack
{
    [BepInPlugin("com.github.wtfblaze.favestack", "FaveStack", "1.0.1")]
    public class Main : BasePlugin
    {
        public static string MainModFolder = Directory.GetParent(Application.dataPath) + "\\WTFBlaze";
        private Transform avatarScreen;
        private GameObject favoriteList;
        private MonoBehaviourPublicAbstractCacaLi1ObpiGaRepicoUnique uiVrcList;
        private Text textLabel;
        private GameObject favBtn;
        private MonoBehaviour1PublicObReObBoVeBoGaVeStBoUnique pageAvatar;

        public override void Load()
        {
            ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
            if (!File.Exists("Newtonsoft.Json.dll"))
            {
                SendLog("Newtonsoft.Json dependency not found! Downloading from the internet...", ConsoleColor.Yellow);
                try
                {
                    WebClient wc = new();
                    var bytes = wc.DownloadData("https://cdn.wtfblaze.com/mods/dependency/Newtonsoft.Json.dll");
                    File.Create("Newtonsoft.Json.dll").Close();
                    File.WriteAllBytes("Newtonsoft.Json.dll", bytes);
                    SendLog("Successfully downloaded Newtonsoft.Json! FaveStack will load next time you start the game!", ConsoleColor.Green);
                    return;
                }
                catch (Exception ex)
                {
                    SendLog("Error downloading Newtonsoft.Json! | Error Message: " + ex.Message, ConsoleColor.Red);
                }
            }
            if (!Directory.Exists(MainModFolder))
            {
                Directory.CreateDirectory(MainModFolder);
            }
            if (!Directory.Exists(MainModFolder + "\\FaveStack"))
            {
                Directory.CreateDirectory(MainModFolder + "\\FaveStack");
            }
            FSConfig.Load();
            WaitForSM().Start();
        }

        public static void SendLog(string msg, ConsoleColor col)
        {
            ConsoleManager.SetConsoleColor(col);
            ConsoleManager.StandardOutStream.WriteLine("[FaveStack] " + msg);
        }

        private void InitializeUI()
        {
            try
            {
                avatarScreen = GameObject.Find("UserInterface/MenuContent/Screens/Avatar").transform;
                pageAvatar = avatarScreen.gameObject.GetComponent<MonoBehaviour1PublicObReObBoVeBoGaVeStBoUnique>();
                favoriteList = UnityEngine.Object.Instantiate(
                    avatarScreen.Find("Vertical Scroll View/Viewport/Content/Public Avatar List").gameObject,
                    avatarScreen.Find("Vertical Scroll View/Viewport/Content"), false);
                favoriteList.GetComponent<MonoBehaviour1PublicObSuStSiBoSiSoObInSoUnique>().field_Public_EnumNPublicSealedvaInPuMiFaSpClPuLi11Unique_0 = MonoBehaviour1PublicObSuStSiBoSiSoObInSoUnique.EnumNPublicSealedvaInPuMiFaSpClPuLi11Unique.SpecificList;
                uiVrcList = favoriteList.GetComponent<MonoBehaviourPublicAbstractCacaLi1ObpiGaRepicoUnique>();
                textLabel = favoriteList.transform.Find("Button").GetComponentInChildren<Text>();
                favoriteList.transform.SetSiblingIndex(0);
                uiVrcList.clearUnseenListOnCollapse = false;
                uiVrcList.usePagination = false;
                uiVrcList.hideElementsWhenContracted = false;
                uiVrcList.hideWhenEmpty = false;
                uiVrcList.field_Protected_Dictionary_2_Int32_List_1_ApiModel_0.Clear();
                uiVrcList.pickerPrefab.transform.Find("TitleText").GetComponent<Text>().supportRichText = true;
                favoriteList.SetActive(true);
                favoriteList.name = "WTFBlaze's FaveStack List";
                textLabel.supportRichText = true;
                textLabel.text = "FaveStack - [0]";

                favBtn = UnityEngine.Object.Instantiate(
                    avatarScreen.Find("Change Button").gameObject,
                    favoriteList.transform.Find("Button"), false);
                favBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(185, 65);
                favBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(885, 0);
                favBtn.GetComponentInChildren<Text>().text = "Fav / Unfav";
                favBtn.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                favBtn.GetComponent<Button>().onClick.AddListener(new Action(() => 
                {
                    if (!FSConfig.Instance.list.Exists(avi => avi.ID == pageAvatar.field_Public_MonoBehaviourPublicSiGaRuGaAcRu3StAvApUnique_0.field_Internal_ApiAvatar_0.id))
                    {
                        FavoriteAvatar(pageAvatar.field_Public_MonoBehaviourPublicSiGaRuGaAcRu3StAvApUnique_0.field_Internal_ApiAvatar_0);
                    }
                    else
                    {
                        UnfavoriteAvatar(pageAvatar.field_Public_MonoBehaviourPublicSiGaRuGaAcRu3StAvApUnique_0.field_Internal_ApiAvatar_0);
                    }
                }));

                SendLog("Successfully created FaveStack List!", ConsoleColor.Green);
                var listener = avatarScreen.gameObject.AddComponent<EnableDisableListener>();
                listener.OnEnabled += () =>
                {
                    RefreshList();
                };
                RefreshList();
            }
            catch (Exception ex)
            {
                SendLog("Failed to create FaveStack list! | Error Message: " + ex.Message, ConsoleColor.Red);
            }
        }

        private IEnumerator WaitForSM()
        {
            while (MonoBehaviour1PublicVo0.prop_MonoBehaviourPublicObBoStTrStSiTrTeStSiUnique_0 == null) yield return null;
            InitializeUI();
            yield break;
        }

        private void RenderElement(Il2CppSystem.Collections.Generic.List<ApiAvatar> AvatarList)
        {
            uiVrcList.Method_Protected_Void_List_1_T_Int32_Boolean_MonoBehaviourPublicStTeRaImObTeStGaObBoUnique_0(AvatarList);
        }

        private void FavoriteAvatar(ApiAvatar apiAvatar)
        {
            if (!FSConfig.Instance.list.Exists(avi => avi.ID == apiAvatar.id))
            {
                FSConfig.Instance.list.Insert(0, apiAvatar.ToModFavAvi());
            }
            RefreshList();
            FSConfig.Instance.Save();
        }

        private void UnfavoriteAvatar(ApiAvatar apiAvatar)
        {
            var item = FSConfig.Instance.list.Find(avi => avi.ID == apiAvatar.id);
            FSConfig.Instance.list.Remove(item);
            RefreshList();
            FSConfig.Instance.Save();
        }

        private void RefreshList()
        {
            try
            {
                Il2CppSystem.Collections.Generic.List<ApiAvatar> AvatarList = new();
                FSConfig.Instance.list.ForEach(avi => AvatarList.Add(avi.ToApiAvatar()));
                RenderElement(AvatarList);
                textLabel.text = $"FaveStack - [{AvatarList.Count}]";
            }
            catch (Exception ex)
            {
                SendLog("Failed to Refresh FaveStack list! | Error Message: " + ex.Message, ConsoleColor.Red);
            }
        }
    }
}
