using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public enum ActivityType
    {
        CustomText,
        CustomMedia,
        //CustomImage,
        //CustomVideo,
        //CustomGallery,
        PostBattle,
        NewProfilePic,
        //HeroLevelUp,
        //Streak,
        //BattleLog,
        //AdvanceLeague,
        //ArenaLevelUp,
        //BossDefeated,
        //DurationQuest,
        //Achievement,
    }

    public enum FeedType
    {
        Global,
        Timeline,
        Topic,
        User
    }

    [Serializable]
    public class ReactionData
    {
        public string key;
        public GameObject iconWithBgPrefab;
        public GameObject iconOffPrefab;
        public GameObject iconOnPrefab;
        public Color color;
    }

    [Serializable]
    public class FeelingData
    {
        public string key;
        public string emoji;
    }

    public class PagingActivitiesPackage
    {
        public List<Activity> activities;
        public PagingQuery<ActivitiesQuery> pagingQuery;
        public string nextCursor;

        public PagingActivitiesPackage(List<Activity> activities, PagingQuery<ActivitiesQuery> pagingQuery, string nextCursor)
        {
            this.activities = activities;
            this.pagingQuery = pagingQuery;
            this.nextCursor = nextCursor;
        }
    }
}