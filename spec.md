# RC-600 Patch Editor Application Specification

**Platform:** .NET MAUI (C# & XAML) targeting Windows and macOS

---

## 1. Overview and Goals

* **Cross-platform desktop app** built with .NET MAUI for Windows & macOS.
* **Manage 99 memory patches** , fully exposing every RC-600 parameter:
* **Tracks (1–6):** playback mode, one-shot, reverse, levels, pan, start/stop/fade, overdub mode, loop sync, measure count, input routing.
* **Input FX & Track FX:** 16 effect slots (4 banks × 4) with full parameter editing, routing targets, and switch modes.
* **Rhythm:** pattern selection (Genre, Pattern, Variation), drum kit, tempo, time signature, volume, start/stop behavior, intro/ending/fill settings, variation‑change timing, plus MIDI‑importable user patterns.
* **Controls:** assign functions to the 9 onboard footswitches (Modes 1–3), 2 external switches, and 2 expression pedals; advanced assign table for up to 16 custom mappings.
* **Intuitive GUI:** logically grouped tabs/panels (Tracks, Rhythm, Effects, Controls), clear labels, inline tooltips, expandable sections.
* **Export/Import via filesystem:** mirror RC-600 USB layout (`DATA/DATA`, `WAVE`), read/write `MEMORY###A.RC0` & `MEMORY###B.RC0`, `RHYTHM.RC0`, optional system files.
* **Device integration:** auto-detect RC-600 drive for one-click export; future MIDI SysEx support.

---

## 2. Data Model

```csharp
public class MemoryPatch
{
    public string Name { get; set; }           // max 12 chars
    public double Tempo { get; set; }
    public PlayModeEnum PlayMode { get; set; }
    public SingleModeSwitchEnum SingleModeSwitch { get; set; }
    public bool LoopSync { get; set; }
    public Track[] Tracks { get; set; } = new Track[6];
    public EffectBanks Effects { get; set; }
    public RhythmSettings Rhythm { get; set; }
    public ControlAssignments Controls { get; set; }
    public List<AssignSlot> Assigns { get; set; }
}
```

```csharp
public class Track
{
    public bool OneShot { get; set; }
    public bool Reverse { get; set; }
    public int Level { get; set; }          // 0–200
    public int Pan { get; set; }            // -50..50
    public StartModeEnum StartMode { get; set; }
    public int FadeInMeasures { get; set; }
    public StopModeEnum StopMode { get; set; }
    public int FadeOutMeasures { get; set; }
    public OverdubModeEnum OverdubMode { get; set; }
    public TempoSyncEnum TempoSyncMode { get; set; }
    public int MeasureCount { get; set; }
    public InputRouting InputRouting { get; set; }
    public OutputAssignEnum? OutputAssign { get; set; }
}
```

```csharp
public class EffectSlot
{
    public enum Category { InputFX, TrackFX }
    public Category SlotCategory { get; set; }
    public string Type { get; set; }
    public bool EnabledByDefault { get; set; }
    public SwitchModeEnum SwitchMode { get; set; }
    public string Target { get; set; }
    public Dictionary<string, double> Parameters { get; set; }
}

public class RhythmSettings
{
    public bool Enabled { get; set; }
    public string Genre { get; set; }
    public string Pattern { get; set; }
    public char Variation { get; set; }      // 'A','B','C','D'
    public string Kit { get; set; }
    public string Beat { get; set; }
    public double Tempo { get; set; }
    public double Volume { get; set; }
    public StartModeEnum StartMode { get; set; }
    public StopModeEnum StopMode { get; set; }
    public bool IntroOnRec { get; set; }
    public bool IntroOnPlay { get; set; }
    public bool Ending { get; set; }
    public bool FillIn { get; set; }
    public VariationTimingEnum VariationChangeTiming { get; set; }
}

public class ControlAssignments
{
    public Dictionary<int, PedalModeAssignment> Pedals { get; set; } // key=1..9
    public string ExternalSwitch1 { get; set; }
    public string ExternalSwitch2 { get; set; }
    public string Expression1 { get; set; }
    public string Expression2 { get; set; }
}

public class AssignSlot
{
    public string Source { get; set; }
    public string Target { get; set; }
    public ActionModeEnum ActionMode { get; set; }
    public double? RangeMin { get; set; }
    public double? RangeMax { get; set; }
}
```

---

## 3. File Format & Export

* **Directory structure**
  ```
  DATA/
    DATA/
      MEMORY001A.RC0 … MEMORY099B.RC0
      RHYTHM.RC0
      SYSTEM1.RC0
      SYSTEM2.RC0
  WAVE/
    WAVE/
      <memory>_<track>/
  ```
* **Parsing & emission**
  * Read tags: `NAME`, `TRACK1`…`TRACK6`, `RHYTHM`, `CTL`, `ASSIGN`.
  * Preserve unknown tags via template merging.
  * Export by patch number: single, range, or all.
* **MIDI import**
  * Accept Standard MIDI files.
  * Convert into RC-600 user-rhythm format.
  * Inject into `RHYTHM.RC0`.

---

## 4. User Interface

* **Patch List** sidebar: select, rename, duplicate, reorder patches.
* **Tabs** : Tracks | Rhythm | Effects | Controls | (System).

### Tracks Tab

* Grid overview of all six tracks.
* Expandable panels for each track’s detailed settings.

### Rhythm Tab

* **Pattern & Tempo** panel.
* **Behavior** panel (start/stop modes).
* **Intro/Ending** panel (intro, ending, fill‑in).
* **MIDI Import** button.

### Effects Tab

* Collapsible  **Banks A–D** .
* Each slot editor with:
  * Type dropdown
  * On/off toggle
  * Parameter controls
  * Routing target
  * Switch mode selector

### Controls Tab

* Pedal‑modes matrix (1–9 × Modes 1–3).
* External & expression control assignments.
* Advanced **Assign** table for custom mappings.

 **Validation** : enforce RC-600 rules (e.g. loopSync requires measure counts, no duplicate pedal assignments).

---

## 5. Optional Device Integration

* Auto‑detect RC-600 drive in USB storage mode for seamless export.
* Future: MIDI SysEx bulk dump/transfer for live editing.

---

## 6. Development Considerations

* Map `.RC0` tags to parameters via sample‑file analysis.
* Reference Owner’s Manual & Parameter Guide for definitions, ranges, behaviors.
* Community testing for edge cases and format quirks.
* **Primary Platform** : **.NET MAUI** (C# & XAML) on Windows & macOS for native UI and full filesystem/USB access.

---

## 7. Diagrams & Mockups

* File structure tree
* Data‑model class diagram
* UI layout mockups

---
