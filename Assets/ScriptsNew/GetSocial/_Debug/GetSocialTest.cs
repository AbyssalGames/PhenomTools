using System;
using GetSocialSdk.Core;
using PhenomTools;
using UnityEngine;

namespace BlackBoxVR.GetSocial
{
    public class GetSocialTest : MonoBehaviour
    {
        //public string debugUsername = "seans_commenttest";
        //public string debugPassword = "test123";

        public int dummiesToCreate = 100;
        //public string[] achievementNames;

        //private string _topicID = "0000";
        //public Dictionary<string, string> test;

        //[Serializable]
        //public class FeedItemContentDict : SerializableDictionaryBase<string, string> { }
        //public FeedItemContentDict serTest;

        //private void Start()
        //{
        //    GetSocial.AddOnInitializedListener(OpenGlobalFeed);
        //    Switchboard.Instance.MainSceneLoaded = true;
        //    Switchboard.Instance.AllObjectLoadedForMainScene?.Invoke();

        //    PhenomUtils.DelayActionByTime(1f, () => APIManager.Instance.AttemptLogin(debugUsername, debugPassword, response =>
        //    {
        //        if (response.Data == null || response.hasError)
        //        {
        //            Debug.LogError(response.Error);
        //            return;
        //        }

        //        Debug.Log(response.Data.userId);

        //        //ServerDataManager._sharedInstance.gameSparksManager.authenticatedUserId = response.Data.userId.ToString();
        //        GetSocialManager.Init(debugUsername, debugPassword, () => { }, _ => { });
        //    }));
        //}

        [ContextMenu("OpenGlobalFeed")]
        public void OpenGlobalFeed()
        {
            ActivityController.OpenCommunitiesView();
        }

        //public void LoadPosts()
        //{
        //    ActivitiesQuery query = ActivitiesQuery.ActivitiesInTopic(_topicID);

        //    var pagingQuery = new PagingQuery<ActivitiesQuery>(query);

        //    Communities.GetActivities(pagingQuery,
        //        (result) =>
        //        {
        //            Debug.Log("Activities: " + result.Entries.Count);
        //        },
        //        (error) =>
        //        {
        //            Debug.Log("Failed to get activities: " + error);
        //        });
        //}

        //public void PostDummyAchievement()
        //{
        //    string[] achievementNames = AchievementsDataManager.SharedInstance.achievementAssetsByShortCode.Keys.ToArray();
        //    int rng = UnityEngine.Random.Range(1, achievementNames.Length);

        //    //Debug.Log(achievementNames[rng]);
        //    //while (AchievementsDataManager.SharedInstance.achievementAssetsByShortCode[achievementNames[rng]].assetReference == null)
        //    //{
        //    //    Debug.LogError(achievementNames[rng] + " was null");
        //    //    rng = UnityEngine.Random.Range(1, achievementNames.Length);
        //    //}
        //    //return;

        //    ActivityContent activityContent = new ActivityContent
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.Achievement.ToString() },
        //            { "Achievement", achievementNames[rng] },
        //            { "Trigger", UnityEngine.Random.Range(0, 1000000000).ToString() }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        //public void PostDummyAdvanceLeague()
        //{
        //    ActivityContent activityContent = new ActivityContent
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.AdvanceLeague.ToString() },
        //            { "League", UnityEngine.Random.Range(1, 5).ToString() }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        //public void PostDummyArenaLevelUp()
        //{
        //    ActivityContent activityContent = new ActivityContent
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.ArenaLevelUp.ToString() },
        //            { "Trophies", UnityEngine.Random.Range(100, 100000).ToString() },
        //            { "Arena", UnityEngine.Random.Range(1, 11).ToString() }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        //public void PostDummyBattleLog()
        //{
        //    ActivityContent activityContent = new ActivityContent 
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.BattleLog.ToString() },
        //            { "Log", JsonUtility.ToJson(new SocialFeedBattleLogInfo()) }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        //public void PostDummyBossDefeated()
        //{
        //    ActivityContent activityContent = new ActivityContent
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.BossDefeated.ToString() },
        //            { "League", UnityEngine.Random.Range(1, 5).ToString() },
        //            { "Arena", UnityEngine.Random.Range(1, 11).ToString() }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        //private string[] questNames =
        //{
        //    "CompleteAllMonthlyQuests",
        //    "CompleteAllWeeklyQuest",
        //    "Burn4000CaloriesInBattle",
        //    "Complete12WorkoutBattles",
        //    "Complete3WorkoutBattles",
        //    "CompleteFirstChallengeOrna"
        //};
        //public void PostDummyDurationQuest()
        //{
        //    ActivityContent activityContent = new ActivityContent
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.DurationQuest.ToString() },
        //            { "Quest", questNames[UnityEngine.Random.Range(1, questNames.Length)] },
        //            { "Sprite", DurationQuestsDataManager.SharedInstance.durationQuestAssetsByShortCode.Keys.ToArray()[UnityEngine.Random.Range(0, 6)] }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        //public void PostDummyHeroLevelUp()
        //{
        //    ActivityContent activityContent = new ActivityContent
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.HeroLevelUp.ToString() },
        //            { "Level", UnityEngine.Random.Range(1, 100).ToString() }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        public void PostDummyNewProfilePic()
        {
            ActivityContent activityContent = new ActivityContent
            {
                Properties =
                {
                    { "Type", ActivityType.NewProfilePic.ToString() },
                }
            };

            Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        }

        //public void PostDummyStreak()
        //{
        //    ActivityContent activityContent = new ActivityContent
        //    {
        //        Properties =
        //        {
        //            { "Type", ActivityType.Streak.ToString() },
        //            { "Index", UnityEngine.Random.Range(1, 100).ToString() }
        //        }
        //    };

        //    Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        //}

        public void PostDummyCustom()
        {
            ActivityContent activityContent = new ActivityContent
            {
                Text = "Hello, this is a test SERIALIZED post! Date (UTC): " + DateTime.UtcNow,
                Properties =
                {
                    { "Type", ActivityType.CustomText.ToString() }
                }
            };

            Communities.PostActivity(activityContent, PostActivityTarget.Timeline(), _ => { }, _ => { });
        }

        public void GenerateRandomDummies()
        {
            if (!GetSocialSdk.Core.GetSocial.IsInitialized)
                return;

            PhenomUtils.RepeatActionByTime(.1f, dummiesToCreate, () =>
                {
                    int random = UnityEngine.Random.Range(0, 1);

                    if (random == 0)
                        PostDummyCustom();
                    else if (random == 1)
                    //    PostDummyStreak();
                    //else if (random == 2)
                    //    PostDummyHeroLevelUp();
                    //else if (random == 3)
                    //    PostDummyBattleLog();
                    //else if (random == 4)
                    //    PostDummyAdvanceLeague();
                    //else if (random == 5)
                    //    PostDummyArenaLevelUp();
                    //else if (random == 6)
                    //    PostDummyBossDefeated();
                    //else if (random == 7)
                    //    PostDummyDurationQuest();
                    //else if (random == 8)
                    //    PostDummyAchievement();
                    //else if (random == 9)
                        PostDummyNewProfilePic();
                }, 
                () => Debug.Log(dummiesToCreate + " Dummies Created"));
        }

        //[ContextMenu("Post")]
        //public void Post()
        //{
        //    // Create test achievement item
        //    ActivityData testSocialFeedItem = new ActivityData
        //    {
        //        type = ActivityType.Custom,// SocialFeedItem.kFeedCategoryAchievements,
        //        //subType = "1000",
        //        //shortCode = "AA04Created",
        //        //userId = "userId",// GetSocial.GetCurrentUser().Id;
        //        //date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),

        //        battleLogInfo = new SocialFeedBattleLogInfo()
        //    };

        //    //Create GetSocial post
        //    var activityContent = new ActivityContent();
        //    activityContent.Text = "Hello, this is a test SERIALIZED post! Date (UTC): " + DateTime.UtcNow;
        //    activityContent.Properties.Add("Data", JsonUtility.ToJson(testSocialFeedItem));
        //    //test = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonUtility.ToJson(testSocialFeedItem));
        //    //serTest = new FeedItemContentDict();
        //    //serTest.CopyFrom(test);

        //    //Select a target topic
        //    var target = PostActivityTarget.Topic(_topicID);

        //    // Post to the topic
        //    Communities.PostActivity(activityContent, target,
        //        (activity) =>
        //        {
        //            Debug.Log("Posted SERIALIZED activity: " + activity);
        //        },
        //        (error) =>
        //        {
        //            Debug.Log("Failed to post SERIALIZED activity, error: " + error);
        //        });
        //}
    }
}
