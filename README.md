# **Dialogue Manager**

#### **Overview**
A RPG styled UI dialogue manager. Supports multiple lines of text, NPC portraits and option buttons.

#### **Documentation**
You can create a new dialogue manager and add it to your scene by right clicking in the hierarchy **"Dialogue/Dialogue Manager"**. Only one dialogue manager should exist within any given scene.

You can create a new dialogue script and add it to your scene by right clicking in the hierarchy **"Dialogue/Dialogue Script"**. This is where all of the speaker's dialogue should be. Each person in a script has a sprite, name and a list of sentances. More people can be added to the same script.

To run the dialogue script, call the ```TriggerDialogue()``` method on the DialogueScript object. This can either be done through code or an event. This will then hand the script over the the dialogue manager and the UI will start playing.

Option buttons can be added to the end of the script for more in-depth conversations. Up to four options can be added to each script. Each option has a name and the next dialogue script to play. Ex. "Yes, Maybe, No". You can also use the unity event for other kinds of interactions.
