using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SquareBlock
{
    public class GameScreen : UIScreen
    {
        public override void RegisterEvents(){}

        public override void UnRegisterEvents(){}

        protected override void OnEvent(string eventName, params object[] _eventData){}

        // Start is called before the first frame update
        public void OnGameExit()
        {
            UIController.Instance.SwitchScreenTo(typeof(MenuScreen));
            ListenerController.Instance.DispatchEvent("StopGame");

        }
        public void BackButtonOnClick()
        {
            ListenerController.Instance.DispatchEvent("StopGame");
            UIController.Instance.BackToPastScreen();
        }
    }
}