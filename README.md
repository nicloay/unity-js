# Introduction
ClearScript Unity3D JavaScript demo
The purpose of this project is to showcase how to execute JavaScript code within the Unity3D runtime environment using the [Microsoft.ClearScript](https://github.com/microsoft/ClearScript) library, 
which provides a way to run JavaScript code with the V8 engine.

To reduce complexity, the JavaScript files in this project are structured using CommonJS modules. 
However, you are free to extend the JSSandbox code to fit your needs. You can refer to the [CommonJS documentation](https://nodejs.org/api/modules.html) for more information on how to work with CommonJS modules.

Please be aware that, currently, only the Mono backend is supported in this project, 
and IL2CPP will not work out of the box, please refer to 
[this link](https://forum.unity.com/threads/il2cpp-does-not-support-marshaling-delegates-that-point-to-instance-methods-to-native-code.1046143/#post-6770206) 
for more information.

Project dependencies include **Microsoft.ClearScript** and **UniTask**, both of which are released under the MIT license.
* https://github.com/microsoft/ClearScript/blob/master/License.txt
* https://github.com/Cysharp/UniTask/blob/master/LICENSE

# Project Structure

| Folder/File | Description |
| --- | --- |
| `Assets/` | Top-level folder |
| `├── Demo/` | Demo scenes which works in editor or play mode |
| `├── Editor/` | [Optional] Only build postprocess stored here |
| `├── JSContainer/` | ClearScript adapter initializes the JS sandbox with specified JavaScript code. |
| `│   ├── Editor/` | The editor build process copies unmanaged DLL files to the build's proper location. |
| `│   ├── Plugins/` | ClearScript and V8 DLLs for Windows 64-bit builds. |
| `│   └── Scripts/` | JSSandbox code |
| `│       ├── Interop/` | Set of objects exposed as CommonJS modules for use in JS scripts. |
| `│       └── JSSandbox.cs` | ClearScript wrapper |
| `├── StreamingAssets/` | Contains JavaScript files evaluated at runtime and copied as-is to the target build. |
| `└── Tests/` | Automated unit testing template with async JSSandbox example. |

# ClearScript Unity Installation from Scratch

The project already contains everything you need to run JS in a V8 container, 
currently using version 7.4.0. If you wish to update to the latest version, you can do so by following these steps:
1. Clear the Plugins folder, or create it if it doesn't already exist.
2. Create a new .NET project and add the following NuGet packages:
  * Microsoft.ClearScript.Core
  * Microsoft.ClearScript.V8
  * Microsoft.ClearScript.V8.ICUData
  * Microsoft.ClearScript.V8.Native.win-x64
  * Newtonsoft.Json
3. Copy the necessary DLL files from the NuGet packages to the "Plugins" folder in your Unity project.
  * For managed DLLs, copy them from the "lib/net45" folder.
  * For unmanaged (native.win-x64) DLLs, copy them from the "runtimes/win-x64/native" folder and place them in the same relative path in the "Plugins" folder (Plugins/runtimes/win-x64/native). 
    > **Note** 
    > If you are copying DLL files for a different platform, you will need to modify the "JSContainer/Editor/BuildPostProcess.cs" file to support copying them to the target build folder.
    
# Project Setup
1. Only the mono backend is currently supported, and IL2CPP will result in numerous errors.
2. As the project uses the [dynamic](https://learn.microsoft.com/en-us/archive/msdn-magazine/2011/february/msdn-magazine-dynamic-net-understanding-the-dynamic-keyword-in-csharp-4) keyword feature of C#, you must have ".NET Framework" instead of 2.1 as API compatibility level.
3. Ensure that you have the appropriate native DLLs that match the target build platform (e.g., Linux or Win-64).  
4. Unitask is added as an external package through the package manager to allow the use of UniTask.DelayFrame. If you don't need it, you can skip this step.

# Usage
## Simple Demo scene

Open the SimpleDemo scene to see how SimpleDemoRunner.cs component is utilized to run simple-demo.js file. 
Take note of how you can create a sandbox and call methods on JavaScript, 
> **Info**
> onStart is dynamic method and is part of js file
```csharp
private void Start()
{
    _container = new JSSandbox();
    var filePath = Path.Combine(Application.streamingAssetsPath, "simple-demo.js");
    _module = _container.EvaluateCommonJSModule(filePath);
    _module.onStart();
}
```        

as well as how JavaScript can call managed code.
> **Info**
> ~engine is exposed api from Interop folder of JSContainer

```javascript
const engine = require("~engine")

module.exports.onStart = async function() {
    await engine.sendMessage("" + dt)
}
```

## Async Demo Runner
Here you can see how JavaScript waits one frame and then utilizes Unity's Debug.DrawLine API. The frame waiting is implemented using UniTask.DelayFrame in a fast and efficient manner.

Don't forget to enable gizmos in the editor so that you can see the spinning lines.

```javascript
module.exports.debugDrawLine = async function () {
   ...
    for (let i = 0; i < 10000; i++) {
        const angle = i / 360.0;
        ...  
        Debug.DrawLine(position, position2, color, Math.fround(1));
        await waitOneFrame();
    }
}
```
and this is what you will se in Editor

![image](https://user-images.githubusercontent.com/1671030/224580822-5826b29c-7f46-4e9d-b8df-a88f1e528628.png)

## Message Exchange Bus
This example demonstrate how to pass messages from JS to NET and back. 
* Unity 
  * The **onStart** function is triggered once when the engine initializes.
  * The **onUpdate** function is triggered at every frame in JS module.
  * Unity listens to the input and fills the message queue with KeyDown and KeyUp messages.
* JS
  * When onStart is triggered, a message is sent to Unity to spawn a cube.
  * When onUpdate is triggered, the following happens:
    * Messages are received from Unity about input and used to modify the scale of the object.
    * Messages are sent back to Unity with transform changes that need to be applied to the spawned cube.

> **Warning** Please note that there are certain areas that could be optimized in the implementation. 
> For example, in the IMessage JSON serialization phase, 
> it is possible to avoid this step by passing references to the objects using ClearScript, 
> which allows for sharing of the array itself instead of going through the process of JSON conversion.

Upon starting the demo scene, you will notice a rotated cube. 
Pressing and holding the space bar will cause it to scale up, and releasing the key will scale it back down to its normal size.

![Animation](https://user-images.githubusercontent.com/1671030/227182506-fc2ece81-b48d-43bc-b05a-7594f7efaf69.gif)

In case if you want to add functionality you should
1. Create new message which implement IMessage interface and has [MessageIdAttribute] on it
2. If you want to handle specific message on Unity side please see MessageHandler, what it does is only call static methods according to incoming messages. You can keep these handlers anywhere, but in the project they are in the same file as message itself. see for example EntityAddMessage.cs
3. If you have new data type and want to continue use JSON serialization phase, you should write custom converter. See for example TransformDataConverter.cs. And offcourse better to verify functionality with UnitTests, see TestJson.cs

# Best Practices for Using the Library

* Always dispose of JSSandbox when you no longer need it, or in the OnDestroy method.
* JavaScript uses 64-bit floats, so use Math.fround if you want to pass these values to functions like Vector3(x,y,z), as shown in the AsyncDemoRunner.
* Even if you are using Windows build, V8 still uses '/' as the path separator, so do not use any backslashes in file names.
* To call methods defined in the JS CommonModules, you can use the **dynamic** keyword to retrieve the returned value from the EvaluateCommonJSModule function. This allows you to call a method by name that is defined in the JS file.". 
  > **Note** Take a look at how the function 'debugDrawLine' is both defined and called.
* If you are unsure about how something works, you can create a UnityTest and use the TestRunner for debugging and testing purposes. This project also provides a template for creating UnityTests that can be used for these purposes.

Although the threshold is set, this test can still fail occasionally even though it runs 25 times.
```csharp
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
```
![image](https://user-images.githubusercontent.com/1671030/224582119-95e37663-c38d-49fb-9239-6ef120fcfa84.png)

# Extending Project (interop operations)

There are several ways to extend JSSandbox:
* If you want to create a static library and use it in JS, check the Interop folder. Create a new script in the same assembly and use the [InteropModule] attribute.
* If you want to expose UnityEngine's built-in types like Vector3, Color (as static or constructors), check the ExposedTypes field in JSSandbox.cs.
* If you want to make a single function available for a specific JS script, use sandbox.Script.yourGlobalFunction = new Func<object>(...);."
* Additionally, you can use ClearScript methods such as AddHostType to add objects. Check out some [examples](https://microsoft.github.io/ClearScript/Examples/Examples.html) for more information.
