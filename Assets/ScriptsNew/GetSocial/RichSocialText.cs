using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using GetSocialSdk.Core;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace BlackBoxVR.GetSocial
{
    public class RichSocialText : MonoBehaviour, IPointerClickHandler
    {
        public const string linkHex = "#007FFF";
        public static Color linkColor { get { ColorUtility.TryParseHtmlString(linkHex, out Color color); return color; } }

        [SerializeField]
        private TextMeshProUGUI text = null;
        [SerializeField]
        private UnityEvent onClickTag = null;
        [SerializeField]
        private UnityEvent onClickMention = null;

        public virtual void Initialize(ActivityItem activityItem)
        {
            if (activityItem is CommentItem)
            {
                text.margin = Vector4.zero;
                text.fontSize = 12;
            }

            Initialize(activityItem.activity.Text);
            LayoutRebuilder.MarkLayoutForRebuild(activityItem.transform as RectTransform);
        }

        protected virtual void Initialize(string raw)
        {
            string richText = raw;
                
            if (richText.Any(c => c is '#' or '@'))
            {
                string[] split = Regex.Split(richText, @"(?=[#@])");
                for (int index = 1; index < split.Length; index++)
                {
                    string link = split[index].Split(' ')[0];
                    string linkType = link[0] == '#' ? "tag" : "user";
                    if (!string.IsNullOrWhiteSpace(link))
                        richText = richText.Replace(link, "<link=" + linkType + "><color=" + linkHex + ">" + link + "</link></color>");
                }
            }

            text.SetText(richText);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, Camera.main);
            
            if (linkIndex == -1)
                return;

            TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
            string link = linkInfo.GetLinkText().Substring(1);
            string linkId = linkInfo.GetLinkID();

            switch (linkId)
            {
                default:
                    Debug.LogError("Unknown Link Type: " + linkId);
                    break;
                case "tag":
                    onClickTag?.Invoke();
                    Debug.Log("Link Clicked, Type: " + linkId);
                    //TODO hook this up to load a feed with items containing this tag
                    // Communities.GetActivities(new PagingQuery<ActivitiesQuery>(ActivitiesQuery.Everywhere().WithTag(link)),
                    //     activityList =>
                    //     {
                    //         // open feed that's filtered by the selected tag
                    //     },
                    //     e => Debug.LogError(e.Message)
                    // );
                    break;
                case "user":
                    onClickMention?.Invoke();
                    Debug.Log("Link Clicked, Type: " + linkId);
                    //TODO instead go to this user's profile, and use this block to load their feed from that script
                    // Communities.GetUsers(new PagingQuery<UsersQuery>(UsersQuery.Find(link)),
                    //     userList => Communities.GetActivities(new PagingQuery<ActivitiesQuery>(ActivitiesQuery.Everywhere().ByUser(UserId.Create(userList.Entries[0].Id))),
                    //         activityList =>
                    //         {
                    //             Debug.Log("View: " + link + "'s Feed");
                    //         },
                    //         e => Debug.LogError(e.Message)
                    //     ),
                    //     e => Debug.LogError(e.Message)
                    // );
                    break;
            }
        }

        private void Reset()
        {
            text = GetComponent<TextMeshProUGUI>();
        }
    }
}
