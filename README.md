# TorqueLab v0.1 - Pre-Alpha 
**Pre-Alpha version warning (Messy scripts and unused files present)**

## What's TorqueLab
TorqueLab is a completly revamping on the native Torque3D game editors (tools folder). The initial releases doesn't provide much new features, the work is focus on the scripts structure and the interface. Once those are completed, new features would be added.


## Instalation
* Paste tlab/ folder into your game folder
* Change the tool folder in the root main.cs file. (To use a pref, visit [Advanced Installation](https://github.com/Mud-H/TorqueLab/wiki/Advanced-Installation#add-a-pref-to-set-native-editor-or-torquelab) )
```
  // load tools scripts if we're a tool build
if (isToolBuild())
    $userDirs = "tlab;" @ $userDirs;  //replaced tools with tlab
```
* If using without the Native tools folder, you need to paste the supplied tools/ folder since some images path are set directly in the engine code
f
## Notes
* TorqueLab will work without any code changes but some features might requires some changes in the code. Those would be disabled unless you make the needed changes.
* For current Pre-Alpha version, I have included my personnal helpers scripts since some are use in TorqueLab. I will make sure to embed those used inside TorqueLab in future release.

## Known major issues
* The Clone on object drag function is not working since it require some code change to work. I will examine to see if I can get it to work without code changes. If not, I will post the code changes needed. 
