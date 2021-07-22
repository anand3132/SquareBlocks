
namespace SquareBlock
{
    public class SplashScreen : UIScreen
    {
        public override void RegisterEvents(){}

        public override void UnRegisterEvents(){}

        protected override void OnEvent(string eventName, params object[] _eventData){}

        public void GameEnterButtonOnClick()
        {
            UIController.Instance.SwitchScreenTo(typeof(MenuScreen));
            ListenerController.Instance.DispatchEvent("UpdateMenuScreen");
            ListenerController.Instance.DispatchEvent("TossMessage", "Hi Welcome to SquareBlocks");
        }

    }
}
