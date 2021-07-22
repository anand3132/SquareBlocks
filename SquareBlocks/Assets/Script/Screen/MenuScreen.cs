using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SquareBlock
{
    public class MenuScreen : UIScreen
    {
        public List<GameData> gdataList;
        private GameData gameData = null;
        public GameObject menuCentralPanel;
        public GameObject gameSelectButton;
        public override void RegisterEvents()
        {
            ListenerController.Instance.RegisterObserver("UpdateGameData", this);
            ListenerController.Instance.RegisterObserver("UpdateMenuScreen", this);
        }

        public override void UnRegisterEvents()
        {
            ListenerController.Instance.UnRegisterObserver("UpdateGameData", this);
            ListenerController.Instance.UnRegisterObserver("UpdateMenuScreen", this);
        }
        private void ReSetUIElements()
        {
            Transform[] childs = menuCentralPanel.gameObject.GetComponentsInChildren<Transform>(true)
            .Where(x => x.gameObject.transform.parent != menuCentralPanel.gameObject.transform.parent).ToArray();

            if (childs != null)
            {
                foreach (Transform item in childs)
                {
                    Destroy(item.gameObject);
                }
            }
        }
        protected override void OnEvent(string eventName, params object[] _eventData)
        {
            if (eventName == "UpdateGameData" && _eventData[0] != null)
            {
                gdataList = _eventData[0] as List<GameData>;
            }

            if (eventName == "UpdateMenuScreen")
            {
                ReSetUIElements();
                if (gdataList != null && gdataList.Count > 0)
                {
                    for(int i=0;i< gdataList.Count;i++)
                    {
                        int _index = i;
                        GameObject buttonobj= Instantiate(gameSelectButton, menuCentralPanel.transform);
                        buttonobj.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                        buttonobj.GetComponent<Button>().onClick.AddListener(()=> OnGameSelect(_index));
                    }
                }
                else
                {
                    Debug.Log("<color=red> Game Data is Not avilable in the Path...</color>");
                }
            }
        }
        public void OnGameSelect(int _index)
        {
            GameData gameData = null;
            if (_index < gdataList.Count)
            {
                gameData = gdataList[_index];
            }
            if (gameData == null)
                return;
            UIController.Instance.SwitchScreenTo(typeof(GameScreen));
            ListenerController.Instance.DispatchEvent("StartGame", gameData);

        }

        public void BackButtonOnClick()
        {
            UIController.Instance.BackToPastScreen();
        }
    }
}