# **Command Console**

#### **Overview**
A debug command console that can be used at runtime and easily extendable.

#### **Documentation**
You can create a new console and add it to your scene by right clicking in the hierarchy "Conosle/Command Console".

Only one command console should exist within any given scene.

To open the console at runtime use the tilde key "~". This can be disabled in the inspector for custom input remaping.

#### **API**
Custom commands can be written for the command console. This is the heart and soul of the console.

A small list of commands have already been written as examples. Some examples include print, load scene and unload scene. They can be found at "Runtime/Scripts/Commands" starting at the root of the package.

 ### **ICommand**
 To create a custom command create a new script and extend the ICommand interface.

|Property/Method|Description|
|---|---|
|`Label`| This is the main name/label of the command you are creating.|
|`SuggestedArgs`| This is an array of args to let the user know what kind of data to put. Ex. int or string.|
|`Action(args)`| This is the executed code when the command has been activated. Commands are activated by running them in the console. Args should match suggested args correctly.|
