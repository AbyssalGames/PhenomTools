// using BlackBoxVR.MobileApp;
// using BlackBoxVR.MobileApp.ServerData;
using GetSocialSdk.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using BlackBoxVR.GetSocial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhenomTools;

public class ProfileImage : MonoBehaviour
{
    public event Action onClick;
    public event Action<ProfileImage> onClickContext;

    [SerializeField]
    private RawImage thumbnail = null;
    [SerializeField]
    private GameObject initialsObject = null;
    [SerializeField]
    private TextMeshProUGUI initialsText = null;
    [SerializeField]
    private TextMeshProUGUI levelText = null;
    [SerializeField]
    private RectTransform pillRect = null;
    [SerializeField]
    private Image pillImage = null;
    // [SerializeField]
    // private ArenaLeagueColorRepository arenaLeagueColorRepository;

    private string userId;
    // private UserProfile userProfile;

    public void InitGetSocial(User user)
    {
        GetSocialManager.GetUserAvatar(user, SetTexture, _ => SetInitials(user));
    }

    public void SetTexture(Texture texture)
    {
        thumbnail.texture = texture;
        thumbnail.gameObject.SetActive(true);
        initialsObject.SetActive(false);
    }

    public void SetInitials(User user)
    {
        // initialsText.SetText(SocialFriendInfo.InitialsForUserName(user.DisplayName));
        thumbnail.gameObject.SetActive(false);
        initialsObject.SetActive(true);
    }

    // public void Initialize(string userId)
    // {
    //     this.userId = userId;
    //     // APIManager.Instance.SocialGetFriendUserProfile(userId, profile => Initialize(userId, profile.Data));
    // }

    public void Initialize(Texture2D texture)
    {
        Debug.LogError("Init with: " + texture.name);
        initialsObject.SetActive(false);
        thumbnail.texture = texture;
    }

    public void Initialize(string userId)
    {
        // if (userProfile == null)
        //     return;

        this.userId = userId;
        // this.userProfile = userProfile;
        this.userId = userId;

        if (thumbnail == null)
            return;

        thumbnail.gameObject.SetActive(false);
        initialsObject.SetActive(true);
        // initialsText.SetText(SocialFriendInfo.InitialsForUserName(userProfile.username));

        // ServerDataManager.SharedInstance.GetUserProfileThumbnailImage(userId,
        //     (texture) =>
        //     {
        //         if (texture != null)
        //         {
        //             thumbnail.gameObject.SetActive(true);
        //             initialsObject.SetActive(false);
        //             thumbnail.texture = texture;
        //         }
        //     });
        //
        // if (pillRect.gameObject.activeSelf)
        // {
        //     levelText.SetText("LVL " + ((int)userProfile.UserHeroLevelValue()).ToString());
        //     pillImage.sprite = arenaLeagueColorRepository.GetPillSpriteByCardTier((CardTier)userProfile.leagueDetails.currentLeague);
        //     //pillImage.pixelsPerUnitMultiplier = pillImage.sprite.texture.height / pillRect.rect.height;
        // }
    }

    public void OnClick()
    {
        onClick?.Invoke();
        onClickContext?.Invoke(this);

        //ContextManager.Instance.TemporaryProfileContextBeforeRefactor = true;
        if (!string.IsNullOrEmpty(userId))
        {
            // if (userProfile != null)
            //     CommentViewController.Instance.ShowProfile(userId, userProfile, true);
            // else
            //     CommentViewController.Instance.ShowProfile(userId, true);
        }
    }

    //[ContextMenu("test")]
    //public void Test()
    //{
    //    Debug.Log(pillImage.sprite.texture.height + " / " + pillRect.rect.height);
    //    pillImage.pixelsPerUnitMultiplier = pillImage.sprite.texture.height / (pillRect.rect.height * ;
    //}
}
