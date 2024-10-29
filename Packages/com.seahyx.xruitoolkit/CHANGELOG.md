# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
