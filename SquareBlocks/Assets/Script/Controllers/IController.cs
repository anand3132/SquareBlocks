using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquareBlock {
    public abstract class ObBase : MonoBehaviour, EventObserver {
        protected virtual void OnEnable()
        {
            try
            {
                this.RegisterEvents();
            }
            catch (Exception e)
            {
                Debug.Log("<color=red>Exception :</color>" + e.StackTrace);
            }
        }
        protected virtual void OnDisable()
        {
            try
            {
                this.UnRegisterEvents();
            }
            catch (Exception e)
            {
                Debug.Log("<color=red>Exception :</color>" + e.StackTrace);
            }
        }

        public void OnGetzEvent(string eventName, params object[] _eventData)
        {
            try
            {
                this.OnEvent(eventName, _eventData);
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.Log("Expecting _eventData on <color=red> [ </color> " + eventName + " <color=red>]: Exception :</color>" + ex.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Log("<color=red>Exception :</color>" + e.StackTrace);
            }
        }
        public abstract void RegisterEvents();
        public abstract void UnRegisterEvents();
        protected abstract void OnEvent(string eventName, params object[] _eventData);
    }

    public abstract class UIScreen : ObBase {
        [SerializeField]
        protected GameObject screenPanel;
        public int ScreenID;
        public GameObject ScreenPanel { get { return screenPanel; } private set => _ = screenPanel; }
    }

    public abstract class IController : ObBase {
    
    }

    public abstract class IProprties : ObBase {
    }
}//SquareBlock
