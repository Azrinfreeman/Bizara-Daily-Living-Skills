# Documentation review and runtime checklist

Reviewed against commit `6389e22f9a10aa3be618cfe4fa7d9f5152934f41`.

## Scope and evidence

This change adds portfolio documentation only. Gameplay scripts, scenes, assets, packages, and project settings are unchanged.

- Read the project version, package manifest, and five enabled build scenes.
- Reviewed activity progression, drag/drop, completion storage, dashboard export, media selection, language preferences, and the parent access question.
- Checked documentation links against tracked files and checked the patch for whitespace errors.

Unity compilation, Play Mode, platform builds, and device permissions were not tested during this documentation review. These notes are not a publication or full-history security audit.

## Source observations to verify

- **Progress keys:** `PaparData` reads `MakanMinum`, `ToiletTime`, `Modul3`, and `Modul4`. The serialized `namaModul` values are `MakanMinum` in Modul1, `ToiletTime` in Modul2, and `SayaBersih` in both Modul3 and Modul4. The last two modules can therefore write to the same record while their dashboard entries read different keys. Reconcile these names before treating the dashboard as accurate for all four modules.
- **Menu navigation:** `ModulManager.KembaliKeMenuUtama()` targets the enabled `ModulUtama` scene. `ModuleLoader.LoadMainMenu()` instead targets `MainMenu`, which is absent from the enabled scene list. Check which buttons invoke each method.
- **Sequence setup:** The shuffle loop in `ModulManager.RawakkanKad()` searches for a nonascending arrangement. A one-card array or cards with identical X coordinates cannot satisfy that condition. Verify the assigned card arrays and positions.
- **Labels and translations:** Some later scenes retain eating-related serialized text. Check displayed labels in both languages, including any runtime translation overrides.
- **Custom media:** Selection stores file paths rather than importing permanent copies. Verify gallery permissions, missing files, image previews, video preparation, and playback on the target device.
- **Local records:** Profile and completion data use `PlayerPrefs`. The parent gate is an arithmetic question, not a password or protected account. Export is CSV despite the method name `EksportKeExcel`.

## Suggested manual checks

1. Open the project in Unity 6000.4.8f1 and resolve dependencies; inspect compilation and console messages.
2. Start at ModulUtama, open each module, and complete selection, drag, sequencing, reward, replay, and return-to-menu flows.
3. Confirm each module updates only its intended dashboard row, including completion counts and timestamps after restarting.
4. Switch English/Malay, edit the learner profile, and check saved preferences after restarting.
5. Exercise the parent question, custom image/video selection, volume controls, and CSV export on the intended platform. Use test learner data and inspect the exported file.

The source observations above remain unfixed in this documentation-only change.
