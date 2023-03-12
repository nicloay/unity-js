using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JSContainer;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /// <summary>
    /// Template which demonstrate how to use JSSandbox with unit tests
    /// </summary>
    public class JSContainerTest
    {
        private const string JS_FILE_NAME = "unit-test.js";

        [UnityTest]
        [Repeat(25)]
        public IEnumerator OneFrameWaiteTest()
        {
            return UniTask.ToCoroutine(async () =>
            {
                using var sandbox = new JSSandbox();
                sandbox.Script.delay =
                    new Func<int, object>(Task
                        .Delay); // you can also put that in to JSSandbox if you want to use it in many paces
                dynamic module =
                    sandbox.EvaluateCommonJSModule(Path.Combine(Application.streamingAssetsPath, JS_FILE_NAME));

                const int waitTimeMS = 500;
                var timer = new Stopwatch();
                timer.Start();
                await module.asyncWait(500);
                timer.Stop();
                Assert.That(timer.Elapsed.Milliseconds, Is.EqualTo(waitTimeMS).Within(15).Percent);
            });
        }
    }
}