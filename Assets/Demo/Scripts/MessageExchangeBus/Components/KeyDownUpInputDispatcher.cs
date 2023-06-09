﻿using UnityEngine;

namespace ClearScriptDemo.Demo.MessageExchangeBus.Components
{
    /// <summary>
    ///     Listen key input, and forward key button press/release to the message queue
    /// </summary>
    public class KeyDownUpInputDispatcher : MonoBehaviour
    {
        private static readonly KeyCode[] LISTEN_CODES =
        {
            KeyCode.Space
        };

        private MessageQueue _queue;

        private void Update()
        {
            foreach (var code in LISTEN_CODES)
            {
                if (Input.GetKeyDown(code)) _queue.AddMessage(new KeyDownMessage { Key = code.ToString().ToLower() });

                if (Input.GetKeyUp(code)) _queue.AddMessage(new KeyUpMessage { Key = code.ToString().ToLower() });
            }
        }

        // Factory method which will create instance on the host object
        public static void Instantiate(GameObject parentObject, MessageQueue queue)
        {
            var instance = parentObject.AddComponent<KeyDownUpInputDispatcher>();
            instance._queue = queue;
        }
    }
}