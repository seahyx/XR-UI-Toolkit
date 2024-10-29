# XR UI Toolkit

An convenient UI toolkit for multi-platform XR with examples and scripts to make our lives easier. Uses Unity's XRInteraction package and aims to provide the same convenience as MRTK.

## Installation

### Dependencies

- XR Interaction Toolkit 2.5.4 or higher
  - Starter Assets (under sample tab)
  - (optional) XR Device Simulator (under sample tab)

### Add Package from Git URL

1. Install dependencies.
2. In your Unity Package Manager click the '+' button and Add package from git URL.
3. Git package URL: `https://github.com/seahyx/XR-UI-Toolkit.git?path=/Packages/com.seahyx.xruitoolkit`

### Add Package from Disk

1. Install dependencies.
2. [Download](https://github.com/seahyx/XR-UI-Toolkit/releases) the latest zip package from the release page and extract it into a folder somewhere (e.g. Downloads).
3. In your Unity Package Manager click the '+' button and **Add package from disk**.
4. Navigate to the package folder you extracted and select the `package.json` file.

## Features

### Interactable Visual Effects

Similar to MRTK Interactable's profile system, XR UI Toolkit adds various XR Visual Effects that exist as individual components, taking on various effect presets that will apply visual effects on GameObjects. An XR Visual Effects Controller allows the visual effects to make use of the various callbacks on XRI's BaseInteractable components and make it easy to manage the visual effects.

The default visual effects which are included in this library are:

- Activate Visual Effect
- Animation Trigger Visual Effect
- Colour Visual Effect
- Position Visual Effect
- Rotation Visual Effect
- Scale Visual Effect

New effects can be created to suit your project easily either by extending BaseVisualEffect or BaseAnimatedVisualEffect. For ease of development, it is recommended to use any the default visual effect as a template for creating a new effect.
