# Wall Picture Placement – Unity Demo

This project demonstrates a modular wall-picture placement interaction built in Unity, designed with XR use cases in mind. The system uses input abstraction to simulate hand-like interaction, snaps the picture to arbitrarily oriented wall surfaces using surface normals, maintains a fixed offset to prevent wall intersection, and constrains movement to the wall plane once snapped.

The implementation emphasizes clear separation between input handling, interaction logic, and spatial snapping, making it easily extensible to XR hand tracking or controller-based input.

## Controls

- **Left Mouse Button (Hold):** Grab picture  
- **Mouse Move:** Move picture  
- **Release Mouse Button:** Release picture  

When the picture is close to a wall and its rotation aligns with the wall normal, it snaps automatically and can be slid along the wall surface.


## Architecture Overview

### Input Abstraction
- `IInputProvider` defines grab and pointer position input.
- `MouseInputProvider` implements mouse-based input and can be replaced with XR hand or controller input.

### Interaction Logic
- `PictureInteractor` manages grab/release state and delegates placement behavior.
- Contains no snapping or spatial logic.

### Snapping & Spatial Logic
- `SnappingSystem` handles wall detection, snapping conditions, rotation alignment, offset enforcement, and plane-constrained sliding.
- Wall orientation is derived dynamically from surface normals, allowing support for arbitrarily oriented walls.

This separation keeps the system modular, extensible, and suitable for XR interaction scenarios.
