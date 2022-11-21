#if UNITY_ANDROID
#endif

namespace BlackBoxVR.GetSocial
{
    public class CreatePostToolbar : CreateActivityToolbar
    {
        private CreatePostView createPostView => createActivityView as CreatePostView;

        public void ShowFeelingSelectView()
        {
            createPostView.ShowFeelingSelectView();
        }
    }
}
