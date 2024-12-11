# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 1.5.4

### Fixed

- Fixed error in `TextVFX` during initialization

## 1.5.3

### Changed

- Updated `TextVFX` to check for TextMeshProUGUI

## 1.5.2

### Added

- `AudioClipVisualEffect` to play audio oneshot on state change (not really visual, is it?)

## 1.5.1

### Changed

- Changed "Create New Visual Effect" dropdown to use `GenericMenu` implementation
- Updated the visual effect runtimes to be grouped by type
- Updated the filename of visual effect runtimes to be consistent when created via dropdown or asset creation menu

## 1.5.0

### Added

- `SpriteImageColourVisualEffect` that can change colour on both SpriteRenderer or UGUI Image component on the target
- `ImageSwapVisualEffect` that can change swap sprites on a UGUI Image component on the target
- `SpriteImageSwapVisualEffect` that can change swap sprites on both SpriteRenderer or UGUI Image component on the target

### Changed

- Renamed `SpriteVisualEffect` to `SpriteSwapVisualEffect`
- Refactored all "Visual Effect" strings to "VFX" in asset creation menu and filenames during creation for brevity
- Updated package links

## 1.4.1

### Added

- `UnityUIVisualEffectsController` now detects for Toggle components and uses the Activated visual effect state to represent the toggle "on" state

## 1.4.0

### Added

- Added new `ImageColourVisualEffect`
- Added new `SpriteColourVisualEffect`

### Changed

- Changed all interpolation methods to unclamped
- Renamed `ColourVisualEffect` to `MaterialColourVisualEffect`

## 1.3.0

### Added

- Added new `RectPositionVisualEffect` for `RectTransform`s
- Added new `TextVisualEffect`
- Added new `SpriteVisualEffect`

## 1.2.0

### Added

- Support for Visual Effects on UGUI elements via `UnityUIVisualEffectsController`

## 1.1.1

- Fixed broken import
- Removed unnecessary usings

## 1.1.0

### Added

- Various editor UI improvements (Refresh controller effect list, reset state priority list)
- Added new "Disabled" state for when the controller is disabled
- Added "Deactivate On Deselect" toggle (default on) to fix visual effect being stuck in Activated state if the interactable is deselected before it is deactivated
- Added "isEnabled" toggle for individual visual effects to disable each effect by itself
- Added new information on README

### Changed

- Refactored BaseVisualEffect and XRVisualEffect to decouple from XRBaseInteractable
- Refactored XRVisualEffectsController to abstract BaseVisualEffectsController and moved Interactable event bindings to XRInteractableVisualEffectsController

## 1.0.1

### Changed

- Restructured project into Runtime and Editor folders
- Renamed package folder to the proper package identifier name

## 1.0.0

### Added

- Package manifest and pre-existing code
- XR Interaction Toolkit 2.5.4 to packages
- Initial code release
