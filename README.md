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

Similar to MRTK Interactable's profile system, XR UI Toolkit adds various XR Visual Effects that exist as individual components, taking on various effect presets that will apply visual changes based on currently active interactable states on GameObjects. A XR Visual Effects Controller allows the visual effects to make use of the various callbacks on XRI's BaseInteractable components and make it easy to manage the visual effects, and can be extended to fit other interaction systems.

There is also a controller that works with Unity UI components as well.

The default visual effects which are included in this library are:

- Activate Visual Effect
- Animation Trigger Visual Effect
- Material Colour Visual Effect
- Image Colour Visual Effect
- Sprite Colour Visual Effect
- Position Visual Effect
- Rect Position Visual Effect
- Rotation Visual Effect
- Scale Visual Effect
- Text Visual Effect
- Sprite Visual Effect

New effects can be created to suit your project easily either by extending `BaseVisualEffect` or `BaseAnimatedVisualEffect`. For ease of development, it is recommended to use any the default visual effect (e.g. `ActivateVisualEffect`) as a template for creating a new effect.

### State Priority List

Each visual effect preset has its own state priority list to adjust or enable/disable effect interactions with the interactable states. Priorities for each interactable state is rank top to bottom, from highest to lowest priority, repectively.

Effects from a higher priority state will overwrite the effects from a lower priority state when both are active at the same time. A disabled effect state will not trigger any effects.

This allows you to fine-tune interaction feedback, such as prioritising the focus state effects over the hover state and vice versa.

## Development

This repository contains an entire Unity project folder which serves as the testbed for the actual package, which is located in `\Packages\com.seahyx.xruitoolkit` (and thus the `?path=/Packages/com.seahyx.xruitoolkit` when installing via git).

When opening up the project in Unity, development is done by modifying the package folder under the Packages directory. Any scripts or assets in the folder can just be opened in Visual Studio/Code and modified directly.

### Uploading New Changes

When commiting a new change,

1. Update the package version in `package.json`,
2. Update the changelog,
3. Zip the package folder (the one that includes `package.json`), and create a new release with the new zip file.

If there are any changes to the README, update both READMEs in the repository root and package root directories.
