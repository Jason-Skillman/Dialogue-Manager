# Dialogue Manager

A RPG styled UI dialogue manager. Supports multiple lines of text, NPC portraits and option buttons.

## How to setup
You can create a new dialogue manager and add it to the hierarchy `Create/Dialogue/Dialogue Manager`. Only one dialogue manager should exist within any given scene.

You can create a new dialogue script and add it to the hierarchy `Create/Dialogue/Dialogue Script`. This is where all of the speaker's dialogue will be. Each person in a script has a sprite, name and a list of sentances. More speakers can be added to the same script.

## How to trigger the dialogue

To trigger the dialogue script, call the `TriggerDialogue()` method on the `DialogueScript` object. This can either be done through code or an event. This will then hand the script over the the dialogue manager and the UI will start playing.

## Extras

Option buttons can be added to the end of the script for more in-depth conversations. Up to four options can be added to each script. Each option has a name and the next dialogue script to play. Ex. "Yes, Maybe, No". You can also use the unity event for other kinds of interactions.
