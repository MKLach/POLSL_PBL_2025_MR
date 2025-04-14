using Assets.model;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsRepository : MonoBehaviour
{
    private List<InstructionDTO> instructionDTOs = new List<InstructionDTO>();

    public void initv1()
    {
        // Main instruction set - Aviation Checklist  
        InstructionDTO mainInstruction = new InstructionDTO("Take off", "How to take off");

        mainInstruction.tasks.Add(new TaskDTO(
            "Requirements",
            "Pre-takeoff requirements",
            "Ensure that the seatbelt is fastened, the engine is running properly, and the aircraft is positioned on the takeoff strip."
        ));

        mainInstruction.tasks.Add(new TaskDTO(
            "Set flaps to neutral position",
            "Adjust wing flaps before takeoff",
            "Ensure that the flaps are set to the takeoff/neutral position as per aircraft specifications."
        ));

        mainInstruction.tasks.Add(new TaskDTO(
            "Adjust throttle - 50%",
            "Initial throttle setting",
            "Set throttle to 50% to begin taxiing and perform final engine checks before full takeoff power."
        ));

        mainInstruction.tasks.Add(new TaskDTO(
            "Elevation up",
            "Begin aircraft lift-off",
            "Gently pull back on the yoke or stick when reaching takeoff speed to raise the nose and begin ascent."
        ));

        mainInstruction.tasks.Add(new TaskDTO(
            "Set throttle to 100%",
            "Full throttle for takeoff",
            "Push the throttle fully forward to provide maximum power for takeoff."
        ));

        mainInstruction.tasks.Add(new TaskDTO(
            "Elevation up",
            "Lift the aircraft off the ground",
            "Once takeoff speed is reached, gently pull back on the control to initiate climb."
        ));

        mainInstruction.tasks.Add(new TaskDTO(
            "Retract landing gear",
            "Gear control after lift-off",
            "Once a stable climb is established and you're clear of the runway, retract the landing gear to reduce drag."
        ));


        InstructionDTO mainInstruction1 = new InstructionDTO("Landing", "How to land");
        mainInstruction1.tasks.Add(new TaskDTO(
            "Requirements",
            "Pre-takeoff requirements",
            "Ensure that the seatbelt is fastened, the engine is running properly, and the aircraft is positioned on the takeoff strip."
        ));

        mainInstruction1.tasks.Add(new TaskDTO(
            "Set flaps to neutral position",
            "Adjust wing flaps before takeoff",
            "Ensure that the flaps are set to the takeoff/neutral position as per aircraft specifications."
        ));


        InstructionDTO mainInstruction2 = new InstructionDTO("Barrell roll", "How to perform barrel roll");
        mainInstruction2.tasks.Add(new TaskDTO(
           "Requirements",
           "Roll requirements",
           "Ensure that the seatbelt is fastened!"
       ));

        mainInstruction2.tasks.Add(new TaskDTO(
            "Set flaps to neutral position",
            "Adjust wing flaps before takeoff",
            "Ensure that the flaps are set to the takeoff/neutral position as per aircraft specifications."
        ));


        InstructionDTO mainInstruction3 = new InstructionDTO("Immelmann loop", "How to perform immelmann loop");
        mainInstruction3.tasks.Add(new TaskDTO(
         "Requirements",
         "Roll requirements",
         "Ensure that the seatbelt is fastened!"
        ));

        mainInstruction3.tasks.Add(new TaskDTO(
            "Set flaps to neutral position",
            "Adjust wing flaps before takeoff",
            "Ensure that the flaps are set to the takeoff/neutral position as per aircraft specifications."
        ));


        InstructionDTO mainInstruction4 = new InstructionDTO("Seatbelt usage", "How to use seatbelt");
        mainInstruction4.tasks.Add(new TaskDTO(
         "Requirements",
         "Seatbelt requirements",
         "Ensure that the seatbelt is fastened???"
        ));

        mainInstruction4.tasks.Add(new TaskDTO(
            "Set flaps to neutral position",
            "Adjust wing flaps before takeoff",
            "Ensure that the flaps are set to the takeoff/neutral position as per aircraft specifications."
        ));



        instructionDTOs.Add(mainInstruction);
        instructionDTOs.Add(mainInstruction1);
        instructionDTOs.Add(mainInstruction2);
        instructionDTOs.Add(mainInstruction3);
        instructionDTOs.Add(mainInstruction4);
    }

    public List<InstructionDTO> get() {
        return instructionDTOs;
    }
}