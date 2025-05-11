Thanks, David. I’ll put together a C# command-line tool specification that reads RC-600 patch data from a directory of files retrieved via USB. The tool will support dumping either summary info for all patches or full details for a specified patch, with output in plain text.

I’ll get started and let you know once the spec is ready.

# RC-600 Patch Data Dump Tool – Technical Specification

## Overview

This specification describes a Windows command-line application (written in C#) designed to read **Boss RC-600 Loop Station** patch data from a backup directory (as retrieved from the device’s USB storage). The tool provides  **two main modes of operation** : a **Top-Level Dump** for patch summaries, and a **Full Patch Dump** for detailed patch information. It is a read-only utility that outputs data to the console in plain text. No filtering of patches or exporting to files is supported in this version (the output is intended for on-screen viewing or redirection via shell). Future versions may extend functionality, but this spec focuses on the initial requirements.

## Command-Line Usage

**Syntax:**

```bash
RC600Dump.exe [options] <PatchDirectory> [<MemoryNumber>[A|B]]
```

* `<PatchDirectory>` – Path to the directory containing the RC-600 patch data files (e.g. the “Roland/Data” folder from the RC-600’s USB drive).
* `<PatchNumber>` – *Optional.* The specific patch number (memory slot) to dump in full. If omitted, the tool will list a summary of **all** patches.

**Options:**

* `-p, --patch <PatchNumber>` – An explicit flag alternative to specifying `<PatchNumber>` positionally. For example, `-p 42` indicates the tool should output the full details of patch number 42. (If both a positional patch number and `-p` are provided, the flag takes precedence. In normal use, one method is used at a time.)
* `-h, --help` – Displays usage information. If the user invokes the tool with no arguments or with `--help`, it will print instructions and exit.

**Usage Examples:**

* **Top-Level Summary for All Patches:**

  ```bash
  # List summary of all patches in the backup directory
  RC600Dump.exe "D:\RC600\Backup\Roland\Data"
  ```

  (This will run the tool in *Top-Level Dump* mode, reading all patch files under the specified directory and printing a one-line summary for each patch.)
* **Full Dump for a Single Patch:**

  ```bash
  # Dump full details of patch number 42
  RC600Dump.exe "D:\RC600\Backup\Roland\Data" --patch 42
  ```

  or equivalently:

  ```bash
  RC600Dump.exe --patch 42 "D:\RC600\Backup\Roland\Data"
  ```

  (This invokes *Full Patch Dump* mode for patch 42, printing all available data for that patch.)

In both cases, the output is sent to standard output. Users can redirect the output to a file if needed using shell redirection (e.g., `> output.txt`), since the tool itself does not directly generate an output file.

## Directory and File Expectations

The backup produced by the RC‑600 (as tested in  **RolExample.zip** ) always contains  **one XML file per memory variation** .

```
<backup root>\
└── DATA\
    └── DATA\
        ├── EDITOR.RCE           (XML – notes only; ignored)
        ├── MEMORY001A.RC0       (Memory 1, Variation A)
        ├── MEMORY001B.RC0       (Memory 1, Variation B)
        ├── MEMORY002A.RC0
        ├── MEMORY002B.RC0
        └── … up to MEMORY200B.RC0
```

* There is  **no monolithic memory file** ; the tool must parse the individual `.RC0` files.
* The three‑digit number (`001`‑`200`) is the  **Memory number** .  The trailing letter `A`/`B` is the  **Variation** .
  * **Top‑Level mode** prints  **one line per memory** , using the `A` file as the representative (variation data are usually very similar).  A future flag (`--include-variations`) can surface both.
  * **Full‑Dump mode** accepts either a bare number (`42` → dumps `MEMORY042A.RC0`) or an explicit variation (`42B`).

### File format (.RC0)

`.RC0` files are **UTF‑8 text that looks like XML but breaks several XML rules** (unescaped characters, duplicate tags, etc.). **Use a tolerant text parser – *****not***** a strict XML library.** The structure is still predictably nested:

```pseudo
<database name="RC-600" revision="0">
  <mem id="0">
     <NAME> … </NAME>          ; Patch name characters
     <PLAY> … </PLAY>          ; Patch‑level behaviour flags
     <TRACK1> … </TRACK1>      ; Tracks 1‑6 (TRACK2 … TRACK6)
     …                         ; Other sections (RHYTHM, ASSIGN01 …) that follow the same pattern
  </mem>
</database>
```

#### Tag‑Index Scheme

Inside *every* parent element the child tags are  **indexes** , not meaningful names. The order is fixed:

| Index  | Tag  | ASCII  | Notes                              |
| ------ | ---- | ------ | ---------------------------------- |
| 1‑26  | A‑Z | 65‑90 | Alphabetical slots                 |
| 27‑36 | 0‑9 | 48‑57 | After Z, digits continue the count |
| 37     | #    | 35     | Highest index seen so far          |

So `<U>` is slot 21 (because U is the 21st letter). To decode the patch you map **parent section + slot number** to a parameter defined in Roland’s MIDI/parameter guide.

#### Key field mapping required for the summary dump

| Section    | Slot(Tag) | Parameter                | Value Mapping                   | Example                      |
| ---------- | --------- | ------------------------ | ------------------------------- | ---------------------------- |
| `NAME`   | A‑L      | Patch name (ASCII codes) | 0‑255 → char                  | `<A>66</A>`→**"B"** |
| `TRACK1` | U         | Tempo (BPM×10)          | `1200`→**120 BPM**    | `<U>1200</U>`              |
| `PLAY`   | C         | PlayMode                 | `0`=MULTI,`1`=SINGLE        | `<C>0</C>`                 |
| `PLAY`   | D         | LoopSync                 | `0`=OFF,`1`=ON              | `<D>1</D>`                 |
| `PLAY`   | E         | SingleModeSwitch         | `0`=Loop‑End,`1`=Immediate | `<E>0</E>`                 |

*(These offsets were derived from the provided backup; confirm against future firmware docs.)*

Parsing steps:

1. **Tokenizer** – Stream through the file, recognising `<TAG>` … `</TAG>` pairs and collecting the text content as an integer.
2. **Section dispatch** – When `<PLAY>` starts, push a new context; each subsequent closing tag pops the context.
3. **Index lookup** – Convert the tag to an index (A‑Z=1‑26, 0‑9=27‑36, #=37) and store it in a per‑section array/dictionary.
4. **Post‑process** – After the entire file is read, translate the raw integers to meaningful values using the table above (or a wider parameter guide for full‑dump mode).

`EDITOR.RCE` remains outside the scope (notes only). All other content in the spec is unchanged.

### Robust enumeration

The tool enumerates every file that matches `MEMORY???[AB].RC0`, sorts by Memory number, and treats missing variations gracefully.

## Modes of Operation and Output Format

The tool has two output modes corresponding to the two main use cases:

### 1. Top-Level Dump (Patch Summary Mode)

In this mode, the application outputs a **summary line for each patch** found in the input directory (or in the memory file). This is the default behavior when no specific patch is requested. The output is formatted for readability in plain text, one patch per line, showing key attributes:  **Name, Tempo, PlayMode, LoopSync, SingleModeSwitch** .

* **Content:** For each patch, the summary includes:

  * **Patch Number/ID** – The memory slot number, for reference.
  * **Name** – The patch’s name (as stored on the RC-600).
  * **Tempo** – The tempo setting of the patch (in BPM).
  * **PlayMode** – The play mode, e.g. “MULTI” vs “SINGLE”, indicating if multiple tracks can play simultaneously or only one at a time.
  * **LoopSync** – The loop synchronization setting (e.g. ON or OFF) for that patch, which determines if track loops are tied to a master tempo/measure.
  * **SingleModeSwitch** – The setting for single-track mode switching behavior. (This likely reflects whether switching from one track to another in single mode is immediate or at loop end, as configured in the patch. It’s represented as a boolean or specific mode value in the data; the tool will output an intuitive value like ON/OFF or IMMEDIATE/LOOP_END as appropriate.)
* **Format:** Each patch’s summary is printed on a single line in a consistent format. For example:

  ```plaintext
  Patch 01: Name="Default Memory", Tempo=120 BPM, PlayMode=MULTI, LoopSync=ON, SingleModeSwitch=OFF
  Patch 02: Name="MySong", Tempo=90 BPM, PlayMode=SINGLE, LoopSync=OFF, SingleModeSwitch=ON
  Patch 03: Name="AmbientLoop", Tempo=75 BPM, PlayMode=MULTI, LoopSync=ON, SingleModeSwitch=OFF
  ... (and so on for all patches found)
  ```

  In the above example format:

  * The patch Name is enclosed in quotes if it contains spaces or special characters (for clarity).
  * Tempo is displayed with a unit (BPM).
  * Boolean or enumerated settings are shown as uppercase identifiers (e.g. ON/OFF, MULTI/SINGLE) matching the device’s terminology.

  The tool will ensure alignment and consistency in formatting. (For instance, it might pad the patch number to two digits or include leading zeros as shown, but this is cosmetic.) The primary goal is that each line clearly identifies the patch and its summary settings, making it easy for the user to scan all patches at a glance.
* **Order:** Patches are listed in numerical order by patch number (typically 1 up to the maximum slots, skipping any missing ones). If the device supports setlists or banks, this tool treats the entire memory list sequentially. In a scenario where the patch files are sparse (e.g., only certain patches exist), the tool may list those it finds in numeric order and omit missing numbers, or it may include placeholders for missing patches (see Error Handling). By default, it will list only actual patch data found.

### 2. Full Patch Dump (Detailed Patch Mode)

In this mode, the application outputs  **all available data for a single specified patch** . This is triggered by providing a patch number (via argument or `--patch` option). The output is a verbose, multiline dump of every parameter and setting stored in the patch’s data file, presented in a human-readable form.

* **Content:** The full dump includes **every parameter** that the RC-600 patch file contains. This will encompass far more than the five summary fields, for example:

  * Patch metadata: Name, Tempo, Rhythm settings, etc.
  * Track settings for each track (the RC-600 has multiple tracks, e.g. 6 tracks) – such as track levels, pan, one-shot status, FX on/off per track, etc.
  * Loop settings: Play Mode, Loop Sync, Tempo Sync, quantization, measure lengths, Single Mode behavior, etc.
  * Effects and Assignments: Any patch-specific effect parameter values or controller assignments (if the patch data includes these—likely yes, the RC-600 patch includes which effects are on, their levels, etc.).
  * MIDI/Control settings: Any MIDI channel or control preferences saved in the patch.
  * Other flags and values: Basically, if the patch’s data structure contains it, the tool should display it. This results in a comprehensive list that might be dozens of lines long per patch.
* **Organization:** To make the output readable, the tool will group related parameters together and use indentation or headers for sections. For example, a possible structure might look like:

  ```plaintext
  Patch 42 - "MySongPatch" (Full Dump):
  Name: MySongPatch
  Tempo: 90 BPM
  Rhythm: ON, Pattern 10, Kit = Rock 1
  PlayMode: SINGLE
  LoopSync: OFF
  SingleModeSwitch (Track Change Mode): ON (Immediate)
  Tempo Sync: OFF
  Quantize: ON
  <...global patch settings continued...>

  Track 1 Settings:
    Track Name: (none)
    Track Status: On
    One-Shot: OFF
    Reverse: OFF
    Input FX: ON  (FX1: Phaser [enabled],  FX2: Delay [disabled])
    Track FX: OFF
    Volume: 100
    Pan: Center
    etc...

  Track 2 Settings:
    ... (similar structure for track 2) ...

  ... (Tracks 3-6 Settings) ...

  Assignments:
    Expression Pedal 1 -> Track1 Volume
    Footswitch 2 (CTL2) -> Play/Stop All
    MIDI CC 80 -> FX1 Toggle
    ... (etc., if applicable) ...

  Other Settings:
    Single Change Mode: LOOP END
    Start/Stop Mode: IMMEDIATE
    Fade Time: 3 sec
    etc...
  ```

  *The above is an illustrative example.* The actual parameters and grouping should reflect the RC-600’s feature set. For instance, “Rhythm” settings (if the RC-600 has built-in rhythms or drum patterns per memory) would be included, as well as any other memory-specific settings found in the data. The tool’s output formatting should use clear labels (matching or closely resembling the names used in the RC-600’s manuals) and values in a human-friendly form (translating any numeric codes in the data to meaningful text when possible).
* **Format Details:** Each parameter is listed on its own line as `ParameterName: Value`. Indentation is used for sub-components (like track-specific parameters under a track heading). Section headers like `"Track 1 Settings:"` or `"Assignments:"` are used to break up the output logically. If the patch has any list of items (like a list of assignments or effects), each is either listed on separate lines (possibly bulleted or prefixed with a `-` for clarity) or in a sub-section. The key is to ensure the output is  **well-structured and easy to read** , despite being comprehensive. Users should be able to find a specific setting by scanning the output, thanks to the clear labels and grouping.
* **Order:** There is a sensible ordering of parameters (for example, start with general patch settings, then per-track settings in numerical order, then any global assignments or miscellaneous settings). This ordering can mirror the layout of the device’s menus or parameter guide for familiarity. The tool will output in a fixed order, not sorted alphabetically or arbitrarily, to maintain logical grouping.
* **Completeness:** Every value present in the patch file is output. If some values are not understood by the tool (for instance, if a proprietary binary contains unknown bytes), the tool can output them in a raw form (e.g. hex dump or a generic "UnknownFieldX: ") or skip them. Ideally, the spec assumes we have knowledge of all fields, but it should be robust to at least not crash on unexpected data. (For known RC-600 parameters, everything should be accounted for.)

**Output Destination:** In both modes, output is printed to standard output (console). No GUI or interactive display is involved. The text can be scrolled in the console or captured by redirecting output. The tool should ensure no unprintable binary data is sent to the console – all binary data from files is translated to readable text. Any binary content that is not meant to be human-readable (e.g., audio or large data blocks, if any in the patch file) is not in scope – note that loop audio recordings are stored separately (in the WAVE folder) and are not handled by this tool at all.

## Error Handling and Robustness

The application will handle error cases gracefully, providing clear messages to the user and appropriate exit codes. Below are expected error scenarios and how the tool responds:

* **Directory Not Found / Invalid Path:** If the `<PatchDirectory>` provided does not exist or cannot be opened, the tool will output an error message such as:

  ```plaintext
  Error: Directory "D:\RC600\Backup\Roland\Data" not found or inaccessible.
  ```

  It will then terminate with a non-zero exit code (e.g., `1`). The user should verify the path and try again. Use of `--help` will remind of proper usage.
* **No Patch Data Files Found:** If the directory is valid but no recognizable patch files are found within, the tool will inform the user:

  ```plaintext
  Error: No RC-600 patch data files were found in "D:\RC600\Backup\Roland\Data".
  ```

  This might happen if the wrong directory was pointed to (e.g., a parent directory instead of the `Data` subfolder), or if the backup is incomplete. The tool in this case does not produce a dump; it exits with an error status. (If desired, a future enhancement could allow it to search subdirectories, but initial scope assumes the directory is correct.)
* **Missing Patch File (in Full Dump Mode):** If a specific patch number is requested but the corresponding patch data cannot be located, the tool will output an error message like:

  ```plaintext
  Error: Patch data for patch 42 was not found in the directory.
  ```

  and exit with an error. It will not attempt to do anything further. This scenario could occur if, say, the user requests a patch number beyond the device’s range or if individual patch files are expected but that number’s file is absent. (If using a combined memory file, this error would instead be “Patch 42 is out of range” if the number exceeds the maximum patch count defined by the device.)
* **Corrupt or Unreadable Patch File:** If a patch file is present but cannot be read (file I/O error) or its contents are not in the expected format (e.g., wrong size, invalid data), the tool will handle it as follows:

  * **I/O Error (file locked, etc.):** An error is reported:

    ```plaintext
    Error: Could not read patch file "PATCH042.RC6" (access denied or file corrupt).
    ```

    It will skip processing that file (in top-level mode, skip that patch and continue with others, but note the error; in single patch mode, just error and exit).
  * **Format Error (Parsing Failure):** If the file is read but the content doesn’t match expected structure (for instance, a SysEx file missing an end byte, or a binary with incorrect length), the tool will either:

    * Try to recover (for example, if possible, skip unknown sections and still extract what it can) and warn the user in the output that some data might be incomplete. Warnings might look like:

      ```plaintext
      Warning: Patch 42 data appears to be incomplete or in an unknown format. Some fields may be missing.
      ```

      Then it would output whatever fields it could parse.
    * Or, if recovery isn’t feasible, output an error similar to “Patch 42 data is corrupt or unsupported format” and skip/abort.

  In all cases of errors in multiple-file scenario, the tool isolates the issue to that patch file and does not let it crash the entire run. The summary mode will continue listing other patches if one patch file has an issue (and perhaps list the problematic patch as “[Error]” or skip it with a message). The full-dump mode will stop since the requested patch can’t be fully read.
* **Invalid Arguments/Usage:** If the user’s command-line invocation is not recognized (e.g., missing directory argument, or an unrecognized flag), the tool will print a brief **Usage** help (similar to what `--help` produces) and exit with code `1`. This ensures the user is informed how to properly use the tool.
* **Internal Exceptions:** Any unexpected exceptions (like runtime errors) should be caught and turned into user-friendly messages rather than raw stack traces. The tool might output “Unexpected error occurred” along with context, and exit with an error code. This is more of a development consideration; the spec’s goal is that under normal anticipated errors, the messages are handled as above.

**Logging and Debugging:** In this simple utility, verbose logging to a file is not required, but for debugging purposes a verbose mode could be toggled (not exposed to user in initial version) to log parsing details. For the user-facing side, only the pertinent error/warning messages described are shown. All errors are written to standard error stream (stderr) so that they can be distinguished from normal output if needed (and to not interfere with output redirection of the normal data).

## Modularity and Future Enhancements

While the current scope is limited to console output of patch data, the design should be modular to allow future improvements. Here are suggestions to ensure extensibility:

* **Separation of Concerns:** The tool’s architecture will separate the core **data parsing logic** from the **output formatting** and **user interface (CLI)** logic. For example:

  * A module or class (e.g. `PatchParser`) will handle opening files and interpreting bytes/SysEx into a structured in-memory representation (e.g. a `Patch` object with properties like Name, Tempo, etc., and perhaps sub-objects for tracks). This parser can be extended if the file format changes (like a firmware update adding new parameters) without affecting how output is generated.
  * Another module (e.g. `PatchPrinter` or the main program) will handle converting that `Patch` object (or list of patches) into text for the console. If we later want JSON or XML output, we can add a different formatter without changing the parser.
* **Output to JSON or Other Formats:** A future version may support an option (e.g. `--json`) to output the patch information in a machine-readable format (JSON, XML, CSV, etc.). Thanks to the modular design, this would involve adding a new output formatter that takes the internal data structures and serializes them to JSON. This could be useful for users who want to import patch data into other applications or scripts. The command-line interface could then allow something like:

  ```bash
  RC600Dump.exe --json --patch 42 "D:\RC600\Backup\Roland\Data"
  ```

  to get a JSON object of patch 42’s data. Similarly, `--output file.json` could be added to directly save to a file. These enhancements would not require a rewrite of parsing logic—just extensions.
* **GUI Front-End:** The core logic (parsing and data model) could be packaged in a library or a class that a GUI application could call into. For example, a WPF or WinForms application could use the same code to load patches and then present them in a user-friendly interface (perhaps a list view for patches and a detail panel for the selected patch). By keeping the CLI as a thin layer over the core, a GUI front-end can be built without duplicating code. The spec does not require building the GUI now, but anticipates this possibility. In practice, the project could be organized with a library (DLL) for the core functionality and two front-ends: one CLI, one GUI.
* **Filtering and Search:** Although filtering (e.g. show only patches that meet certain criteria, or search by name) is not in scope initially, the design should not preclude it. For instance, the list of Patch objects in memory could be easily filtered by name or tempo, etc., before printing. In the future, command-line flags like `--name "Ballad"` could be added to only dump patches whose name matches “Ballad”, or `--tempo >120` to list patches above a certain tempo. Planning the data structures with this in mind (storing all patches in a collection, allowing iterative filtering) will make such enhancements easier.
* **Writing/Editing Capabilities:** This specification is read-only, but a possible extension is the ability to modify patch data or create new patch files. If that were to be added, the parsing logic should be complemented with serialization logic. Ensuring the code is organized (e.g. a class per parameter group) will simplify implementing a future `--set` or GUI editing feature. For now, this is just noted as a potential extension, and the file format knowledge used for reading would be reused for writing if ever needed.
* **Multi-Device Support:** Although designed for the RC-600, a similar structure might be used by related Boss devices (RC-505mkII, etc.). A well-structured parser could be adapted or configured for different devices’ memory formats. If future development wanted to generalize the tool, it could be structured such that device-specific details (like number of tracks, parameter set, file naming) are isolated (perhaps via configuration files or subclasses). This way, the core program might eventually handle “RC-600” and others via a flag or auto-detection. This is beyond current requirements but a consideration for modular design.

In summary, the tool’s implementation should emphasize  **clean separation and clarity** , making it easier to bolt on features like JSON output or a GUI without major rework. The current CLI tool delivers the immediate functionality needed (dumps of RC-600 patch settings), and these modularity choices ensure it can grow or integrate into larger workflows over time.

## Conclusion

This technical specification outlines a console-based C# application to extract and display Boss RC-600 patch data. The application will operate in two modes (summary of all patches, or detailed single-patch output) and handle the RC-600’s data directory structure. Clear text output is prioritized for readability, and robust error handling is included to guide the user in case of issues. While the initial feature set is narrow (no filtering or file export beyond console output), the design recommendations ensure that the tool can be extended with additional output formats or interfaces in the future. Following this spec will result in a useful utility for RC-600 users to inspect their loop station’s memory settings quickly from the command line, serving as both a diagnostic tool and a basis for potential future enhancements.
