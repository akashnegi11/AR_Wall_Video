using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;

namespace ARWallVideo
{
    public class AddressableVideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoPlayerController _videoController;
        [SerializeField] private VideoSwitcherUI _switcherUI;

        private AsyncOperationHandle<VideoClip> _currentHandle;
        private bool _isLoading;

        public void LoadVideo(string address)
        {
            if (_isLoading) return;
            StartCoroutine(LoadCoroutine(address));
        }

        private IEnumerator LoadCoroutine(string address)
        {
            _isLoading = true;

            // Release previous asset from memory
            if (_currentHandle.IsValid())
                Addressables.Release(_currentHandle);

            var handle = Addressables.LoadAssetAsync<VideoClip>(address);
            _currentHandle = handle;

            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _videoController.PlayClip(handle.Result);
            }
            else
            {
                Debug.LogError(
                    $"[AddressableVideoLoader] Failed to load '{address}': " +
                    handle.OperationException
                );
            }

            _switcherUI.OnLoadComplete();
            _isLoading = false;
        }

        void OnDestroy()
        {
            if (_currentHandle.IsValid())
                Addressables.Release(_currentHandle);
        }
    }
}