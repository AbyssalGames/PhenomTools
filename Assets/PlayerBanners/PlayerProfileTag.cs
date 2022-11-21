using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// using BlackBoxVR.MobileApp;
// using BlackBoxVR.MobileApp.ServerData;
// using BlackBoxVR.MobileApp.DataManagers.SocialDataManager;
//using DG.Tweening;
using System;
using GetSocialSdk.Core;

public class PlayerProfileTag : MonoBehaviour
{
    public TextMeshProUGUI usernameLabel;
    public TextMeshProUGUI xpTitleLabel;
    //public TextMeshProUGUI userLevelLabel;
    [SerializeField]
    private ProfileImage profileImage = null;
    //public Image bannerImagePlaceholder;
    public Image bannerImage;
    //public RawImage userProfileImage;
    //public Image initialsThumbnail;
    //public TextMeshProUGUI initialsTextLabel;
    //public Image levelPillImage;
    public CanvasGroup canvasGroup;
    // public ArenaLeagueColorRepository arenaLeagueColorRepository;

    private string thisUserId;

    public void Awake()
    {
        //HideElementsBeforeLoad();
    }

    private void GetUserProfile(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            Debug.LogError("Get User Profile userId is null");
            return;
        }

        // APIManager.Instance.SocialGetFriendUserProfile(userId,
        //         delegate (ModelDataResponse<UserProfile> response)
        //         {
        //             if (response.Data != null)
        //             {
        //                 if (!response.hasError)
        //                 {
        //                     onComplete?.Invoke(response.Data);
        //                     //Init(userId, response.Data);
        //                 }
        //                 else
        //                 {
        //                     Debug.LogError("couldn't init profile tag for " + userId + ", " + response.Error);
        //                     onComplete?.Invoke(null);
        //                 }
        //             }
        //             else
        //             {
        //                 Debug.LogError("couldn't init profile tag for " + userId + ", " + response.Error);
        //                 onComplete?.Invoke(null);
        //             }
        //         });
    }

    public void Init(string userId)
    {
        //HideElementsBeforeLoad();
        //canvasGroup.alpha = 0f;
        // GetUserProfile(userId,
        //     (userProfile) =>
        //     {
        //         Setup(userId, userProfile);
        //     });
    }

    public void Init(string userId, string username)
    {
        SetUsername(username);

        Init(userId);
    }

    // public void Init(string userId)
    // {
    //     //HideElementsBeforeLoad();
    //
    //     Setup(userId);
    // }

    //public void InitGetSocial(User user)
    //{
    //    SetUsername(user.DisplayName);
    //    usernameLabel.colorGradientPreset = arenaLeagueColorRepository.GetTMPColorGradientByCardTier(CardTier.Diamond);// (CardTier)int.Parse(user.PublicProperties["League"]));
    //    profileImage.InitGetSocial(user);
    //}

    public void SetUsername(string username)
    {
        usernameLabel.text = username.ToUpper();
        //usernameLabel.DOFade(1.0f, 1.0f);
    }

    private void Setup(string userId)
    {
        // TODO: Revise this
        // if (userProfile == null)
        //     return;

        // thisUserId = userId;
        // //Debug.LogWarning("USER ID OF NULL USER: " + userId);
        //
        // SetUsername(userProfile.username);
        // usernameLabel.colorGradientPreset = arenaLeagueColorRepository.GetTMPColorGradientByCardTier((CardTier)userProfile.leagueDetails.currentLeague);
        // //usernameLabel.DOFade(1.0f, 1.0f);
        //
        // profileImage.Initialize(userId, userProfile);
        // //SetupLevelPill(userProfile);
        // //SetupProfilePic(userId, userProfile.username);
        //
        // APIManager.Instance.connectorStore.GetPlayerItems(null, userId, GSStoreConnector.PlayerItemCategory.BannerAndTitle, (response) =>
        // {
        //     if (response != null)
        //     {
        //         if (response.hasError)
        //         {
        //             SetupXPTitle("");
        //             SetupBanner("");
        //             Debug.LogError("Inventory: Response Has Errors: " + response.Error);
        //         }
        //         else
        //         {
        //             if (response.Data != null)
        //             {
        //                 AvatarCustomizationModel data = response.Data;
        //
        //                 string title = "";
        //                 if (data.items.bannerCards.selection.profiletitles != null &&
        //                     data.items.bannerCards.selection.profiletitles.Count > 0)
        //                 {
        //                     title = data.items.bannerCards.selection.profiletitles[0].title;
        //                 }
        //
        //                 string selectedBannerShortCode = "";
        //                 if (data.items.bannerCards.selection.profilebackgrounds != null &&
        //                 data.items.bannerCards.selection.profilebackgrounds.Count > 0)
        //                 {
        //                     selectedBannerShortCode = data.items.bannerCards.selection.profilebackgrounds[0].shortCode;
        //                 }
        //
        //                 SetupXPTitle(title);
        //                 SetupBanner(selectedBannerShortCode);
        //             }
        //         }
        //     }
        //     else
        //     {
        //         SetupXPTitle("");
        //         SetupBanner("");
        //     }
        // });
        //
        // if (canvasGroup != null)
        //     canvasGroup.alpha = 1f;
    }

    //private void HideElementsBeforeLoad()
    //{
    //    usernameLabel.text = "";
    //    xpTitleLabel.text = "";
    //    initialsTextLabel.text = "";

    //    bannerImage.DOFade(0.0f, 0.001f);
    //    bannerImagePlaceholder.DOFade(1.0f, 0.001f);
    //    usernameLabel.DOFade(0.0f, 0.001f);
    //    xpTitleLabel.DOFade(0.0f, 0.001f);
    //    initialsTextLabel.DOFade(0.0f, 0.001f);
    //    userLevelLabel.DOFade(0.0f, 0.001f);
    //    levelPillImage.DOFade(0.0f, 0.001f);
    //}

    private void SetupBanner(string shortCode)
    {
        // bannerImage.sprite = PlayerBannerDataRepositoryService.Instance.GetPlayerBannerSpriteByShortCode(shortCode);
        //bannerImage.DOFade(1.0f, 1.0f);
        //bannerImagePlaceholder.DOFade(0.0f, 1.0f);
    }

    private void SetupXPTitle(string title)
    {
        if (title == "")
        {
            xpTitleLabel.text = "";
        }
        else
        {
            xpTitleLabel.text = @"XP TITLE: <color=""white""> " + title.ToUpper();
            //xpTitleLabel.DOFade(1.0f, 1.0f);
        }
        //xpTitleLabel.text = xpTitleLabel.text.Replace("{PROFILETITLE}", title.ToUpper());
    }

    //private void SetupLevelPill(UserProfile userProfile)
    //{
    //    userLevelLabel.text = "LVL " + ((int)userProfile.UserHeroLevelValue()).ToString();
    //    levelPillImage.sprite = arenaLeagueColorRepository.GetPillSpriteByCardTier((CardTier)userProfile.leagueDetails.currentLeague);

    //    //userLevelLabel.DOFade(1.0f, 1.0f);
    //    //levelPillImage.DOFade(1.0f, 1.0f);
    //}

    //private void SetupProfilePic(string userId, string userName)
    //{
    //    userProfileImage.gameObject.SetActive(false);
    //    initialsThumbnail.gameObject.SetActive(true);
    //    initialsTextLabel.text = SocialFriendInfo.InitialsForUserName(userName);
    //    //initialsTextLabel.DOFade(1.0f, 1.0f);

    //    ServerDataManager.SharedInstance.GetUserProfileThumbnailImage(userId,
    //        (texture) =>
    //    {
    //        if(texture != null)
    //        {
    //            userProfileImage.gameObject.SetActive(true);
    //            initialsThumbnail.gameObject.SetActive(false);
    //            userProfileImage.texture = texture;
    //            //userProfileImage.DOFade(1.0f, 1.0f);
    //        }
    //    });

    //}

    public void ShowProfileForUser()
    {
        // CommentViewController.Instance.ShowProfile(thisUserId, true);
    }
}
