using Assets.model;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsRepository2 : MonoBehaviour
{
    public List<InstructionDTO> instructionDTOs = new List<InstructionDTO>();

    public InstructionsRepository2()
    {
        // Main instruction set - Aviation Checklist  
        InstructionDTO mainInstructions = new InstructionDTO("Aviation¹æ Procedures", "Pre-flight and Flight Operations");

        // Add pre-flight tasks  
        mainInstructions.tasks.Add(new TaskDTO("1", "Pre-Flight Inspection", "Conduct a thorough external and internal check"));
        mainInstructions.tasks.Add(new TaskDTO("2", "Cockpit Preparation", "Set up instruments, radios, and navigation systems"));

        // Add engine and systems tasks  
        TaskDTO engineTask = new TaskDTO("3", "Engine & Systems Check", "Verify proper functioning of aircraft systems");
        engineTask.subTasks.Add(new TaskDTO("3.1", "Start the Aircraft", "Follow startup sequence: fuel pump, ignition, throttle"));
        engineTask.subTasks.Add(new TaskDTO("3.2", "Check Fuel Levels", "Verify quantity, crossfeed, and no contaminants"));
        engineTask.subTasks.Add(new TaskDTO("3.3", "Test Avionics", "Confirm radios, GPS, and transponder operation"));

        // Add structural inspection task  
        TaskDTO structuralTask = new TaskDTO("4", "Structural Inspection", "Ensure airframe integrity before flight");
        structuralTask.subTasks.Add(new TaskDTO("4.1", "Check Wing Surfaces", "Inspect flaps, ailerons, and leading edges for damage"));
        structuralTask.subTasks.Add(new TaskDTO("4.2", "Examine Landing Gear", "Check tires, struts, and hydraulic systems"));
        structuralTask.subTasks.Add(new TaskDTO("4.3", "Verify Control Surfaces", "Test rudder, elevators, and trim functionality"));

        // Add to main instructions  
        mainInstructions.tasks.Add(engineTask);
        mainInstructions.tasks.Add(structuralTask);

        // Final checks  
        mainInstructions.tasks.Add(new TaskDTO("5", "Final Safety Review", "Confirm checklist completion and ATC clearance"));

        instructionDTOs.Add(mainInstructions);
    }
}