using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using FaveStack.Utils;
using System;
using System.Collections;
using System.IO;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace FaveStack
{
    [BepInPlugin("me.wtfblaze.favestack", "FaveStack", "1.0.0")]
    public class Main : BasePlugin
    {
        public static ManualLogSource logger;
        public static string MainModFolder = Directory.GetParent(Application.dataPath) + "\\WTFBlaze";
        private Transform avatarScreen;
        private GameObject favoriteList;
        private MonoBehaviourPublicAbstractCacaLi1ObpiGaRepicoUnique uiVrcList;
        private Text textLabel;
        private GameObject favBtn;
        private MonoBehaviour1PublicObReObBoVeBoGaVeStBoUnique pageAvatar;

        public override void Load()
        {
            logger = Log;
            ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
            if (!Directory.Exists(MainModFolder))
            {
                Directory.CreateDirectory(MainModFolder);
            }
            if (!Directory.Exists(MainModFolder + "\\FavStack"))
            {
                Directory.CreateDirectory(MainModFolder + "\\FavStack");
            }
            FSConfig.Load();
            WaitForSM().Start();
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
                favoriteList.name = "WTFBlaze's FavStack List";
                textLabel.supportRichText = true;
                textLabel.text = "FavStack - [0]";

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

                logger.LogInfo("Successfully created FavStack List!");
                var listener = avatarScreen.gameObject.AddComponent<EnableDisableListener>();
                listener.OnEnabled += () =>
                {
                    RefreshList();
                };
                RefreshList();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to create FavStack list! | Error Message: " + ex.Message);
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
                textLabel.text = $"FavStack - [{AvatarList.Count}]";
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to Refresh FavStack list! | Error Message: " + ex.Message);
            }
        }
    }
}
