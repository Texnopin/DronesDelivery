Drone and Resource Management System

This project implements a drone and resource management system using Unity. Below are the key features and architectural details of the system.


![image](https://github.com/user-attachments/assets/f57e83e1-a5fa-404c-9969-8a4d84bae4a7)



Features Implemented

Drone Spawning and Control:

Drones are divided into two factions (red and blue).

Spawn points and bases are defined for each faction.

Drone count and speed are adjustable via UI sliders.

Drones can collect resources and deliver them back to their respective bases.

Path visibility is toggleable via a UI element.

Resource Management:

Resources spawn periodically within a defined area.

The spawn rate is adjustable via a UI slider.

Drones can claim and collect resources.

Monitor Displays:

Cameras on drones display their views on separate monitors (cubes in the scene).

Up to 10 monitors are supported, dynamically linked to drones.

System Architecture

Scripts Overview

Drone.cs:

Controls individual drone behavior, including movement, resource collection, and base return.

Updates drone material based on faction.

Handles path rendering and interactions with resources.

DroneSpawner.cs:

Manages spawning and despawning of drones for both factions.

Updates drone speed, path visibility, and resource collection events.

Links drone cameras to render textures displayed on monitors.

ResourceSpawner.cs:

Handles periodic spawning of resources within a predefined area.

Allows the spawn rate to be adjusted dynamically via a slider.

ResourceTracker.cs:

Tracks which resources are claimed by drones to avoid conflicts.

Provides methods to claim and release resources.

Tools and Techniques Used

Unity Components:

NavMeshAgent for drone movement.

LineRenderer for visualizing drone paths.

UI sliders and toggles for dynamic configuration.

RenderTextures:

Used to display drone camera views on monitors.

Event-driven Programming:

Resource collection events trigger UI updates.

How to Use

Adjust the number of drones and their speed using the respective sliders.

Toggle path visibility for drones via the toggle switch.

Use the spawn interval slider to control resource spawning frequency.

Observe drone activities on the monitors in the scene.
