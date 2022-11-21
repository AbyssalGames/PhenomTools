using System;
using UnityEngine;
using PhenomTools;
using EasyMobile;
using GetSocialSdk.Core;
using UnityEngine.Video;

namespace BlackBoxVR.GetSocial
{
    [Serializable]
    public class MediaData
    {
        public MediaType mediaType;
        public string url;
        public bool isLocal;

        [SerializeField]
        private Texture2D texture;

        private MediaResult localMedia;
        private MediaAttachment remoteMedia;

        public MediaData(MediaType mediaType, string url)
        {
            this.mediaType = mediaType;
            this.url = url;
        }
        public MediaData(MediaResult mediaResult)
        {
            isLocal = true;
            localMedia = mediaResult;
            mediaType = mediaResult.Type;
            url = mediaResult.Uri;
        }
        public MediaData(MediaAttachment mediaAttachment)
        {
            remoteMedia = mediaAttachment;
            if (!string.IsNullOrEmpty(mediaAttachment.VideoUrl))
            {
                mediaType = MediaType.Video;
                url = mediaAttachment.VideoUrl;
            }
            else
            {
                mediaType = MediaType.Image;
                url = mediaAttachment.ImageUrl;
            }
        }

        public void GetTexture(Action<Texture2D> callback)
        {
            if(texture != null)
            {
                callback?.Invoke(texture);
            }
            else 
            { 
                if (mediaType == MediaType.Image)
                {
                    if (isLocal)
                    {
                        localMedia.LoadImage((error, tex) =>
                        {
                            texture = tex;
                            callback?.Invoke(texture);
                        });
                    }
                    else if(!string.IsNullOrEmpty(url))
                    {
                        PhenomUtils.GetTextureFromURL(url, tex =>
                        {
                            texture = (Texture2D)tex;
                            callback?.Invoke(texture);
                        }, error => Debug.LogError(error));
                    }
                    else
                    {
                        Debug.LogError("Cannot load remote image without URL");
                    }
                }
                else if (mediaType == MediaType.Video)
                {
                    GetThumbnail(url, callback);
                }
            }
        }

        public static void GetThumbnail(string url, Action<Texture2D> callback)
        {
            VideoPlayer tempVideoPlayer = new GameObject("TempVideoPlayer").AddComponent<VideoPlayer>();
            tempVideoPlayer.renderMode = VideoRenderMode.APIOnly;
            tempVideoPlayer.source = VideoSource.Url;
            tempVideoPlayer.url = url;

            GetThumbnail(tempVideoPlayer, tex =>
            {
                UnityEngine.Object.Destroy(tempVideoPlayer.gameObject);
                callback?.Invoke(tex);
            });
        }

        public static void GetThumbnail(VideoPlayer videoPlayer, Action<Texture2D> callback)
        {
            if (!videoPlayer.isPrepared)
            {
                videoPlayer.prepareCompleted += _ => callback?.Invoke(GetThumbnail(videoPlayer));
                videoPlayer.Prepare();
            }
            else
            {
                callback?.Invoke(GetThumbnail(videoPlayer));
            }
        }

        private static Texture2D GetThumbnail(VideoPlayer videoPlayer)
        {
            videoPlayer.time = 0;
            videoPlayer.Play();

            Texture2D videoPlayerTexture = videoPlayer.texture as Texture2D;
            Texture2D thumbnail = new Texture2D(videoPlayerTexture.width, videoPlayerTexture.height);
            thumbnail.SetPixels(videoPlayerTexture.GetPixels());
            thumbnail.Apply();
            
            videoPlayer.Pause();

            return thumbnail;
        }

        public override bool Equals(object obj)
        {
            MediaData other = obj as MediaData;
            return mediaType == other.mediaType && url == other.url;
        }

        public override int GetHashCode()
        {
            return mediaType.GetHashCode() ^ url.GetHashCode();
        }
    }
}
