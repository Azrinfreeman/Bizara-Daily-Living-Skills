# Bizara Life Skill

A Unity learning project that turns daily routines into short selection, drag-and-drop, and sequencing activities. A parent dashboard provides a local learner profile, completion records, and options to personalize learning media.

## Learning modules

| Module | Routine | Scene |
| --- | --- | --- |
| Makan & Minum | Eating and drinking | `Modul1` |
| Toilet Time | Using the toilet | `Modul2` |
| Saya Bersih | Handwashing and cleanliness | `Modul3` |
| Saya Kemas | Preparing belongings and getting ready | `Modul4` |

The module controller progresses from a selection activity to dragging an object, arranging steps in order, and a reward panel. Learners can repeat a module or return to the main menu.

## Features in the source

- Pointer-driven dragging and left-to-right sequence checking.
- English/Malay translation support with a saved language preference.
- A local learner name, age, and avatar, stored through Unity `PlayerPrefs`.
- Completion counts and last-completed timestamps, with a parent dashboard and semicolon-delimited CSV export.
- Custom images and videos selected through NativeGallery, with saved file paths and video playback support.
- Persistent background music and volume controls.
- A simple addition question before opening the parent dashboard. This is a child-facing access gate, not account authentication.

The project is a prototype. In particular, the scene record names and dashboard lookup names need alignment before relying on every module's progress display. See [validation notes](docs/VALIDATION.md) for source observations and the remaining runtime checks.

## Open the project

1. Clone the complete repository and open its root folder through Unity Hub.
2. Use **Unity 6000.4.8f1**, as recorded in [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt).
3. Allow Unity to import assets and resolve [package dependencies](Packages/manifest.json). The project uses URP 17.4.0, Input System 1.19.0, and uGUI 2.0.0. Its Unity MCP package is fetched from Git, so dependency resolution needs network access.
4. Open [ModulUtama](Assets/Scenes/ModulUtama.unity) and enter Play Mode to begin checking the menu and modules.

The enabled scene order in [EditorBuildSettings.asset](ProjectSettings/EditorBuildSettings.asset) is `ModulUtama`, `Modul1`, `Modul2`, `Modul3`, and `Modul4`. Platform builds, media permissions, and the complete learning flow still need testing in Unity and on the intended device.

## Code guide

| Responsibility | Source |
| --- | --- |
| Activity progression, sequencing, rewards, and replay | [ModulManager.cs](Assets/ScriptLoader/ModulManager.cs) |
| Drag movement and successful drops | [SuduDrag.cs](Assets/ScriptLoader/SuduDrag.cs), [PingganDrop.cs](Assets/ScriptLoader/PingganDrop.cs) |
| Completion storage | [PengurusData.cs](Assets/ScriptLoader/PengurusData.cs) |
| Profile, dashboard records, and CSV export | [PaparData.cs](Assets/ScriptLoader/PaparData.cs) |
| Media selection and module video loading | [PengurusMediaDashboard.cs](Assets/ScriptLoader/PengurusMediaDashboard.cs), [MuatVideoModul.cs](Assets/ScriptLoader/MuatVideoModul.cs) |
| Language preferences and translations | [PengurusBahasa.cs](Assets/ScriptLoader/PengurusBahasa.cs) |
| Parent-dashboard access question | [KunciIbuBapa.cs](Assets/ScriptLoader/KunciIbuBapa.cs) |

Learner data is stored locally on the device. CSV export writes to `Application.persistentDataPath`; it does not generate an Excel workbook. Custom media retains the selected file path, so moving or deleting that file can prevent later playback.
