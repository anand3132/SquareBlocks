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
        public Button RetryButton;


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
            if (msgText != null && !OnLock)
            {
                StartCoroutine(DisplayMessege(message));
            }
        }

        public void Message(string message,float toastStartDelay, float toastEndDelay, bool lockUI = false)
        {
            if (msgText != null && !OnLock)
            {
                StartCoroutine(DisplayMessege(message, toastStartDelay, toastEndDelay));
            }
        }

        private bool OnLock;
        public  void LockUI(string msg)
        {
            OnLock = true;
            screenPanel.SetActive(true);
            RetryButton.interactable = true;
            if (msgText != null)
                msgText.text = msg;
        }

        public void ReleaseUI()
        {
            OnLock = false;
            screenPanel.SetActive(false);
            RetryButton.interactable = false;

            if (msgText != null)
                msgText.text = "";
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

        public void RetryButtonOnClick()
        {
            //ListenerController.Instance.DispatchEvent("CheckInterNet");
        }

        protected override void OnEvent(string eventName, params object[] _eventData)
        {
            if (eventName == "TossMessage")
            {
                Message((string)_eventData[0]);
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