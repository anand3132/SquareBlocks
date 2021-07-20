using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SquareBlock
{
    [Serializable]
    public class UIController : IController
    {
        public UIScreen startScreen;
        public string startUpEvent = "TriggerSplash";
        public UIScreen[] screens;
        public GameObject loaderImage;

        private UIScreen pastScreen;
        private UIScreen currentScreen;
        public UIScreen hudUIScreenPanel;
        public Transform cartContentTrasform;
        public GameObject cartCounter;
        public TMP_Text totalAmount;
        public Transform purchaseElementsParent;
        public Transform addressElementParent;
        [HideInInspector]
        public bool callPastScreenMethod = false;
        private enum ScreenState
        {
            ScreenIN
            , ScreenOut
            , StackIn
            , StackOut
            , Max
        }
        public Stack<UIScreen> screenStack = new Stack<UIScreen>();

        //public PopUPPanel messenger;
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

        //public void Toss(string _msg)
        //{
        //    if (messenger)
        //    {
        //        messenger.Message(_msg);
        //    }
        //    else
        //    {
        //        Debug.Log("<color=red> Messenger is not avilable</color>");
        //    }
        //}

        //public void Toast(string _msg, float toastEndDelay)
        //{
        //    if (messenger)
        //    {
        //        messenger.Message(_msg, 0, toastEndDelay);
        //    }
        //    else
        //    {
        //        Debug.Log("<color=red> Messenger is not avilable</color>");
        //    }
        //}


        //public void LockUI(string msg)
        //{
        //    if (messenger != null)
        //        messenger.LockUI(msg);
        //}

        //public void UnlockUI()
        //{
        //    if (messenger != null)
        //        messenger.ReleaseUI();
        //}

        public bool Loader {
            set { loaderImage.SetActive(value); }
            get { return loaderImage.activeSelf; }
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

            Debug.Log("  poped one screen!!" + currentScreen.name);
            if (currentScreen == pastScreen)
            {
                Debug.Log("<color=red> poped Same screen!!" + currentScreen.name + "</color>");
                return;
            }
            //Getcomponets for the popped screen
            // var hudScreen = currentScreen.GetComponent<HudUIScreen>();
            // currentAnimator = currentScreen.gameObject.GetComponent<Animator>();

            //if moving to hud Enable Canvas aswell
            //bool hudEnabled = (hudScreen != null);
            // SwitchToCanvas(!hudEnabled);

            ResetAllScreens();
            //Start Screen transition
            //OnScreenTransition(ScreenState.ScreenOut, pastScreen);
            //OnScreenTransition(ScreenState.StackOut, currentScreen);
            callPastScreenMethod = false;
        }

        public void HomeScreen(UIScreen uIScreen)
        {
            //if (uIScreen.ScreenPanel.GetComponent<PanelAnimation>() != null)
            //    uIScreen.ScreenPanel.GetComponent<PanelAnimation>().ScreenOut();
            //else
                uIScreen.ScreenPanel.SetActive(false);
            screenStack.Clear();
            screenStack.Push(hudUIScreenPanel);
            currentScreen = hudUIScreenPanel;
            //OnScreenTransition(ScreenState.ScreenIN, hudUIScreenPanel);
        }

        //public class ForDebug
        //{
        //    bool enabled = false;
        //    DateTime prevTime;
        //    private static readonly object padlock = new object();
        //    public static ForDebug Instance
        //    {
        //        get
        //        {
        //            lock (padlock)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new ForDebug();
        //                }
        //                return instance;
        //            }
        //        }
        //    }
        //    private static ForDebug instance = null;

        //    System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        //    public void StartTimer(string _label = null)
        //    {
        //        if (!enabled)
        //            return;
        //        timer.Start();
        //        Debug.Log("<color=yellow>---------->" + _label + " Start Time : " + DateTime.Now.ToString("h:mm:ss tt") + "------------</color>");
        //    }

        //    public void PutTimeStamp(string _label = null)
        //    {
        //        if (!enabled)
        //            return;
        //        Debug.Log("<color=yellow>---------->" + _label + " TimeStamp : " + DateTime.Now.ToString("h:mm:ss tt") + "------------</color>");
        //        Debug.Log("<color=yellow>---------- :Total Seconds : " + timer.Elapsed.TotalSeconds + "------------</color>");

        //    }
        //    public void StopTimer(string _label = null)
        //    {
        //        if (!enabled)
        //            return;
        //        timer.Stop();
        //        Debug.Log("<color=yellow>----------" + _label + " Stop Time : " + DateTime.Now.ToString("h:mm:ss tt") + "------------</color>");
        //        Debug.Log("<color=yellow>---------- Total S: " + timer.Elapsed.TotalSeconds + "------------</color>");
        //        Debug.Log("<color=yellow>---------- Total M: " + timer.Elapsed.TotalMinutes + "------------</color>");

        //    }

        //    public void ContinueTimer(string _label = null)
        //    {
        //        if (!enabled)
        //            return;
        //        timer.Restart();
        //        Debug.Log("<color=yellow>----------" + _label + " ReStart Time : " + System.Diagnostics.Stopwatch.GetTimestamp() + "------------</color>");
        //        Debug.Log("<color=yellow>----------Elapsed Time : " + timer.Elapsed + "------------</color>");

        //    }

        //}

        //private void OnScreenTransition(ScreenState _state = ScreenState.Max, UIScreen transitionScreen = null)
        //{
        //    //Debug.Log("<color=red>" + _state + " state , transitionScreen </color >" + transitionScreen.name);
        //    if (transitionScreen == null)
        //        transitionScreen = currentScreen;
        //    switch (_state)
        //    {
        //        case ScreenState.Max:
        //        {
        //        }
        //        break;
        //        case ScreenState.StackIn:
        //        {
        //            if (transitionScreen.ScreenPanel.GetComponent<PanelAnimation>() != null)
        //                transitionScreen.ScreenPanel.GetComponent<PanelAnimation>().StackIn();
        //            else transitionScreen.ScreenPanel.SetActive(false);
        //        }
        //        break;
        //        case ScreenState.StackOut:
        //        {
        //            if (transitionScreen.ScreenPanel.GetComponent<PanelAnimation>() != null)
        //                transitionScreen.ScreenPanel.GetComponent<PanelAnimation>().StackOut();
        //            else transitionScreen.ScreenPanel.SetActive(true);
        //        }
        //        break;
        //        case ScreenState.ScreenIN:
        //        {
        //            if (transitionScreen.ScreenPanel.GetComponent<PanelAnimation>() != null)
        //                transitionScreen.ScreenPanel.GetComponent<PanelAnimation>().ScreenIn();
        //            else transitionScreen.ScreenPanel.SetActive(true);
        //        }
        //        break;
        //        case ScreenState.ScreenOut:
        //        {
        //            if (transitionScreen.ScreenPanel.GetComponent<PanelAnimation>() != null)
        //                transitionScreen.ScreenPanel.GetComponent<PanelAnimation>().ScreenOut();
        //            else transitionScreen.ScreenPanel.SetActive(false);
        //        }
        //        break;
        //    }
        //}

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

        public void ResetAllScreens()
        {
            foreach (var item in screens)
            {

                if (item.ScreenPanel == null)
                    continue;
                //if (item.ScreenPanel.activeInHierarchy)
                //    OnScreenTransition(ScreenState.StackIn,item);
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
}//Getz
