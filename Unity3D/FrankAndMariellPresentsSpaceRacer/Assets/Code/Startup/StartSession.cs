using System;
using Code.Store;
using UnityEngine;

namespace Code.Startup
{
    public class StartSession
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            SessionStore.SessionId = Guid.NewGuid().ToString();
            Debug.Log($"Session ID: '{SessionStore.SessionId}'");
        }

        [RuntimeInitializeOnLoadMethod]
        static void OnSecondRuntimeMethodLoad()
        {
            Debug.Log($"Shold be same Session ID: '{SessionStore.SessionId}'");
        }

    }
}