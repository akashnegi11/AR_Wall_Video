using UnityEngine;
using UnityEngine.UI;

namespace ARWallVideo
{
    public class VideoSwitcherUI : MonoBehaviour
    {
        [SerializeField] private AddressableVideoLoader _loader;
        [SerializeField] private Button _video1Button;
        [SerializeField] private Button _video2Button;
        [SerializeField] private GameObject _spinner;

        private const string Video1Address = "video_1";
        private const string Video2Address = "video_2";

        void Start()
        {
            _video1Button.onClick.AddListener(
                () => OnVideoButtonPressed(Video1Address)
            );
            _video2Button.onClick.AddListener(
                () => OnVideoButtonPressed(Video2Address)
            );
            _spinner.SetActive(false);
        }

        private void OnVideoButtonPressed(string address)
        {
            SetButtonsInteractable(false);
            _spinner.SetActive(true);
            _loader.LoadVideo(address);
        }

        // Called by AddressableVideoLoader when loading finishes
        public void OnLoadComplete()
        {
            SetButtonsInteractable(true);
            _spinner.SetActive(false);
        }

        private void SetButtonsInteractable(bool state)
        {
            _video1Button.interactable = state;
            _video2Button.interactable = state;
        }

        void Update()
        {
            if (_spinner.activeSelf)
                _spinner.transform.Rotate(0f, 0f, -180f * Time.deltaTime);
        }
    }
}