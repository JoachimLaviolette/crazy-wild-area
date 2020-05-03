using UnityEngine;
using System;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          FunctionPeriodic class
 * ------------------------------------------------
 */

namespace JoaDev
{
    public class FunctionPeriodic
    {
        /**
         * Inner class handling the function periodic behaviour
         * Allow to use MonoBehaviour functions
         */
        private class MonoBehaviourHook: MonoBehaviour
        {
            public Action onUpdate;

            private void Update()
            {
                onUpdate?.Invoke();
            }
        }

        private Action _action, _destroyFunction;
        private float _startAfter, _startAfter_tmp, _timer, _timer_tmp;
        private GameObject _functionPeriodicGO;

        /**
         * Create function
         */
        public static FunctionPeriodic Create(Action action, string actionName, float startAfter, float timer, Action destroyAction = null)
        {
            GameObject functionPeriodicGO = new GameObject(actionName, typeof(MonoBehaviourHook));
            FunctionPeriodic functionPeriodic = new FunctionPeriodic(action, startAfter, timer, functionPeriodicGO, destroyAction);
            functionPeriodicGO.GetComponent<MonoBehaviourHook>().onUpdate = functionPeriodic.Update;

            return functionPeriodic;
        }

        /**
         * Private constructor
         */
        private FunctionPeriodic(Action action, float startAfter, float timer, GameObject functionPeriodicGO, Action destroyFunction = null)
        {
            _action = action;
            _startAfter = startAfter;
            _startAfter_tmp = startAfter;
            _timer = _timer_tmp = timer;
            _functionPeriodicGO = functionPeriodicGO;
            _destroyFunction = destroyFunction;
        }

        /**
         * Update the function periodic model
         */
        public void Update()
        {
            _startAfter_tmp -= Time.deltaTime;
            _timer_tmp -= Time.deltaTime;

            if (_startAfter_tmp < 0f && _timer_tmp < 0f)
            {
                // Trigger the action
                _action();
                // Reset the timer and the start after
                Reset();
            }

            Destroy();
        }

        /** 
         * Destroy the game object associated to function periodic
         */
        private void Reset()
        {
            _startAfter_tmp = _startAfter;
            _timer_tmp = _timer;
        }

        /**
         * Destroy the function periodic if needed
         */
        private void Destroy()
        {
            _destroyFunction?.Invoke();
        }
    }
}