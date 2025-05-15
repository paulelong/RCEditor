## Memory Settings (LOOP)

### TRACK 1‑6

| Tag   | Parameter               | Values (default in**bold**)        | Explanation                                                                                                                                                                                |
| ----- | ----------------------- | ---------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| A     | REVERSE                 | **OFF**, ON                        | Conventional playback (OFF) or reverse playback (ON).*Overdubbing isn’t available once recording finishes if ON.*                                                                       |
| B     | 1SHOT                   | **OFF**, ON                        | OFF = loop playback. ON = one‑shot playback; stops at the phrase end and can be retriggered; overdubbing disabled. If you don’t want tempo sync, set**TEMPO SYNC SW**OFF.        |
| C     | PAN                     | L50 … CENTER … R50                     | Stereo position of the track.                                                                                                                                                              |
| D     | PLAY LEVEL              | 0 …**100**… 200                  | Playback level of the track.                                                                                                                                                               |
| E     | START MODE              | **IMMEDIATE**, FADE                | FADE starts with a fade‑in; length set in**FADE TIME**.                                                                                                                             |
| F     | STOP MODE               | **IMMEDIATE**, FADE, LOOP          | IMMEDIATE = stop instantly; FADE = fade‑out then stop (time via**FADE TIME**); LOOP = stop at loop end.                                                                             |
| G     | DUB MODE                | **OVERDUB**, REPLACE1, REPLACE2    | OVERDUB = layer new audio; REPLACE1 = overwrite while playing; REPLACE2 = overwrite silently.                                                                                              |
| H     | FX                      | **OFF**, ON                        | Enable/disable input or track FX.                                                                                                                                                          |
| I     | PLAY MODE               | **MULTI**, SINGLE                  | MULTI = play all tracks; SINGLE = only one track plays; the current track stops when another                                                                                               |
| J     | MEASURE                 | AUTO, FREE, 1 – N                     | Number of measures for the track.**AUTO**follows the first recorded track;**FREE**sets length automatically; manual value fixes it. Available only if**LOOP SYNC**is ON. |
| K     | LOOP SYNC SW            | **OFF**, ON                        | Turns loop sync on/off.                                                                                                                                                                    |
| L     | LOOP SYNC MODE          | IMMEDIATE,**MEASURE**, LOOP LENGTH | How syncing is performed.                                                                                                                                                                  |
| M     | TEMPO SYNC SW           | **OFF**, ON                        | Track plays at its own tempo or at the memory tempo.                                                                                                                                       |
| N     | TEMPO SYNC MODE         | PITCH,**XFADE**                    | PITCH changes pitch with tempo; XFADE keeps pitch.                                                                                                                                         |
| O     | TEMPO SYNC SPEED       | HALF,**NORMAL**, DOUBLE            | Playback speed multiplier.                                                                                                                                                                 |
| P     | BOUNCE IN               | **OFF**, ON                        | Whether playback from other tracks is recorded during overdub.                                                                                                                             |
| Q,R,S | INPUT (MIC/INST/RHYTHM) | **OFF**, ON per source             | Route each input to the track during recording.                                                                                                                                            |

### REC

| Parameter     | Values (default in**bold**) | Explanation                                                  |
| ------------- | --------------------------------- | ------------------------------------------------------------ |
| REC ACTION    | **REC→DUB**, REC→PLAY     | Order in which the [REC/PLAY] switch cycles.                 |
| QUANTIZE      | **OFF**, MEASURE            | Corrects timing to measure grid under loop‑sync conditions. |
| AUTO REC SW   | **OFF**, ON                 | Arms auto‑recording; starts when input exceeds threshold.   |
| AUTO REC SENS | 1 …**50**… 100            | Sensitivity for auto recording.                              |
| BOUNCE SW     | **OFF**, ON                 | Enables bounce recording.                                    |
| BOUNCE TRACK  | 1 – 6                            | Track used for bounce recording.                             |

### PLAY

| Parameter                    | Values (default in**bold**) | Explanation                                               |
| ---------------------------- | --------------------------------- | --------------------------------------------------------- |
| S.TRK CHANGE                 | **IMMEDIATE**, LOOP END     | Track‑switch timing when**PLAY MODE**= SINGLE.     |
| CURRENT TRACK                | TRACK 1 – TRACK 6              | Target track for operations/editing.                      |
| FADE TIME IN / OUT           | Note values, 1 – 64 MEAS       | Fade‑in/out length when**START/STOP MODE**= FADE.  |
| ALL START TRK / ALL STOP TRK | Per‑track**OFF**, ON       | Tracks that respond to MIDI Start/Stop messages.          |
| LOOP LENGTH                  | AUTO, 1 – 25362                | Length used by**LOOP SYNC**when MODE = LOOP LENGTH. |
| SPEED CHANGE                 | **IMMEDIATE**, LOOP END     | When the speed change takes effect.                       |
| SYNC ADJUST                  | MEASURE, BEAT                     | Sync tolerance when**SPEED CHANGE**= IMMEDIATE.     |

### INPUT FX (BANK A‑D)

| Parameter      | Values (default in**bold**) | Explanation                                                                             |
| -------------- | --------------------------------- | --------------------------------------------------------------------------------------- |
| FX A–D        | **OFF**, ON                 | Turns each input effect on/off. When**MODE**= SINGLE, only one of A–D can be on. |
| BANK           | A, B, C, D                        | Selects the FX bank to operate or edit.                                                 |
| SW             | **OFF**, ON                 | Enables/disables the selected FX bank.                                                  |
| MODE           | **SINGLE**, MULTI           | SINGLE = one effect active; MULTI = multiple effects can be active.                 |
| FX TARGET A–D | A, B, C, D                        | Chooses which effect’s intensity an expression pedal will control.                     |

### INPUT FX – FX A‑D

| Parameter | Values (default in**bold**)       | Explanation                                                 |
| --------- | --------------------------------------- | ----------------------------------------------------------- |
| SW        | **OFF**, ON                       | Toggles the specific effect on/off.                         |
| SW MODE   | **TOGGLE**, MOMENT                | TOGGLE = latch; MOMENT = only while the switch is held. |
| INSERT    | ALL, MIC1, MIC2, INST1‑L/R, INST2‑L/R | Selects which inputs are processed by the effect.           |
| FX TYPE   | Effect list                             | Selects the effect algorithm (see Input/Track FX List).    |

### TRACK FX (BANK A‑D)

| Parameter      | Values (default in**bold**) | Explanation                                                              |
| -------------- | --------------------------------- | ------------------------------------------------------------------------ |
| FX A–D        | **OFF**, ON                 | Turns each track effect on/off.                                          |
| BANK           | A, B, C, D                        | Selects the FX bank being edited.                                        |
| SW             | **OFF**, ON                 | Enables/disables the selected FX bank.                                   |
| MODE           | **SINGLE**, MULTI           | SINGLE = one effect active; MULTI = multiple effects simultaneously. |
| FX TARGET A–D | A, B, C, D                        | Chooses which effect is controlled by an expression pedal.               |

### TRACK FX – FX A‑D

| Parameter | Values (default in**bold**) | Explanation                                              |
| --------- | --------------------------------- | -------------------------------------------------------- |
| SW        | **OFF**, ON                 | Toggles the selected track effect on/off.                |
| SW MODE   | **TOGGLE**, MOMENT          | Latching or momentary operation for the effect switch.   |
| INSERT    | ALL, TRACK1‑6                    | Selects which tracks are processed by the effect.        |
| FX TYPE   | Effect list                       | Selects the effect algorithm (see Input/Track FX List). |

### RHYTHM

| Parameter  | Values (default in**bold**)      | Explanation                                       |
| ---------- | -------------------------------------- | ------------------------------------------------- |
| GENRE      | ACOUSTIC, BALLAD, BLUES, … USER       | Chooses rhythm genre.                             |
| PATTERN    | Genre‑specific list                   | Selects a rhythm pattern within the genre.        |
| VARIATION  | A, B, C, D                             | Variation of the selected pattern.                |
| KIT        | STUDIO, LIVE, LIGHT, HEAVY, … 808+909 | Drum kit used for playback.                       |
| BEAT       | 2/4 – 7/4, 5/8 – 15/8                | Time signature for rhythm.                        |
| START TRIG | LOOP START, REC END, BEFORE LOOP       | Timing for rhythm start relative to loop actions. |
| STOP TRIG  | OFF, LOOP STOP, REC END                | Timing for rhythm stop.                           |
| INTRO REC  | **OFF**, ON                      | Adds an intro before recording when ON.           |
| INTRO PLAY | **OFF**, ON                      | Adds an intro before playback when ON.            |
| ENDING     | **OFF**, ON                      | Adds an ending when rhythm stops.                 |
| FILL       | **OFF**, ON                      | Inserts fill‑ins during rhythm playback.         |
| VAR.CHANGE | MEASURE, LOOP END                      | When pattern variation changes.                   |

### NAME

| Parameter | Values               | Explanation                                                                           |
| --------- | -------------------- | ------------------------------------------------------------------------------------- |
| NAME      | Text (20 characters) | Edits the memory’s name. Use knob [4] to move cursor; knob [3] to choose characters. |

---

# System Settings (MENU)

The following tables cover global (system‑level) settings accessed via the MENU button.

## INPUT

### SETUP

| Parameter                         | Values (default in**bold**) | Explanation                                                     |
| --------------------------------- | --------------------------------- | --------------------------------------------------------------- |
| PHANTOM MIC1/2                    | **OFF**, ON                 | Enables 48 V phantom power for condenser mics.                  |
| INST1 GAIN / INST2 GAIN           | **INST**, LINE              | Sets instrument input gain to match the connected source level. |
| STEREO LINK (MIC / INST1 / INST2) | OFF,**ON**                  | When ON, L/R (or MIC1/2) share the same settings.               |
| PREFERENCE (MIC / INST1 / INST2)  | **SYSTEM**, MEMORY          | Use system‑wide settings or allow per‑memory overrides.       |

### EQ

| Parameter   | Range (default in**bold**)  | Explanation                       |
| ----------- | --------------------------------- | --------------------------------- |
| SW          | **OFF**, ON                 | Toggles EQ section.               |
| LO GAIN     | −20 dB …**0 dB**… +20 dB | Low‑shelf boost/cut.             |
| HIGH GAIN   | −20 dB …**0 dB**… +20 dB | High‑shelf boost/cut.            |
| LO MID FREQ | 20 Hz …**250 Hz**… 10 kHz | Center freq for low‑mid bell.    |
| LO MID Q    | 0.5 …**4**… 16            | Bandwidth of low‑mid filter.     |
| LO MID GAIN | −20 dB …**0 dB**… +20 dB | Gain for low‑mid band.           |
| HI MID FREQ | 20 Hz …**800 Hz**… 10 kHz | Center freq for hi‑mid bell.     |
| HI MID Q    | 0.5 …**4**… 16            | Bandwidth of hi‑mid filter.      |
| HI MID GAIN | −20 dB …**0 dB**… +20 dB | Gain for hi‑mid band.            |
| LEVEL       | −20 dB …**0 dB**… +20 dB | Overall EQ level trim.            |
| LO CUT      | **FLAT**, 20 – 800 Hz      | Low‑cut (HPF) corner frequency.  |
| HI CUT      | 630 Hz – 12.5 kHz,**FLAT** | High‑cut (LPF) corner frequency. |

### DYNAMICS

| Channel | COMP        | NS     | Explanation                                                   |
| ------- | ----------- | ------ | ------------------------------------------------------------- |
| MIC 1   | OFF, 1–100 | 0–100 | Per‑channel compressor depth and noise suppressor threshold. |
| MIC 2   | OFF, 1–100 | 0–100 | Same as above.                                                |
| INST 1  | —          | 0–100 | Noise suppressor only.                                        |
| INST 2  | —          | 0–100 | Noise suppressor only.                                        |

citeturn3file13turn3file7turn3file9

## OUTPUT

### SETUP

| Parameter                                               | Values (default in**bold**)  | Explanation                                                                    |
| ------------------------------------------------------- | ---------------------------------- | ------------------------------------------------------------------------------ |
| OUTPUT KNOB                                             | **ALL**, MASTER, PHONES, OFF | Selects which jacks the front‑panel OUTPUT LEVEL knob controls.               |
| STEREO LINK (MAIN / SUB1 / SUB2)                        | OFF,**ON**                   | Couples L/R channels for each stereo pair.                                     |
| PREFERENCE (MAIN / SUB1 / SUB2 / PHONES / RHYTHM / MFX) | **SYSTEM**, MEMORY           | Determines if jack, rhythm, and master‑FX settings are global or per‑memory. |

### MASTER FX

| Parameter | Values (default in**bold**)             | Explanation                                            |
| --------- | --------------------------------------------- | ------------------------------------------------------ |
| COMP      | **OFF**, 1–40                          | Global compressor depth applied at outputs.            |
| REVERB    | 0–40                                         | Global reverb send level.                              |
| INSERT    | MAIN‑L/R, SUB1‑L/R, SUB2‑L/R,**OFF** | Select which jacks receive the COMP/REVERB processing. |

### ROUTING (Track → Jack)

For each output pair (MAIN, SUB 1, SUB 2, PHONES) you can turn each track (1–6) ON/OFF. Use the four panel knobs to select track and toggle status. citeturn3file10

## MIXER

| Parameter     | Range (default in**bold**) | Explanation                                         |
| ------------- | -------------------------------- | --------------------------------------------------- |
| MIC 1/2 IN    | 0 …**100**… 200          | Adjusts mic input level (push = mute).              |
| INST1‑L/R IN | 0 …**100**… 200          | Instrument 1 input gain; stereo‑linked if enabled. |
| INST2‑L/R IN | 0 …**100**… 200          | Instrument 2 input gain.                            |
| MAIN‑L/R OUT | 0 …**100**… 200          | Per‑pair output trims for MAIN/Sub/Phones.         |
| SUB1‑L/R OUT | 0 …**100**… 200          |                                                     |
| SUB2‑L/R OUT | 0 …**100**… 200          |                                                     |
| LOOP OUT      | 0 …**100**… 200          | Overall loop playback level.                        |
| RHYTHM OUT    | 0 …**100**… 200          | Drum/rhythm level.                                  |
| PHONES OUT    | 0 …**100**… 200          | Headphone output level.                             |
| MASTER OUT    | 0 …**100**… 200          | Global master level (affects all outs).             |

# System Settings (MENU) – Continued

## CTL FUNC – Pedal Modes 1‑3

Default switch assignments for the nine onboard footswitches in each pedal mode.

| Pedal | Mode 1           | Mode 2         | Mode 3            | Explanation                                         |
| ----- | ----------------- | --------------- | ------------------ | --------------------------------------------------- |
| 1     | TRACK 1 REC/PLAY | TRACK 1 STOP   | TRACK 1 UNDO/REDO | Track‑level transport and undo functions per mode. |
| 2     | TRACK 2 REC/PLAY | TRACK 2 STOP   | TRACK 2 UNDO/REDO |                                                     |
| 3     | TRACK 3 REC/PLAY | TRACK 3 STOP   | TRACK 3 UNDO/REDO |                                                     |
| 4     | TRACK 4 REC/PLAY | TRACK 4 STOP   | TRACK 4 UNDO/REDO |                                                     |
| 5     | TRACK 5 REC/PLAY | TRACK 5 STOP   | TRACK 5 UNDO/REDO |                                                     |
| 6     | TRACK 6 REC/PLAY | TRACK 6 STOP   | TRACK 6 UNDO/REDO |                                                     |
| 7     | RHYTHM START/STOP | MEMORY ↑ (INC) | MEMORY ↓ (DEC)    | Global rhythm and memory navigation.                |
| 8     | ALL START/STOP    | TAP TEMPO      | TEMPO DOUBLE       | Master transport and tempo utilities.               |
| 9     | PEDAL MODE CYCLE  | MEMORY WRITE    | MIC IN MUTE        | Toggles pedal mode or mutes mic, depending on mode. |

> Full CTL FUNC list—including hundreds of alternative assignments such as **FX ON/OFF**, **TRACK FX BANK INC/DEC**, **LOOP LEVEL** etc.—is tabulated in the guide; replicate or customise as needed. citeturn4file13turn4file15

---

## ASSIGN 1‑16 (per Memory)

| Parameter          | Values (default in**bold**)                                                                         | Explanation                                         |
| ------------------ | --------------------------------------------------------------------------------------------------------- | --------------------------------------------------- |
| SW                 | **OFF**, ON                                                                                        | Enables the assignment.                             |
| SOURCE             | TRK 1‑6 REC/PLY, PLY/STP, PEDAL1‑9 MODE1‑3, CTL1‑4, EXP1/2, MIDI CC#01‑31/64‑95, SYNC ST/STP, etc. | Event or controller that will drive the assignment. |
| SOURCE MODE        | **MOMENT**, TOGGLE                                                                                  | How a momentary footswitch behaves.                 |
| ACT LOW / ACT HIGH | 0‑127                                                                                                    | Active range of the SOURCE that triggers control.   |
| TARGET             | Hundreds of functions (Track transport, levels, FX parameters, mixer outs, etc.)                          | Destination parameter to control.                   |
| TARGET MIN / MAX   | Contextual range                                                                                          | Range the target will sweep within.                 |

> Assignments are stored per memory and saved with **WRITE**. citeturn4file7turn4file8

---

## USB

| Parameter    | Values (default in**bold**)       | Explanation                                                     |
| ------------ | --------------------------------------- | --------------------------------------------------------------- |
| STORAGE      | **OFF**, CONNECT                 | Puts unit into mass‑storage mode for file backup when CONNECT. |
| AUDIO MODE   | **GENERIC**, VENDOR              | Select class‑compliant or dedicated BOSS driver.               |
| ROUTING      | **LINE OUT**, SUB MIX, LOOP IN | Destination for USB‑return audio.                              |
| INPUT LEVEL  | 0 …**100**… 200               | Level of audio coming*from*computer.                          |
| OUTPUT LEVEL | 0 …**100**… 200               | Level of audio sent*to*computer.                              |

citeturn4file4turn4file6

---

## MIDI

| Parameter     | Values (default in**bold**)    | Explanation                                   |
| ------------- | ------------------------------------ | --------------------------------------------- |
| RX CH CTL    | 1‑16                                | Receive channel for control‑change messages. |
| RX CH RHYTHM | 1‑16                                | Channel for rhythm note messages.             |
| RX CH VOICE  | 1‑16                                | Channel for Harmony/Vocoder notes.            |
| TX CH         | 1‑16, RX CTL                      | Transmit channel.                             |
| SYNC CLOCK    | **AUTO**, INTERNAL, MIDI, USB  | Clock source.                                 |
| CLOCK OUT     | **OFF**, ON                    | Whether to transmit MIDI clock.               |
| START SYNC    | OFF,**ALL**, RHYTHM            | Response to external MIDI Start.              |
| PC OUT        | **OFF**, ON                    | Whether to send program‑change.              |
| THRU          | OFF, MIDI OUT, USB OUT, USB & MIDI | Soft‑thru routing of incoming MIDI.          |

citeturn4file6

---

## SETUP

| Parameter                 | Values / Range                                                                                             | Default | Description                                                                                    |
| ------------------------- | ---------------------------------------------------------------------------------------------------------- | ------- | ---------------------------------------------------------------------------------------------- |
| **CONTRAST**        | 1–10                                                                                                      | 6       | Adjusts display contrast. citeturn1file2                                                 |
| **DISPLAY MODE**    | MEMORY NUMBER, TRACK STATUS, LOOP TRACKS, LOOP STATUS, LOOP LEVEL, INPUT FX, TRACK FX                      | —      | Selects the play screen shown immediately after startup. citeturn1file2                  |
| **LOOP INDICATORS** | LOOP INDICAT (outer ring): LOOP, RHYTHM, BEAT, TEMPO; ORB INDICAT (center ring): LOOP, RHYTHM, BEAT, TEMPO | —      | Controls which information the outer and center LED rings display. citeturn1file2        |
| **AUTO OFF**        | OFF / ON                                                                                                   | OFF     | When ON, power turns off automatically if the unit is idle for 10 h. citeturn1file2     |
| **MEMORY EXTENT**   | MIN 01–99, MAX 01–99                                                                                     | —      | Limits the range of memory numbers scrollable from the panel. citeturn1file2             |
| **KNOB FUNC 1–4**  | See_Knob Func Values_ full list below*                                                                     | —      | Assigns the function of each top‑panel knob during playback. citeturn1file5turn1file8 |

_Knob Func Values_

| Value                                           | Description                                                                                                       |
| ----------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| **OFF**                                   | Disables the knob (assignments 2 or 3).                                                                           |
| **MEMORY**                                | Switch memories when turned.                                                                                      |
| **TRK 1–6 REVERSE**                      | Toggles reverse playback for the specified track.                                                                 |
| **TRK 1–6 1SHOT**                        | Switches the specified track to one‑shot playback.                                                               |
| **TRK 1–6 PAN**                          | Pans the specified track left‑right.                                                                             |
| **TRK 1–6 LEVEL**                        | Adjusts volume of the specified track.                                                                            |
| **TRK 1–6 DUB**                          | Adjusts overdub mode for the specified track.                                                                     |
| **TRK 1–6 FX**                           | Controls input/track FX depth for the specified track.                                                            |
| **TRK 1–6 SPEED**                        | Controls playback speed of the specified track.                                                                   |
| **TRK 1–6 BNC IN**                       | Toggles bounce‑record enable on the specified track.                                                             |
| **DUB MODE**                              | Selects the global overdub mode.                                                                                  |
| **AUTO REC**                              | Enables automatic recording start.                                                                                |
| **BOUNCE**                                | Starts/stops bounce recording.                                                                                    |
| **CURRENT TRACK**                         | Selects which track is the “current” one.                                                                       |
| **FD TIME IN**                            | Sets fade‑in time.                                                                                               |
| **FD TIME OUT**                           | Sets fade‑out time.                                                                                              |
| **IN FX A–D SW**                         | Toggles Input FX A–D on/off for the current bank.                                                                |
| **IN FX A–D TYPE**                       | Selects Input FX A–D type for the current bank.                                                                  |
| **IN FX A–D PRM1–4**                    | Adjusts parameters 1–4 of Input FX A–D.                                                                        |
| **IN FX A–D SW MODE**                    | Sets toggle/momentary for Input FX A–D.                                                                          |
| **IN FX BANK**                            | Selects the Input FX bank (A–D).                                                                                 |
| **IN FX MODE**                            | Selects Input FX MODE parameter.                                                                                  |
| **IN FX SW**                              | Global Input FX on/off.                                                                                           |
| **IN FX TARGET**                          | Chooses which Input FX (A–D) is targeted.                                                                        |
| **IN FX SW MODE**                         | Toggle/momentary for all Input FX at once.                                                                        |
| **IN FX CUR PRM1–4**                     | Adjusts parameters 1–4 of the*currently selected*Input FX.                                                    |
| **IN FX CUR SEQ**                         | Turns Input FX sequence on/off.                                                                                   |
| **IN FX CUR S.SYNC**                      | Sync on/off for current Input FX sequence.                                                                        |
| **IN FX CUR S.RTRIG**                     | Retrigger on/off for current Input FX seq.                                                                        |
| **IN FX CUR S.RATE**                      | Step‑rate for current Input FX seq.                                                                              |
| **IN FX CUR S.MAX**                       | Step‑max for current Input FX seq.                                                                               |
| **TR FX A–D SW**                         | Toggles Track FX A–D on/off for the current bank.                                                                |
| **TR FX A–D TYPE**                       | Selects Track FX A–D type for the current bank.                                                                  |
| **TR FX A–D PRM1–4**                    | Adjusts parameters 1–4 of Track FX A–D.                                                                        |
| **TR FX A–D SW MODE**                    | Sets toggle/momentary for Track FX A–D.                                                                          |
| **TR FX BANK**                            | Selects the Track FX bank (A–D).                                                                                 |
| **TR FX MODE**                            | Selects Track FX MODE parameter.                                                                                  |
| **TR FX SW**                              | Global Track FX on/off.                                                                                           |
| **TR FX SW MODE**                         | Toggle/momentary for all Track FX A–D.                                                                           |
| **TR FX TGT INC / DEC**                   | Cycles which Track FX (A–D) is targeted.                                                                         |
| **TR FX BNK INC / DEC**                   | Cycles Track FX banks A→D or D→A.                                                                               |
| **T FX A–D / AA–DD …**                 | Full per‑FX controls (CTL, TYPE, PRM1–4, SEQ, SYNC, RTRIG, RATE, MAX) for Track FX across banks and sub‑banks. |
| **RHYTHM LEVEL**                          | Adjusts rhythm output level.                                                                                      |
| **RHYTHM VARI**                           | Selects rhythm pattern variation.                                                                                 |
| **RHYTHM KIT**                            | Selects drum kit.                                                                                                 |
| **RHYTHM R.INTRO / P.INTRO**              | Toggles rhythm intro record / play.                                                                               |
| **RHYTHM ENDING**                         | Triggers rhythm ending.                                                                                           |
| **MIC 1, 2 LEVEL / MUTE**               | Adjusts or mutes MIC inputs.                                                                                      |
| **INST 1, 2 LEVEL / MUTE**              | Adjusts or mutes INST inputs.                                                                                     |
| **INST1(R), 2(R) LEVEL / MUTE**          | Adjusts or mutes right‑channel INST inputs.                                                                      |
| **LOOP LEVEL**                            | Master loop playback level.                                                                                       |
| **MAIN LEVEL / MAIN(R) LEVEL**            | Adjusts main output L or R.                                                                                       |
| **SUB1, 2 LEVEL / SUB1(R), 2(R) LEVEL** | Adjusts sub outputs.                                                                                              |
| **PHONES LEVEL**                          | Headphone output level.                                                                                           |
| **MASTER LEVEL**                          | Global master level.                                                                                              |
| **INST 1, 2 GAIN**                      | Adjusts instrument input gain.                                                                                    |
| **MIC 1, 2 EQ***                        | Comprehensive MIC EQ parameters: SW, LO G, HI G, LM F, LM Q, LM G, HM F, HM Q, HM G, LVL, LO C, HI C.   |
| **INST1‑L,R EQ***                        | Same full EQ parameter set for INST1.                                                                             |
| **INST2‑L,R EQ***                        | Same full EQ parameter set for INST2.                                                                             |
| **MAIN‑L,R EQ***                         | Same full EQ parameter set for MAIN out.                                                                          |
| **SUB1‑L,R EQ***                         | Same full EQ parameter set for SUB 1 out.                                                                         |
| **SUB2‑L,R EQ***                         | Same full EQ parameter set for SUB 2 out.                                                                         |
| **RHYTHM OUT**                            | Adjusts rhythm out routing level.                                                                                 |
| **INPUT THRU**                            | Adjusts input‑thru monitor level.                                                                                |
| **PHONES OUT**                            | Adjusts headphone routing level.                                                                                  |
| **MFX COMP / REVERB**                     | Controls master compressor or reverb depth.                                                                       |
| **PEDAL MODE**                            | Switches pedal‑mode bank (1–3).                                                                                 |
| **DISPLAY MODE**                          | Cycles play‑screen mode.                                                                                         |
| **LOOP INDICAT / ORB INDICAT**            | Selects indicator‑ring assignment.                                                                               |
| **LOOP STATUS COLOR**                     | Sets LED colors for REC/PLAY/DUB/STOP/BLANK.                                                                      |

*Entries marked ****EQ*** represent 12 independent EQ parameters each (SW, frequency, gain, Q, etc.) for the listed source/output, as detailed in the Parameter Guide.*
citeturn1file5turn1file8turn1file16

---

## FACTORY RESET

| Parameter     | Values                          | Explanation                                   |
| ------------- | ------------------------------- | --------------------------------------------- |
| FACTORY RESET | MEMORY 01‑99, SYSTEM, MEM+SYS | Restores factory defaults for selected scope. |

citeturn4file1

## Input/Track FX – Full Parameter Tables

### SEQ Data

This table describes the SEQ data that exists for FXs that are marked with a YES in the **Enumeration of all FX** table below.

| Parameter             | Values*                                                    | What it does                                                                                                                                                                              |
| --------------------- | ---------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **SW**          | `OFF`,`ON`                                             | Turns the FX-Sequence function itself off/on.                                                                                                                                             |
| **SYNC**        | `OFF`,`ON`                                             | When**ON** , the first step of the sequence is aligned (cued) with the start of loop playback.                                                                                      |
| **RETRIG**      | `OFF`,`ON`                                             | When**ON** , the sequence is re-started from step 1 every time you switch <br />the effect on (e.g., via a foot-switch).                                                            |
| **TARGET**      | *parameter list varies by effect*                        | Chooses which effect parameter the sequence will change. Parameters that can be<br /> targeted are labeled as Target in the SEQ column. Those marked as Initial indicate intial values. |
| **RATE**        | `0–100`,`4MEAS`,`2MEAS`,`1MEAS`,`½–1/32 note` | Sets the cycle length of the entire sequence.                                                                                                                                             |
| **MAX**         | `1–16`                                                  | Defines how many steps (up to 16) are active in the sequence.                                                                                                                             |
| **VAL1–VAL16** | `1–16`                                                  | Step-by-step values that the TARGET parameter will take.<br />One entry for each possible step.                                                                                           |

### 1 LPF – Low‑Pass Filter

| Parameter | Values (default in**bold**)       | Explanation                     | SEQ     |
| --------- | --------------------------------------- | ------------------------------- | ------- |
| RATE      | 4MEAS, 2MEAS, 1MEAS, ♩ … ♬, 0‑100 | LFO speed for cutoff modulation |         |
| DEPTH     | 0‑100                                  | Modulation depth                | Target  |
| RESONANCE | 0‑100                                  | Resonance (Q) at cutoff         |         |
| CUTOFF    | 0‑100                                  | Cutoff frequency                | Initial |
| STEP RATE | OFF, 4MEAS … 0‑100                  | Rate when**FX SEQ**is ON |         |

### 2 BPF – Band‑Pass Filter

| Parameter | Values           | Explanation      | SEQ     |
| --------- | ---------------- | ---------------- | ------- |
| RATE      | same list as LPF | LFO speed        |         |
| DEPTH     | 0‑100           | Mod depth        | Target  |
| RESONANCE | 0‑100           | Band resonance   |         |
| CUTOFF    | 0‑100           | Center frequency | Initial |
| STEP RATE | OFF … 0‑100  | Seq rate         |         |

### 3 HPF – High‑Pass Filter

| Parameter | Values           | Explanation      | SEQ     |
| --------- | ---------------- | ---------------- | ------- |
| RATE      | same list as LPF | LFO speed        |         |
| DEPTH     | 0‑100           | Depth            | Target  |
| RESONANCE | 0‑100           | Q at cutoff      |         |
| CUTOFF    | 0‑100           | Cutoff frequency | Initial |
| STEP RATE | OFF … 0‑100  | Seq rate         |         |

### 4 Phaser

| Parameter | Values              | Explanation        | SEQ     |
| --------- | ------------------- | ------------------ | ------- |
| RATE      | 4MEAS … 0‑100   | Effect speed       |         |
| DEPTH     | 0‑100              | Effect depth       | Target  |
| RESONANCE | 0‑100              | Feedback intensity | Target  |
| MANUAL    | 0‑100              | Center frequency   | Initial |
| STAGE     | 4, 8, 12, BI‑PHASE | Number of stages   |         |
| STEP RATE | OFF … 0‑100     | Seq rate           |         |
| D.LEVEL   | 0‑100              | Dry level          | Target  |
| E.LEVEL   | 0‑100              | Effect level       | Target  |

### 5 Flanger

| Parameter  | Values            | Explanation      | SEQ     |
| ---------- | ----------------- | ---------------- | ------- |
| RATE       | 4MEAS … 0‑100 | Speed            |         |
| DEPTH      | 0‑100            | Depth            | Target  |
| RESONANCE  | 0‑100            | Feedback         | Target  |
| MANUAL     | 0‑100            | Center frequency | Initial |
| STEP RATE  | OFF … 0‑100   | Seq rate         |         |
| D.LEVEL    | 0‑100            | Dry level        | Target  |
| E.LEVEL    | 0‑100            | Wet level        | Target  |
| SEPARATION | 0‑100            | Stereo width     | Target  |

### 6 Synth

| Parameter    | Values (default in**bold**) | Explanation                  | SEQ    |
| ------------ | --------------------------------- | ---------------------------- | ------ |
| FREQUENCY ② | 0 –**50** – 100           | Filter cutoff position       | Target |
| RESONANCE ② | 0 –**50** – 100           | Filter emphasis              | Target |
| DECAY ②     | 0 –**50** – 100           | Time for the filter to close | Target |
| BALANCE      | 0 –**50** – 100           | Direct : synth level blend   |        |

### 7 Lo-Fi

| Parameter  | Values                              | Explanation           |
| ---------- | ----------------------------------- | --------------------- |
| BITDEPTH   | **OFF**, 31 – 8 – 1         | Word-length reduction |
| SAMPLERATE | **OFF**, ½ – ¼ – ¹⁄₃₂ | Sample-rate reduction |
| BALANCE    | 0 –**50** – 100             | Dry : effect mix      |

### 8 Radio

| Parameter | Values                  | Explanation                 |
| --------- | ----------------------- | --------------------------- |
| LO-FI     | 1 –**6** – 10   | Amount of “AM-radio” blur |
| LEVEL     | 0 –**50** – 100 | Effect volume               |

### 9 Ring Mod

| Parameter    | Values                  | Explanation               | SEQ     |
| ------------ | ----------------------- | ------------------------- | ------- |
| FREQUENCY ② | 0 –**50** – 100 | Internal-oscillator pitch | Initial |
| BALANCE      | 0 –**50** – 100 | Dry : effect mix          |         |
| MODE         | **1**, 2          | Algorithm (legacy / new)  |         |

### 10 G2B (Guitar-to-Bass)

| Parameter | Values                  | Explanation              |
| --------- | ----------------------- | ------------------------ |
| BALANCE   | 0 –**50** – 100 | Dry : effect mix         |
| MODE      | **1**, 2          | Algorithm (legacy / new) |

### 11 Sustainer

| Parameter | Values            | Explanation          |
| --------- | ----------------- | -------------------- |
| ATTACK    | 0‑100            | Pick attack emphasis |
| RELEASE   | 0‑100            | Release time         |
| LEVEL     | 0‑100            | Effect level         |
| LOW GAIN  | ‑20 … +20 dB | Low‑shelf EQ        |
| HI GAIN   | ‑20 … +20 dB | High‑shelf EQ       |
| SUSTAIN   | 0‑100            | Sustain amount       |

### 12 Auto Riff

| Parameter | Values                    | Explanation              |
| --------- | ------------------------- | ------------------------ |
| PHRASE    | **P1** – P30       | Factory riff pattern     |
| TEMPO     | 4 MEAS – … – 0 – 100  | Playback speed           |
| HOLD      | **OFF**, ON         | Latch the phrase         |
| ATTACK    | 0 –**50** – 100   | Added pick attack        |
| LOOP      | **OFF**, ON         | Loop phrase continuously |
| KEY       | **C**(Am) – B(G#m) | Musical key              |
| BALANCE   | 0 –**50** – 100   | Dry : effect mix         |

### 13 Slow Gear

| Parameter | Values                  | Explanation              |
| --------- | ----------------------- | ------------------------ |
| SENS      | 0 –**50** – 100 | Input sensitivity        |
| RISE TIME | 0 –**50** – 100 | Fade-in length           |
| LEVEL     | 0 –**50** – 100 | Effect volume            |
| MODE      | **1**, 2          | Algorithm (legacy / new) |

### 14 Transpose

| Parameter | Values                                | Explanation              | SEQ     |
| --------- | ------------------------------------- | ------------------------ | ------- |
| TRANS ②  | –12 –**0** – +12 (semitones) | Fixed pitch shift        | Initial |
| MODE      | **1**, 2                        | Algorithm (legacy / new) |         |

### 15 Pitch Bend

| Parameter | Values                  | Explanation              | SEQ    |
| --------- | ----------------------- | ------------------------ | ------ |
| PITCH     | –3 – +4**OCT**  | Maximum bend range       |        |
| BEND ②   | 0 –**50** – 100 | Bend amount inside range | Target |
| MODE      | **1**, 2          | Algorithm (legacy / new) |        |

### 16 Robot

| Parameter | Values                    | Explanation               |
| --------- | ------------------------- | ------------------------- |
| NOTE      | **C** – B          | Fixed vocal pitch         |
| FORMANT   | –50 –**0** – +50 | Voice character (♂ / ♀) |
| MODE      | **1**, 2            | Algorithm (legacy / new)  |

### 17 Electric

| Parameter | Values                    | Explanation       |
| --------- | ------------------------- | ----------------- |
| SHIFT     | –12 –**0** – +12 | Pitch step amount |
| FORMANT   | –50 –**0** – +50 | Voice character   |
| SPEED     | 0 –**5** – 10     | Quantise speed    |
| STABILITY | –10 –**0** – +10 | Tracking ease     |
| SCALE     | CHROMATIC, Key (C … B)   | Scale type        |

### 18 HRM Manual

| Parameter | Values                      | Explanation      |
| --------- | --------------------------- | ---------------- |
| VOICE     | OCT±, -6th … +6th, UNISON | Harmony interval |
| FORMANT   | –50 –**0** – +50   | Harmony tone     |
| PAN       | L50 – CENTER – R50        | Harmony pan      |
| KEY       | **C**(Am) – B(G#m)   | Song key         |
| D.LEVEL   | 0 –**100**           | Dry level        |
| HRM LEVEL | 0 – 80 – 100              | Harmony level    |

### 19 HRM Auto (MIDI)

| Parameter | Values                         | Explanation        |
| --------- | ------------------------------ | ------------------ |
| VOICE     | OCT±, LOWER … HIGHER, UNISON | Interval from MIDI |
| FORMANT   | –50 –**0** – +50      | Harmony tone       |
| PAN       | L50 – CENTER – R50           | Harmony pan        |
| HRM MODE  | **HYBRID**, AUTO         | Chord source       |
| KEY       | **C**(Am) – B(G#m)      | Fallback key       |
| D.LEVEL   | 0 –**100**              | Dry level          |
| HRM LEVEL | 0 – 80 – 100                 | Harmony level      |

### 20 Vocoder

| Parameter    | Values                            | Explanation            |
| ------------ | --------------------------------- | ---------------------- |
| CARRIER      | MIC 1/2, INST 1/2-L/R, TRACK 1–6 | Modulator source       |
| TONE         | –50 –**0** – +50         | Timbre                 |
| ATTACK       | 0 –**50** – 100           | Envelope attack        |
| MOD SENS     | –50 –**0** – +50         | Modulation sensitivity |
| BALANCE      | 0 –**50** – 100           | Dry : vocoder mix      |
| CARRIER THRU | **OFF**, ON                 | Pass carrier straight  |

### 21 OSC VOC Midi

| Parameter     | Values                                  | Explanation            |
| ------------- | --------------------------------------- | ---------------------- |
| OSC / CARRIER | SAW, VINT SAW, DETUNE SAW, SQUARE, RECT | Synth waveform         |
| TONE          | ‑50 … +50                           | Bright‑dark character |
| ATTACK        | 0‑100                                  | Attack time            |
| NOTE / OCTAVE | C‑G9 (BOT) or ‑2OCT … +1OCT (VOC)  | Pitch source           |
| MOD SENS      | ‑50 … +50                           | Input modulation depth |
| RELEASE (VOC) | 0‑100                                  | Release time           |
| BALANCE       | 0‑100                                  | Dry‑wet               |

### 22 OSC BOT

| Parameter     | Values                                  | Explanation            | SEQ     |
| ------------- | --------------------------------------- | ---------------------- | ------- |
| OSC / CARRIER | SAW, VINT SAW, DETUNE SAW, SQUARE, RECT | Synth waveform         |         |
| TONE          | ‑50 … +50                           | Bright‑dark character |         |
| ATTACK        | 0‑100                                  | Attack time            |         |
| NOTE / OCTAVE | C‑G9 (BOT) or ‑2OCT … +1OCT (VOC)  | Pitch source           | Initial |
| MOD SENS      | ‑50 … +50                           | Input modulation depth |         |
| RELEASE (VOC) | 0‑100                                  | Release time           |         |
| BALANCE       | 0‑100                                  | Dry‑wet               |         |

### 23 Preamp

| Parameter           | Values                                                                                                           | Explanation    |
| ------------------- | ---------------------------------------------------------------------------------------------------------------- | -------------- |
| AMP TYPE            | JC‑120, NAT CLEAN, FULL RANGE, COMBO CRUNCH, STACK CRUNCH, HI‑GAIN STACK, POWER DRIVE, EXTREM LEAD, CORE METAL | Amp model      |
| SPK TYPE            | OFF, ORIGINAL, 1×8″ … 8×12″                                                                               | Virtual cab    |
| GAIN                | 0‑120                                                                                                           | Distortion     |
| T‑COMP             | ‑10 … +10                                                                                                    | Tube comp feel |
| BASS / MID / TREBLE | 0‑100                                                                                                           | Tone stack     |
| LEVEL               | 0‑100                                                                                                           | Output level   |

### 24 Dist (Vocal / Boost / OD / Metal / Fuzz)

| Parameter | Values                            | Explanation  |
| --------- | --------------------------------- | ------------ |
| TYPE      | VOCAL, BOOST, OD, DS, METAL, FUZZ | Dist circuit |
| TONE      | ‑50 … 0 … +50              | Brightness   |
| DIST      | 0‑100                            | Drive amount |
| D.LEVEL   | 0‑100                            | Dry level    |
| E.LEVEL   | 0‑100                            | Wet level    |

### 25 Dynamics

| Parameter          | Values (default in**bold**)                                                                                                                                                                                               | What it does                                                                                            |
| ------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| **TYPE**     | **NATURAL COMP**, MIXER COMP, LIVE COMP, NATURAL LIM, HARD LIM, JINGL COMP, HARD COMP, SOFT COMP, CLEAN COMP, DANCE COMP, ORCH COMP, VOCAL COMP, ACOUSTIC, ROCK BAND, ORCHESTRA, LOW BOOST, BRIGHTEN, DJ VOICE, PHONE VOX | Chooses a preset compressor/limiter curve and EQ contour.                                               |
| **DYNAMICS** | –20 –**0** – +20                                                                                                                                                                                                       | Raises or lowers the amount of dynamic levelling (bigger negative = gentler; bigger positive = harder). |

### 26 4-Band EQ

| Parameter       | Values                       | Explanation      |
| --------------- | ---------------------------- | ---------------- |
| LO / HI         | –20 –**0** – +20 dB | Shelf gain       |
| LO-MID / HI-MID | –20 –**0** – +20 dB | Peaking gain     |
| LO-MID FREQ     | 20 Hz – 10 kHz              | Center frequency |
| LO-MID Q        | 0.5 – 16                    | Bandwidth        |
| HI-MID FREQ     | 20 Hz – 10 kHz              | Center frequency |
| HI-MID Q        | 0.5 – 16                    | Bandwidth        |
| HIGH            | –20 –**0** – +20 dB | High-shelf gain  |
| LEVEL           | 0 –**50** – 100      | Overall EQ level |

### 27 Isolator

| Parameter  | Values                 | Explanation           | SEQ     |
| ---------- | ---------------------- | --------------------- | ------- |
| BAND       | LOW, MID, HIGH         | Frequency band to cut |         |
| RATE       | 4MEAS … 0‑100      | Mod speed             |         |
| BAND LEVEL | 0‑100                 | Amount cut            |         |
| DEPTH      | 0‑100                 | Depth                 | Initial |
| STEP RATE  | OFF, 4MEAS … 0‑100 | Seq rate              |         |
| WAVEFORM   | TRI, SQR               | Mod shape             |         |

### 28 Octave

| Parameter           | Values (default in**bold**)         | What it does                                                                                                     | SEQ    |
| ------------------- | ----------------------------------------- | ---------------------------------------------------------------------------------------------------------------- | ------ |
| **OCTAVE**    | **-1 OCT**, -2 OCT, -1 OCT & -2 OCT | Chooses which lower octave(s) are added to the original note.                                                    |        |
| **MODE**      | **1**, 2                            | Selects the algorithm:<br />**1** = legacy (same as earlier RC models); <br />**2** = new algorithm. |        |
| **OCT.LEVEL** | 0 –**50** – 100                   | Sets the volume of the added-octave sound.                                                                       | Target |

### 29 Auto Pan

| Parameter  | Values            | Explanation        |
| ---------- | ----------------- | ------------------ |
| RATE       | 4MEAS … 0‑100 | Pan speed          |
| DEPTH      | 0‑100            | Pan width          |
| WAVEFORM   | 0‑100            | Shape crispness    |
| INIT PHASE | 0‑180            | Start pan position |
| STEP RATE  | OFF … 0‑100   | Seq rate           |

### 30 Manual Pan

| Parameter | Values           | Explanation              | SEQ     |
| --------- | ---------------- | ------------------------ | ------- |
| POSITION  | L50‑CENTER‑R50 | Fixed pan via expression | Initial |

### 31 Stereo Enhance

| Parameter | Values                   | Explanation  |
| --------- | ------------------------ | ------------ |
| LOW CUT   | FLAT / 20 Hz – 12.5 kHz | Remove lows  |
| HIGH CUT  | 20 Hz – 12.5 kHz / FLAT | Remove highs |
| ENHANCE   | 0 –**50** – 100  | Width depth  |

### 32 Tremolo

| Parameter | Values            | Explanation   | SEQ     |
| --------- | ----------------- | ------------- | ------- |
| RATE      | 4MEAS … 0‑100 | Tremolo speed | Target  |
| DEPTH     | 0‑100            | Depth         | Initial |
| WAVEFORM  | TRI, SQR          | LFO shape     |         |

### 33 Vibrato

| Parameter | Values            | Explanation                                          | SEQ     |
| --------- | ----------------- | ---------------------------------------------------- | ------- |
| RATE      | 4MEAS … 0‑100 | Pitch‑mod speed                                     |         |
| DEPTH     | 0-50-100          | Pitch‑mod depth                                     | Initial |
| COLOR     | 0-50-100          | Higher settings produce more<br />omplex modulation. |         |
| D.LEVEL   | 0-50-100          | Direct level                                         | Target  |
| E.LEVEL   | 0-50-100          | Effect level                                         | Target  |

### 34 Pattern Slicer

| Parameter   | Values (default in**bold**) | Explanation                  |
| ----------- | --------------------------------- | ---------------------------- |
| RATE        | 4MEAS … 0‑100                 | Tempo‑synced slice rate     |
| DUTY        | 1 …**60**… 99           | Slice gate length            |
| ATTACK      | 0‑35‑100                        | Slice attack accent          |
| PATTERN     | P01‑20                           | 20 preset groove patterns    |
| DEPTH       | 0‑100                            | Depth of volume cut          |
| COMP THRESH | ‑30 dB … 0 dB               | Threshold for built‑in comp |
| COMP GAIN   | 0 …**+2**… +20 dB      | Make‑up gain                |

### 35 Step Slicer

| Parameter   | Values            | Explanation             |
| ----------- | ----------------- | ----------------------- |
| RATE        | 4MEAS … 0‑100 | Slice rate              |
| STEP MAX    | 1‑16             | Max steps per measure   |
| STEP LEN    | 1‑100            | Length of each step (%) |
| STEP LVL    | 0‑100            | Level per step          |
| DEPTH       | 0‑100            | Depth                   |
| COMP THRESH | ‑30–0 dB       | Comp threshold          |
| COMP GAIN   | 0–+6–+20 dB    | Gain                    |

### 36 Delay (standard)

| Parameter | Values                   | Explanation  |
| --------- | ------------------------ | ------------ |
| TIME      | 1 – 200 – 2000 ms / 𝄽 | Delay time   |
| FEEDBACK  | 1 –**16**         | Repeats      |
| D.LEVEL   | 0 –**100**        | Dry level    |
| LOW CUT   | FLAT / 20 Hz – 12.5 kHz | Remove lows  |
| HIGH CUT  | 20 Hz – 12.5 kHz / FLAT | Remove highs |
| E.LEVEL   | 0 – 50 – 120           | Echo level   |

### 37 Panning Delay

| Parameter    | Values                    | Explanation   |
| ------------ | ------------------------- | ------------- |
| TIME         | 1 ms … 2000 ms       | Base delay    |
| FEEDBACK     | 1‑16                     | Repeats       |
| D.LEVEL      | 0‑100                    | Dry level     |
| LOW/HIGH CUT | FLAT or 20 Hz‑12.5 kHz | Band‑shaping |
| E.LEVEL      | 0‑120                    | Echo level    |

### 38 Reverse Delay

Same parameter block as Panning Delay. Produces time‑reversed echoes. citeturn9file1

### 39 Mod Delay

| Parameter          | Values                   | Explanation  |
| ------------------ | ------------------------ | ------------ |
| TIME               | 1 – 200 – 2000 ms / 𝄽 | Delay time   |
| FEEDBACK           | 1 –**16**         | Repeats      |
| MOD DEPTH          | 0 –**50** – 100  | Chorus depth |
| D.LEVEL            | 0 –**100**        | Dry level    |
| LOW CUT / HIGH CUT | As above                 | Tone shaping |
| E.LEVEL            | 0 – 50 – 120           | Echo level   |

### 40 Tape Echo 1

| Parameter     | Values                  | Explanation |
| ------------- | ----------------------- | ----------- |
| REPEAT RATE   | 0 –**50** – 100 | Tape speed  |
| INTENSITY     | 0 –**50** – 100 | Feedback    |
| D.LEVEL       | 0 –**100**       | Dry level   |
| BASS / TREBLE | –50 – 0 – +50        | Tone        |
| E.LEVEL       | 0 – 50 – 120          | Echo level  |

### 41 Tape Echo 2

| Parameter                              | Values                   | Explanation |
| -------------------------------------- | ------------------------ | ----------- |
| REPEAT RATE                            | 1 – 200 – 2000 ms / 𝄽 | Tape speed  |
| INTENSITY                              | 0 –**50** – 100  | Feedback    |
| D.LEVEL / LOW CUT / HIGH CUT / E.LEVEL | As above                 |             |

### 42 Granular Delay

| Parameter | Values                  | Explanation            |
| --------- | ----------------------- | ---------------------- |
| TIME      | 0 –**50** – 100 | Spacing between grains |
| FEEDBACK  | 0 – 70 – 100          | Repeat length          |
| E.LEVEL   | 0 –**50** – 100 | Effect level           |
| Source    |                         |                        |

### 43 Warp

| Parameter | Values                  | Explanation   |
| --------- | ----------------------- | ------------- |
| LEVEL     | 0 –**50** – 100 | Effect volume |

### 44 Twist

| Parameter | Values                  | Explanation   |
| --------- | ----------------------- | ------------- |
| RELEASE   | **FALL**, FADE    | How it stops  |
| RISE      | 0 –**50** – 100 | Rise-time     |
| FALL      | 0 –**50** – 100 | Fade-time     |
| LEVEL     | 0 –**50** – 100 | Effect volume |

### 45 Roll 1

| Parameter | Values                        | Explanation       |
| --------- | ----------------------------- | ----------------- |
| TIME      | 1 – 1000 ms / 𝄽             | Loop length       |
| FEEDBACK  | 1 – 50 – 100                | Repeats after off |
| ROLL      | OFF / ½ / ¼ / ⅛ / ¹⁄₁₆ | Sub-divides loop  |
| BALANCE   | 0 –**50** – 100       | Dry : effect mix  |

### 46 Roll 2

| Parameter | Values                  | Explanation      |
| --------- | ----------------------- | ---------------- |
| TIME      | 1 – 1000 ms / 𝄽       | Loop length      |
| REPEAT    | 1 – 50 – 100 / INF    | Repeats          |
| BALANCE   | 0 –**50** – 100 | Dry : effect mix |

### 47 Freeze

| Parameter | Values                  | Explanation          |
| --------- | ----------------------- | -------------------- |
| ATTACK    | 0 –**30** – 100 | Fade-in time         |
| RELEASE   | 0 –**30** – 100 | Fade-out time        |
| DECAY     | 0 –**30** – 100 | Decay of frozen tone |
| SUSTAIN   | 0 –**30** – 100 | Sustain amount       |
| BALANCE   | 0 –**50** – 100 | Dry : effect mix     |

### 48 Chorus

| Parameter | Values                  | Explanation  |
| --------- | ----------------------- | ------------ |
| RATE      | 0‑100, ♩              | Mod rate     |
| DEPTH     | 0‑100                  | Depth        |
| D.LEVEL   | 0‑100                  | Direct level |
| LOW CUT   | FLAT, 20 Hz‑12.5 kHz | HPF          |
| HIGH CUT  | 20 Hz‑12.5 kHz, FLAT | LPF          |
| E.LEVEL   | 0‑100                  | Wet level    |

### 49 Reverb

| Parameter      | Values                    | Explanation         |
| -------------- | ------------------------- | ------------------- |
| TIME           | 0.1 –**3** – 10 s | Reverb length       |
| PRE DELAY      | 0 – 500 ms               | Input-to-reverb gap |
| DENSITY        | 1 –**6** – 10     | Reflection density  |
| D.LEVEL        | 0 –**100**         | Dry level           |
| LOW / HIGH CUT | FLAT / 20 Hz–12.5 kHz    | Tone shaping        |
| E.LEVEL        | 0 – 50 – 100            | Wet level           |

### 50 Gate Reverb

| Parameter                              | Values                    | Explanation      |
| -------------------------------------- | ------------------------- | ---------------- |
| TIME                                   | 0.1 –**3** – 10 s | Reverb length    |
| PRE DELAY                              | 0 – 500 ms               | Gap              |
| THRESHOLD                              | 0 –**50** – 100   | Gate close level |
| D.LEVEL / LOW-CUT / HIGH-CUT / E.LEVEL | As above                  |                  |

### 51 Reverse Reverb

| Parameter                              | Values                    | Explanation   |
| -------------------------------------- | ------------------------- | ------------- |
| TIME                                   | 0.1 –**3** – 10 s | Reverb length |
| PRE DELAY                              | 0 – 500 ms               | Gap           |
| GATE TIME                              | 0.1 – 0.5 – 1 s         | Fade-in time  |
| D.LEVEL / LOW-CUT / HIGH-CUT / E.LEVEL | As above                  |               |

> Note: The effects shown below can only be used with FX A when the
> TRACK FX MODE (p. 6) is set to “MULTI”

### 52 Beat Scatter (track-only)

| Parameter | Values                | Explanation   |
| --------- | --------------------- | ------------- |
| TYPE      | **P1** – P4    | Scrub flavour |
| LENGTH    | THRU / 𝄽 – ♩ – ♪ | Scrub length  |
| Source    |                       |               |

### 53 Beat Scatter (track-only)

| Parameter | Values                 | Explanation   |
| --------- | ---------------------- | ------------- |
| TYPE      | FORWARD / REWIND / MIX | Direction     |
| LENGTH    | THRU / 𝄽 – ♩ – ♪  | Repeat length |

### 54 Beat Shift (track-only)

| Parameter | Values                | Explanation          |
| --------- | --------------------- | -------------------- |
| TYPE      | FUTURE / PAST         | Shift ahead / behind |
| SHIFT     | THRU / ♪ – ♩ – 𝄽 | Offset amount        |

| Parameter | Values                  | Explanation           |
| --------- | ----------------------- | --------------------- |
| FLICK     | 0 –**50** – 100 | Virtual platter speed |

### 55 Vinyl Flick (track-only)

| Parameter | Values                  | Explanation           |
| --------- | ----------------------- | --------------------- |
| FLICK     | 0 –**50** – 100 | Virtual platter speed |

---

# Rhythm Pattern List (RC-600)

### Acoustic

| Pattern      | Beat |
| ------------ | ---- |
| SIDE STICK   | 4/4  |
| BOSSA        | 4/4  |
| BRUSH1       | 4/4  |
| BRUSH2       | 4/4  |
| CONGA 8BEAT  | 4/4  |
| CONGA 16BEAT | 4/4  |
| CONGA 4BEAT  | 4/4  |
| CONGA SWING  | 4/4  |
| CONGA BOSSA  | 4/4  |
| CAJON1       | 4/4  |
| CAJON2       | 4/4  |

### Ballad

| Pattern     | Beat |
| ----------- | ---- |
| SHUFFLE2    | 3/4  |
| SIDE STICK1 | 4/4  |
| SIDE STICK2 | 4/4  |
| SIDE STICK3 | 4/4  |
| SIDE STICK4 | 4/4  |
| SHUFFLE1    | 4/4  |
| 8BEAT       | 4/4  |
| 16BEAT1     | 4/4  |
| 16BEAT2     | 4/4  |
| SWING       | 4/4  |
| 6/8 BEAT    | 6/8  |

### Blues

| Pattern  | Beat |
| -------- | ---- |
| 3BEAT    | 3/4  |
| 12BARS   | 4/4  |
| SHUFFLE1 | 4/4  |
| SHUFFLE2 | 4/4  |
| SWING    | 4/4  |
| 6/8 BEAT | 6/8  |

### Jazz

| Pattern     | Beat |
| ----------- | ---- |
| JAZZ BLUES  | 4/4  |
| FAST 4BEAT  | 4/4  |
| HARD BOP    | 4/4  |
| BRUSH BOP   | 4/4  |
| BRUSH SWING | 4/4  |
| FAST SWNG   | 4/4  |
| MED SWING   | 4/4  |
| SLOW LEGATO | 4/4  |
| JAZZ SAMBA  | 4/4  |
| 6/8 BEAT    | 6/8  |

### Fusion

| Pattern            | Beat |
| ------------------ | ---- |
| 16BEAT1 – 16BEAT7 | 4/4  |
| SWING              | 4/4  |
| 7/8 BEAT           | 7/8  |

### R&B

| Pattern                    | Beat |
| -------------------------- | ---- |
| SWING1 – SWING3           | 4/4  |
| SIDE STICK1 – SIDE STICK3 | 4/4  |
| SHUFFLE1 – SHUFFLE2       | 4/4  |
| 8BEAT1                     | 4/4  |
| 16BEAT                     | 4/4  |
| 7/8 BEAT                   | 7/8  |

### Soul

| Pattern              | Beat |
| -------------------- | ---- |
| SWING1 – SWING4     | 4/4  |
| 16BEAT1 – 16BEAT3   | 4/4  |
| SIDESTK1 – SIDESTK2 | 4/4  |
| MOTOWN               | 4/4  |
| PERCUS               | 4/4  |

### Funk

| Pattern            | Beat |
| ------------------ | ---- |
| 8BEAT1 – 8BEAT4   | 4/4  |
| 16BEAT1 – 16BEAT4 | 4/4  |
| SWING1 – SWING3   | 4/4  |

### Pop

| Pattern                    | Beat |
| -------------------------- | ---- |
| 8BEAT1 – 8BEAT2           | 4/4  |
| 16BEAT1 – 16BEAT2         | 4/4  |
| PERCUS1                    | 4/4  |
| SHUFFLE1 – SHUFFLE2       | 4/4  |
| SIDE STICK1 – SIDE STICK2 | 4/4  |
| SWING1 – SWING2           | 4/4  |
| PERCUS2                    | 6/8  |

### Soft Rock

| Pattern                    | Beat |
| -------------------------- | ---- |
| 16BEAT1 – 16BEAT4         | 4/4  |
| 8BEAT                      | 4/4  |
| SWING1 – SWING4           | 4/4  |
| SIDE STICK1 – SIDE STICK2 | 4/4  |
| PERCUS1 – PERCUS2         | 4/4  |

### Rock

| Pattern              | Beat |
| -------------------- | ---- |
| 8BEAT1 – 8BEAT6     | 4/4  |
| 16BEAT1 – 16BEAT4   | 4/4  |
| SHUFFLE1 – SHUFFLE2 | 4/4  |
| SWING1 – SWING4     | 4/4  |

### Alt Rock

| Pattern            | Beat |
| ------------------ | ---- |
| RIDEBEAT           | 4/4  |
| 8BEAT1 – 8BEAT4   | 4/4  |
| 16BEAT1 – 16BEAT4 | 4/4  |
| SWING              | 4/4  |
| 5/4 BEAT           | 5/4  |

### Punk

| Pattern            | Beat |
| ------------------ | ---- |
| 8BEAT1 – 8BEAT6   | 4/4  |
| 16BEAT1 – 16BEAT3 | 4/4  |
| SIDE STICK         | 4/4  |

### Heavy Rock

| Pattern              | Beat |
| -------------------- | ---- |
| 8BEAT1 – 8BEAT3     | 4/4  |
| 16BEAT1 – 16BEAT3   | 4/4  |
| SHUFFLE1 – SHUFFLE2 | 4/4  |
| SWING1 – SWING3     | 4/4  |

### Metal

| Pattern          | Beat |
| ---------------- | ---- |
| 8BEAT1 – 8BEAT6 | 4/4  |
| 2XBD1 – 2XBD5   | 4/4  |

### Trad

| Pattern              | Beat |
| -------------------- | ---- |
| TRAIN2               | 2/4  |
| ROCKN ROLL           | 4/4  |
| TRAIN1               | 4/4  |
| COUNTRY1 – COUNTRY3 | 4/4  |
| FOXTROT              | 4/4  |
| TRAD1 – TRAD2       | 4/4  |

### World

| Pattern                    | Beat |
| -------------------------- | ---- |
| BOSSA1 – BOSSA2           | 4/4  |
| SAMBA1 – SAMBA2           | 4/4  |
| BOOGALOO                   | 4/4  |
| MERENGUE                   | 4/4  |
| REGGAE                     | 4/4  |
| LATIN ROCK1 – LATIN ROCK2 | 4/4  |
| LATIN PERC                 | 4/4  |
| SURDO                      | 4/4  |
| LATIN1 – LATIN2           | 4/4  |

### Ballroom

| Pattern          | Beat |
| ---------------- | ---- |
| CUMBIA           | 2/4  |
| WALTZ1 – WALTZ2 | 3/4  |
| CHACHA           | 4/4  |
| BEGUINE          | 4/4  |
| RHUMBA           | 4/4  |
| TANGO1 – TANGO2 | 4/4  |
| JIVE             | 4/4  |
| CHARLSTON        | 4/4  |

### Electro

| Pattern              | Beat |
| -------------------- | ---- |
| ELCTRO01 – ELCTRO08 | 4/4  |
| 5/4 BEAT             | 5/4  |

### Guide

| Pattern          | Beat                            |
| ---------------- | ------------------------------- |
| 2/4 TRIPLE       | 2/4                             |
| 3/4              | 3/4                             |
| 3/4 TRIPLE       | 3/4                             |
| 4/4              | 4/4                             |
| 4/4 TRIPLE       | 4/4                             |
| BD 8BEAT         | 4/4                             |
| BD 16BEAT        | 4/4                             |
| BD SHUFFLE       | 4/4                             |
| HH 8BEAT         | 4/4                             |
| HH 16BEAT        | 4/4                             |
| HH SWING2        | 4/4                             |
| 8BEAT1 – 8BEAT4 | 4/4                             |
| 5/4              | 5/4                             |
| 5/4 TRIPLE       | 5/4                             |
| 6/4              | 6/4                             |
| 6/4 TRIPLE       | 6/4                             |
| 7/4              | 7/4                             |
| 7/4 TRIPLE       | 7/4                             |
| 5/8 – 15/8      | (corresponding irregular beats) |

### User

| Pattern     | Beat |
| ----------- | ---- |
| SIMPLE BEAT | 4/4  |

## Enumeration of all FX

| #  | Effect name                 | Seq |
| -- | --------------------------- | --- |
| 1  | LPF (Low-pass filter)       | Yes |
| 2  | BPF (Band-pass filter)      | Yes |
| 3  | HPF (High-pass filter)      | Yes |
| 4  | Phaser                      | Yes |
| 5  | Flanger                     | Yes |
| 6  | Synth                       | Yes |
| 7  | Lo-Fi                       | No  |
| 8  | Radio                       | No  |
| 9  | Ring Mod                    | Yes |
| 10 | G2B (Guitar-to-Bass)        | No  |
| 11 | Sustainer                   | No  |
| 12 | Auto Riff                   | No  |
| 13 | Slow Gear                   | No  |
| 14 | Transpose                   | Yes |
| 15 | Pitch Bend                  | Yes |
| 16 | Robot                       | No  |
| 17 | Electric                    | No  |
| 18 | HRM Manual                  | No  |
| 19 | HRM Auto (MIDI)             | No  |
| 20 | Vocoder                     | No  |
| 21 | OSC Voc (MIDI)              | No  |
| 22 | OSC Bot                     | Yes |
| 23 | Preamp                      | No  |
| 24 | Dist (Distortion)           | No  |
| 25 | Dynamics (Comp/Lim presets) | No  |
| 26 | EQ (4-band)                 | No  |
| 27 | Isolator                    | Yes |
| 28 | Octave                      | Yes |
| 29 | Auto Pan                    | No  |
| 30 | Manual Pan                  | Yes |
| 31 | Stereo Enhance              | No  |
| 32 | Tremolo                     | Yes |
| 33 | Vibrato                     | Yes |
| 34 | Pattern Slicer              | No  |
| 35 | Step Slicer                 | No  |
| 36 | Delay                       | No  |
| 37 | Panning Delay               | No  |
| 38 | Reverse Delay               | No  |
| 39 | Mod Delay                   | No  |
| 40 | Tape Echo 1                 | No  |
| 41 | Tape Echo 2                 | No  |
| 42 | Granular Delay              | No  |
| 43 | Warp                        | No  |
| 44 | Twist                       | No  |
| 45 | Roll 1                      | No  |
| 46 | Roll 2                      | No  |
| 47 | Freeze                      | No  |
| 48 | Chorus                      | No  |
| 49 | Reverb                      | No  |
| 50 | Gate Reverb                 | No  |
| 51 | Reverse Reverb              | No  |
| 52 | Beat Scatter (Track-only)   | No  |
| 53 | Beat Repeat (Track-only)    | No  |
| 54 | Beat Shift (Track-only)     | No  |
| 55 | Vinyl Flick (Track-only)    | No  |
