using MVC.Controller.Menu;
using UnityEngine;

namespace Manager
{
    namespace Manager
    {
        public class MenuContext : MonoBehaviour
        {
            [SerializeField] private MenuUIController menuUIController;

            private void Awake()
            {
                if (menuUIController == null)
                    menuUIController = FindAnyObjectByType<MenuUIController>();
            }

            private void Start()
            {
                Debug.Log("=== MenuContext Start BEGIN ===");

                try
                {
                    Debug.Log("Initializing Menu UI...");
                    menuUIController.Init();

                    Debug.Log("=== MenuContext Start COMPLETE ===");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Menu Init failed: {e.Message}\n{e.StackTrace}");
                }
            }

            // 필요시 메뉴 관련 이벤트 구독
            private void OnEnable()
            {
                // EventManager.Subscribe<OnMenuEvent>(HandleMenuEvent);
            }

            private void OnDisable()
            {
                // EventManager.Unsubscribe<OnMenuEvent>(HandleMenuEvent);
            }
        }
    }
}