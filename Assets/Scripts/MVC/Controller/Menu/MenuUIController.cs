using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace MVC.Controller.Menu
{
    public class MenuUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;
        
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject optionsPanel;
        
        [Header("Options UI")]
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Button optionsBackButton;
        
        [Header("Scene Settings")]
        [SerializeField] private string levelSceneName = "Level";

        public void Init()
        {
            Debug.Log("MenuUIController initialized");
            ShowMainMenu();
            SetupButtonListeners();
        }

        private void SetupButtonListeners()
        {
            if (startButton != null)
                startButton.onClick.AddListener(OnStartButtonClicked);
            
            if (optionsButton != null)
                optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitButtonClicked);
            
            if (optionsBackButton != null)
                optionsBackButton.onClick.AddListener(OnOptionsBackClicked);
            
            if (volumeSlider != null)
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        private void RemoveButtonListeners()
        {
            if (startButton != null)
                startButton.onClick.RemoveListener(OnStartButtonClicked);
            
            if (optionsButton != null)
                optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            
            if (quitButton != null)
                quitButton.onClick.RemoveListener(OnQuitButtonClicked);
            
            if (optionsBackButton != null)
                optionsBackButton.onClick.RemoveListener(OnOptionsBackClicked);
            
            if (volumeSlider != null)
                volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        }

        private void OnEnable()
        {
            // 이미 Init에서 설정되지만, 씬 재활성화 시를 대비
        }

        private void OnDisable()
        {
            RemoveButtonListeners();
        }

        #region Button Handlers
        
        private void OnStartButtonClicked()
        {
            Debug.Log("Start button clicked - Loading level scene");
            LoadLevelScene();
        }

        private void OnOptionsButtonClicked()
        {
            Debug.Log("Options button clicked");
            ShowOptionsPanel();
        }

        private void OnQuitButtonClicked()
        {
            Debug.Log("Quit button clicked");
            QuitGame();
        }

        private void OnOptionsBackClicked()
        {
            Debug.Log("Back to main menu");
            ShowMainMenu();
        }

        private void OnVolumeChanged(float value)
        {
            // AudioListener.volume = value;
            Debug.Log($"Volume changed to: {value}");
        }

        #endregion

        #region Panel Management
        
        private void ShowMainMenu()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
            
            if (optionsPanel != null)
                optionsPanel.SetActive(false);
        }

        private void ShowOptionsPanel()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);
            
            if (optionsPanel != null)
                optionsPanel.SetActive(true);
        }

        #endregion

        #region Scene Management
        
        private void LoadLevelScene()
        {
            if (!string.IsNullOrEmpty(levelSceneName))
            {
                SceneManager.LoadScene(levelSceneName);
            }
            else
            {
                Debug.LogError("Level scene name is not set!");
            }
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}