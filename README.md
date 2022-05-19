# unity-audio-system
Audio System for Unity

# install
Use this repository directly in Unity.

### Dependencies
* https://github.com/KleinerHacker/unity-editor-ex
* https://github.com/KleinerHacker/unity-common-ex
* https://github.com/KleinerHacker/unity-pref-ex

### Open UPM
URL: https://package.openupm.com

Scopes:
* org.pcsoft

# usage
To configure see project settings and setup default volumes and mixer groups. Additional you can create custom SFX systems.

### API
To play Audio Clip (own Asset type!) use `SfxSystem`:

```CSharp
SfxSystem.Default.Play...
SfxSystem.Get("mykey").Play...
```

There are three methods to play SFX:
* `PlayOneShot` - Play one times the given clip (supports `AudioClip` too)
* `PlayAmbience` - Play the clip multiple times depends on setup delay values - Stop it via result `ISfxPlayedClip`
* `PlayLoop` - Replay clip after finished - Stop it via result `ISfxPlayedClip`

There are two types of SFX Clips:
* `RandomSfxClip` - A list of audio clips. The next played clip is randomized.
* `SimpleSfxClip` - A single audio clip.

You can always setup a custom overwritten value for each audio clip via this asset.

### Prefs Support
If you change the value programmatically via `SfxSystem.Default/Get("mykey).Volume` the new value is stored into Player Prefs. On reload it will load volume from with player prefs. Naming is identifier + ".volume".
