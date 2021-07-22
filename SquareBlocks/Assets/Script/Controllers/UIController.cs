using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquareBlock
{
    enum UIBehaviour {
        MAX
        , ACTICVE
        , PASSIVE
        , WARNING
        , LOCK
        , ERROR
        , OPEAN
        , CLOSE
    }


    [Serializable]
    public class UIController : IController
    {
        public UIScreen startScreen;
        public string startUpEvent ;
        public UIScreen[] screens;
        public GameObject loaderImage;

        private UIScreen pastScreen;
        private UIScreen currentScreen;

        private enum ScreenState
        {
            ScreenIN
            , ScreenOut
            , StackIn
            , StackOut
            , Max
        }
        public Stack<UIScreen> screenStack = new Stack<UIScreen>();
        public static UIController Instance { get; private set; }

        private UIController() { }

        private bool isUiControllerInitialised = false;

        private void InitUiController()
        {
            if (isUiControllerInitialised)
            {
                Debug.Log("<color=red> Error: Multi Initialistion on UIController.... </color>");
                //return;
            }
            if (Instance == null)
                Instance = this;

            screens = gameObject.GetComponentsInChildren<UIScreen>(true);
            SwitchScreenTo(startScreen.GetType(), false);
            if (!string.IsNullOrEmpty(startUpEvent))
            {
                ListenerController.Instance.DispatchEvent(startUpEvent);
            }

            if (!loaderImage)
                Debug.Log("Cant able to find loader component");
            else
            {
                loaderImage.SetActive(false);
            }
            isUiControllerInitialised = true;
        }

        public void SwitchToCanvas(bool _status)
        {
            ResetAllScreens();
        }

        public void BackToPastScreen()
        {
            Debug.Log("BackButton clicked!!");
            if (screenStack.Count < 1)
            {
                Debug.Log("Nothig to pop!!");
                return;
            }
            //switch past screen Data
            pastScreen = currentScreen;
            Debug.Log("pastScreen " + pastScreen.name);
            //pastScreenAnimator = currentAnimator;
            currentScreen = screenStack.Pop();
            Debug.Log("currentScreen " + currentScreen.name);

            Debug.Log("  poped one screen!! " + currentScreen.name);
            if (currentScreen == pastScreen)
            {
                Debug.Log("<color=red> poped Same screen!!" + currentScreen.name + "</color>");
                return;
            }
            ResetAllScreens();
            currentScreen.ScreenPanel.SetActive(true);
        }
        public void SwitchScreenTo(Type _type, bool storeHistory = true)
        {
            Debug.Log("onSwitching..");
            //bool hudEnabled = (_type == typeof(HudUIScreen));
            //SwitchToCanvas(!hudEnabled);
            ResetAllScreens();
            if (currentScreen)
            {
                pastScreen = currentScreen;
                Debug.Log("past screen name "+pastScreen.name);
              //  pastScreenAnimator = pastScreen.gameObject.GetComponent<Animator>();
            }

            currentScreen = GetScreenObjectType(_type);


            if (pastScreen != null)
            {
                int currentIndex = currentScreen.transform.GetSiblingIndex();
                int pastIndex = pastScreen.transform.GetSiblingIndex();
                if (currentIndex < pastIndex)
                {
                    Debug.Log("<color=green>currentScreen Index Changed... " + currentScreen.name + " </color>");
                    Debug.Log("<color=green> CurrentScreen Index: " + currentIndex + " > PastScreen Index: " + pastIndex + " </color>");
                    currentScreen.transform.SetSiblingIndex(pastIndex);
                }
                //OnScreenTransition(ScreenState.StackIn,pastScreen);
                pastScreen.ScreenPanel.SetActive(false);
            }

            if (storeHistory)
                screenStack.Push(pastScreen);

            if (currentScreen.ScreenPanel == null) { Debug.Log("<color=red>" + _type.Name + " Panel Not avilable.. </color>"); return; }


            if (currentScreen.ScreenPanel)
            {
                ////////////currentAnimator = currentScreen.gameObject.GetComponent<Animator>();
                ////////////if (currentAnimator)
                ////////////    OnScreenTransition(ScreenState.ScreenIN, currentAnimator);
                //if (currentScreen.ScreenPanel.GetComponent<PanelAnimation>() != null)
                //    OnScreenTransition(ScreenState.ScreenIN, currentScreen);
                //else
                    currentScreen.ScreenPanel.SetActive(true);
            }

            // currentScreen.ScreenPanel = GetScreenObject(_type);
            //currentScreen.ScreenPanel.SetActive(true);
            Debug.Log("=========================================[  <color=#008080>" + _type.Name + "</color>  ]===============================================================");
        }

        public GameObject GetScreenObject(Type _screen)
        {
            foreach (var item in screens)
            {
                if (item.GetType() == _screen)
                {
                    Debug.Log("Got the screen " + _screen.Name);
                    return item.ScreenPanel;
                }
            }
            Debug.Log("No screen found!!! " + _screen.Name);

            return null;
        }

        public UIScreen GetScreenObjectType(Type _screen)
        {
            foreach (var item in screens)
            {
                if (item.GetType() == _screen)
                {
                    //                    Debug.Log("Got the screen " + _screen.Name);
                    return item;
                }
            }
            Debug.Log("No screen found!!! " + _screen.Name);

            return null;
        }

        public void ToastMsg(string _msg) {

            ListenerController.Instance.DispatchEvent("TossMessage", _msg);
        }
        public void ToastMsg(string _msg, float toastStartDelay,float toastEndDelay,bool lockUI=false) {

            ListenerController.Instance.DispatchEvent("TossMessage", _msg, toastStartDelay, toastEndDelay, lockUI);
        }
        public void LockUI(string _msg) {

            ListenerController.Instance.DispatchEvent("LockUIWithMessege", _msg);
        }
        public void Release(string _msg) {

            ListenerController.Instance.DispatchEvent("ReleaseUIWithMessege", _msg);
        }
        public void ResetAllScreens()
        {
            foreach (var item in screens)
            {

                if (item.ScreenPanel == null)
                    continue;
                if (item.ScreenPanel.activeInHierarchy)
                    item.ScreenPanel.SetActive(false);
            }
        }

        public override void RegisterEvents()
        {
            ListenerController.Instance.RegisterObserver("InitializeUIController", this);
        }

        public override void UnRegisterEvents()
        {
            ListenerController.Instance.UnRegisterObserver("InitializeUIController", this);
        }

        protected override void OnEvent(string eventName, params object[] _eventData)
        {
            if (eventName == "InitializeUIController")
            {
                if (isUiControllerInitialised && Instance)
                {
                    Debug.Log("<color=red>UiController Already Initialise..<color>");
                }
                else
                {
                    InitUiController();
                }
            }
        }
    }//UIController
}//SquareBlock