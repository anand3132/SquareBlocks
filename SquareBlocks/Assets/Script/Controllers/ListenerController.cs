using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
namespace SquareBlock
{
    public interface EventObserver
    {
        void OnGetzEvent(string eventName, params object[] _eventData);

    }

    public class ListenerController : MonoBehaviour
    {
        Dictionary<string, List<EventObserver>> observers = new Dictionary<string, List<EventObserver>>();

        public static ListenerController Instance { get; private set; }
        public static void Initiate() { }
        private void Awake()
        {
            if (Instance)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        public void Reset()
        {
            foreach (var d in observers)
            {
                d.Value.Clear();
            }
            observers.Clear();
        }

        public bool RegisterObserver(string eventName, EventObserver observer)
        {
            if (eventName.Length == 0 || observer == null)
            {
                return false;
            }
            if (!observers.ContainsKey(eventName))
            {
                observers[eventName] = new List<EventObserver>();
            }

            observers[eventName].Add(observer);
            return true;
        }

        public bool UnRegisterObserver(string eventName, EventObserver observer)
        {
            if (eventName.Length == 0 || observer == null)
            {
                return false;
            }

            if (!observers.ContainsKey(eventName))
            {
                return false;
            }

            var list = observers[eventName];
            return list.Remove(observer);
        }

        public void UnRegisterObserver(EventObserver observer)
        {
            if (observer == null)
            {
                return;
            }

            foreach (var d in observers)
            {
                d.Value.Remove(observer);
            }
        }

        public void DispatchEventWithDelay(string _eventName, int _delay, params object[] _eventData)
        {
            StartCoroutine(DispatchWithDelayCoroutine(_eventName, _delay, _eventData));
        }

        IEnumerator DispatchWithDelayCoroutine(string _eventName, int _delay, params object[] _eventData)
        {
            yield return new WaitForSeconds(_delay);
            DispatchEvent(_eventName, _eventData);
        }
        public void TailGateEvent(string ObserverEvent, string triggerEvent, params object[] _eventData)
        {
            if (ObserverEvent == null || ObserverEvent == triggerEvent)
            {
                Debug.Log("ObserverEvent is null Given event: <color=red>" + triggerEvent + " </color> Can't be tailgated ");
                return;
            }
            else
            {
                if (!observers.ContainsKey(triggerEvent) || !observers.ContainsKey(ObserverEvent))
                {
                    Debug.Log("Cant able to Dispatched event:: <color=red> " + triggerEvent + "</color>");
                    return;
                }
                else
                {
                    //  observers.Add(triggerEvent, _eventData.OfType<EventObserver>().ToList());
                    if (currentDispatchList != null)
                    {
                        currentDispatchList.AddRange(_eventData.OfType<EventObserver>().ToList());
                    }
                }
            }
        }

        private List<EventObserver> currentDispatchList;

        private string currentDispatchEvent = null;
        public void DispatchEvent(string eventName, params object[] _eventData)
        {

            if (eventName.Length == 0)
            {
                return;
            }

            if (!observers.ContainsKey(eventName))
            {
                Debug.Log("Cant able to Dispatched event:: <color=red> " + eventName + "</color>");

                return;
            }

            currentDispatchList = observers[eventName];
            foreach (var o in currentDispatchList)
            {
                currentDispatchEvent = eventName;
                Debug.Log("Dispatching event:: <color=green>" + eventName + "</color>");
                o.OnGetzEvent(eventName, _eventData);
            }
            currentDispatchEvent = null;
        }
        private struct ObserverEventData
        {
            public string onHoldEventName;
            public object[] onHoldEventData;
        }

        ObserverEventData observerEventData = new ObserverEventData();

        public void HoldEvent(string eventName, params object[] _eventData)
        {
            observerEventData.onHoldEventName = eventName;
            observerEventData.onHoldEventData = _eventData;
            Debug.Log(" event:: <color=green>" + eventName + " On Hold</color>");
        }

        public string ReleaseEvent(out object[] _eventData)
        {
            string ev = observerEventData.onHoldEventName;
            object[] evData = observerEventData.onHoldEventData;
            observerEventData.onHoldEventName = null;
            observerEventData.onHoldEventData = null;
            _eventData = evData;

            if (string.IsNullOrEmpty(ev))
                Debug.Log("<color=green>No Event On Hold </color>");
            else
                Debug.Log(" Relese event:: <color=green>" + ev + " </color>");

            return ev;
        }

        public string ReleaseEvent()
        {
            string ev = observerEventData.onHoldEventName;
            observerEventData.onHoldEventName = null;
            observerEventData.onHoldEventData = null;

            if (string.IsNullOrEmpty(ev))
                Debug.Log("<color=green>No Event On Hold </color>");
            else
                Debug.Log(" Relese event:: <color=green>" + ev + " </color>");
            return ev;
        }

    }
     
}
