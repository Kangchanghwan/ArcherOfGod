using Controller.Entity;
using MVC.Controller.Game;
using UnityEngine;
using Util;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        private void Start()
        {

            GameUIController uiController = FindFirstObjectByType<GameUIController>();
            uiController.Init();
            
            EntityControllerBase[] bases = FindObjectsByType<EntityControllerBase>(FindObjectsSortMode.None);
            foreach (var controllerBase in bases)
            {
                controllerBase.Init();
            }
        }
    }
}