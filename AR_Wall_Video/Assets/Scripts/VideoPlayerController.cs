using UnityEngine;
using UnityEngine.Video;

namespace ARWallVideo
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoPlayerController : MonoBehaviour
    {
        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private MeshRenderer _quadRenderer;

        private VideoPlayer _videoPlayer;

        void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
            _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            _videoPlayer.targetTexture = _renderTexture;
            _videoPlayer.isLooping = true;
            _videoPlayer.prepareCompleted += OnPrepareCompleted;
        }

        // Called by AddressableVideoLoader with a downloaded VideoClip
        public void PlayClip(VideoClip clip)
        {
            _videoPlayer.Stop();
            _videoPlayer.source = VideoSource.VideoClip;
            _videoPlayer.clip = clip;
            _videoPlayer.Prepare();
        }

        private void OnPrepareCompleted(VideoPlayer vp)
        {
            // Correct quad scale to match video aspect ratio
            if (vp.width > 0 && vp.height > 0)
            {
                float aspect = (float)vp.width / vp.height;
                Vector3 s = _quadRenderer.transform.localScale;
                _quadRenderer.transform.localScale =
                    new Vector3(s.y * aspect, s.y, s.z);
            }
            vp.Play();
        }

        void OnDestroy()
        {
            _videoPlayer.prepareCompleted -= OnPrepareCompleted;

            // Clear RenderTexture to avoid ghost frame on next load
            if (_renderTexture != null)
            {
                RenderTexture prev = RenderTexture.active;
                RenderTexture.active = _renderTexture;
                GL.Clear(true, true, Color.clear);
                RenderTexture.active = prev;
            }
        }
    }
}