using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SquareBlock
{
    public class PopUPScreen :UIScreen
    {
        public TextMeshProUGUI msgText;
        // Start is called before the first frame update
        [Range(0f,2f)]
        public float TossStartDelay;

        [Range(0f, 10f)]
        public float TossEndDelay;
        public Button RecoveryButton;


        public RectTransform rt;
        public Text txt;

        private void Start()
        {
            if (msgText == null)
            {
                msgText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        public void Message(string message,bool lockUI=false)
        {
            if (msgText != null)
            {
                if (OnLock) {
                    LockUI(message);
                }else
                StartCoroutine(DisplayMessege(message));
            }
        }

        public void Message(string message,float toastStartDelay, float toastEndDelay, bool lockUI = false)
        {
            if (msgText != null )
            {
                if(OnLock) {
                    LockUI(message);
                }else
                StartCoroutine(DisplayMessege(message, toastStartDelay, toastEndDelay));
            }
        }

        private bool OnLock;
        public  void LockUI(string message)
        {
            OnLock = true;
            screenPanel.SetActive(true);
            if (RecoveryButton) {
                RecoveryButton.interactable = true;
                RecoveryButton.onClick.AddListener(()=>ReleaseUI());
            }
            screenPanel.GetComponent<Image>().enabled = true;
            Color tmpc = screenPanel.GetComponent<Image>().color;
            tmpc.a = 0;
            screenPanel.GetComponent<Image>().color = tmpc;
            if (msgText != null)
                msgText.text = message;
            Debug.Log("<color=red>UI is onLock....</color>");
        }

        public void ReleaseUI()
        {
            OnLock = false;
            screenPanel.SetActive(false);
            if (RecoveryButton)
                RecoveryButton.interactable = false;

            if (msgText != null)
                msgText.text = "";
            screenPanel.GetComponent<Image>().enabled = false;

        }

        private IEnumerator DisplayMessege(string msg)
        {
            yield return new WaitForSeconds(TossStartDelay);
            screenPanel.SetActive(true);
            msgText.text = msg;
            yield return new WaitForSeconds(TossEndDelay);
            msgText.text = "";
            screenPanel.SetActive(false);
        }

        private IEnumerator DisplayMessege(string msg,float toastStartDelay,float toastEndDelay)
        {
            yield return new WaitForSeconds(toastStartDelay);
            screenPanel.SetActive(true);
            msgText.text = msg;
            yield return new WaitForSeconds(toastEndDelay);
            msgText.text = "";
            screenPanel.SetActive(false);
        }

        protected override void OnEvent(string eventName, params object[] _eventData)
        {
            if (eventName == "TossMessage"&&_eventData !=null)
            {
                switch (_eventData.Length) {
                    case 0: {
                        Message("No msg found");
                        break;
                    }
                    case 1: {
                        Message((string)_eventData[0]);
                        break;
                    }
                    case 2: {
                        Message((string)_eventData[0], (float)_eventData[1], 0);
                        break;
                    }
                    case 3: {
                        Message((string)_eventData[0], (float)_eventData[1], (float)_eventData[2]);
                        break;
                    }
                    case 4: {
                        Message((string)_eventData[0], (float)_eventData[1], (float)_eventData[2], (bool)_eventData[3]);
                        break;
                    }
                    default: {
                        Message("No msg found");
                        break;
                    }
                }
            }
            if (eventName == "LockUIWithMessege")
            {
                LockUI((string)_eventData[0]);
            }
            if (eventName == "ReleaseUIWithMessege")
            {
                ReleaseUI();
            }
        }

        public override void RegisterEvents()
        {
            ListenerController.Instance.RegisterObserver("TossMessage", this);
            ListenerController.Instance.RegisterObserver("LockUIWithMessege", this);
            ListenerController.Instance.RegisterObserver("ReleaseUIWithMessege", this);
        }

        public override void UnRegisterEvents()
        {
            StopAllCoroutines();
            ListenerController.Instance.UnRegisterObserver("TossMessage", this);
            ListenerController.Instance.UnRegisterObserver("LockUIWithMessege", this);
            ListenerController.Instance.UnRegisterObserver("ReleaseUIWithMessege", this);
        }
    }
}