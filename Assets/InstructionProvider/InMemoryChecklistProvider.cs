using Assets.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System.Threading;

namespace Assets.InstructionProvider
{
    public class InMemoryChecklistProvider : IChecklistProvider
    {
        Dictionary<string, ChecklistGroup> checklists;
        Dictionary<string, ChecklistGroup> IChecklistProvider.GetChecklists()
        {
            return checklists;
        }

        bool ready = false;

        IEnumerator IChecklistProvider.signalDownload()
        {
            checklists = new Dictionary<string, ChecklistGroup>();

            { 
                ChecklistGroup cessna172rg = new ChecklistGroup();

                InstructionDTO takeoff = new InstructionDTO("Take-OFF", "Take-OFF");
                takeoff.addTask("Flaps", "0 degrees");
                takeoff.addTask("Carb h.", "OFF");
                takeoff.addTask("LDG Light", "ON");
                takeoff.addTask("Flight controls", "FREE");
                takeoff.addTask("Power", "FULL / 2700 RPM");
                takeoff.addTask("Rotate", "at 55 KIAS");
                takeoff.addTask("Landing gear", "RETRACT 50 ft");
                takeoff.addTask("AAL", "at 400 ft");
                takeoff.addTask("Flaps", "0 degrees");
                takeoff.addTask("Power", "25 / 2500");
                takeoff.addTask("LDG Light", "OFF");

                takeoff.id = 1234141;

                

                InstructionDTO pretakeoff = new InstructionDTO("Before Take-OFF", "BEFORE Take-OFF");
                pretakeoff.addTask("Parking brake", "SET");
                pretakeoff.addTask("Altimeters", "SET");
                pretakeoff.addTask("Trims", "T / O");
                pretakeoff.addTask("Flight controls", "FREE");
                pretakeoff.addTask("Fuel selector", "BOTH");
                pretakeoff.addTask("Throttle", "1800 RPM");
                pretakeoff.addTask("Mixture", "RICH");
                pretakeoff.addTask("L & R Magnetos drop", "150 MAX / 50 DIFF");
                pretakeoff.addTask("Propeller", "CYCLE LOW - HIGH");
                pretakeoff.addTask("Carb h.", "CHECK DROP");
                pretakeoff.addTask("Engine instruments", "CHECK");
                pretakeoff.addTask("Suction", "CHECK");
                pretakeoff.addTask("Throttle", "1000 RPM");
                pretakeoff.addTask("Radio", "SET");
                pretakeoff.addTask("Parking brake", "OFF");
                pretakeoff.addTask("T/O clearance", "OBTAIN");
                pretakeoff.id = 15124981;
            
                

                InstructionDTO descent = new InstructionDTO("DESCENT", "DESCENT");

                descent.addTask("Fuel selector", "BOTH");
                descent.addTask("Power", "AS REQ.");
                descent.addTask("Carb h.", "ON");
                descent.addTask("Mixture", "ENRICH");
                descent.addTask("Cowl flaps", "CLOSED");
                descent.addTask("Flaps", "AS REQ.");
                descent.addTask("Landing gear", "AS REQ.");

                descent.id = 1234142;

                

                InstructionDTO beforeLanding = new InstructionDTO("BEFORE LANDING", "BEFORE LANDING");

                beforeLanding.addTask("Fuel selector", "BOTH");
                beforeLanding.addTask("LDG Light", "ON");
                beforeLanding.addTask("Landing gear", "GREEN");
                beforeLanding.addTask("Mixture", "RICH");
                beforeLanding.addTask("Propeller", "HIGH RPM");
                beforeLanding.addTask("Carb h.", "ON");
                beforeLanding.addTask("Flaps", "AS REQ.");
                beforeLanding.addTask("Power", "AS REQ.");
                beforeLanding.addTask("Carb h.", "OFF BEFORE LDG");

                beforeLanding.id = 1234143;

                
                InstructionDTO afterLanding = new InstructionDTO("AFTER LANDING", "AFTER LANDING");

                afterLanding.addTask("Flaps", "0°");
                afterLanding.addTask("Carb h.", "OFF");
                afterLanding.addTask("LDG Light", "OFF");
                afterLanding.addTask("Cowl flaps", "OPEN");

                afterLanding.addTask("Parking brake", "SET");
                afterLanding.addTask("Lights", "OFF");
                afterLanding.addTask("Throttle", "1000 RPM");
                afterLanding.addTask("Avionics", "OFF");
                afterLanding.addTask("Mixture", "IDLE - CUT OFF");
                afterLanding.addTask("Throttle", "CLOSE");
                afterLanding.addTask("Ignition", "OFF");
                afterLanding.addTask("Master", "OFF");
                afterLanding.addTask("Fuel selector", "OFF");

                afterLanding.id = 1234144; // Unique ID

                InstructionDTO engineStartup = new InstructionDTO("ENGINE START-UP", "ENGINE START-UP");
                engineStartup.id = 1234145; // Unique ID
                engineStartup.addTask("Mixture", "RICH");
                engineStartup.addTask("Propeller", "HIGH RPM");
                engineStartup.addTask("Carb h.", "OFF");
                engineStartup.addTask("Throttle", "1 \"");
                engineStartup.addTask("Master", "ON");
                engineStartup.addTask("Beacon", "ON");
                engineStartup.addTask("Prop aera", "CLEAR");
                engineStartup.addTask("Primer", "APPLY");
                engineStartup.addTask("Ignition", "START");
                engineStartup.addTask("Oil press.", "CHECK");
                engineStartup.addTask("Avionics master", "ON");
                engineStartup.addTask("Radio", "ON");
                engineStartup.addTask("XPDR", "STB / ALT");

               

                cessna172rg.instructions.Add(engineStartup.id, engineStartup);


                cessna172rg.instructions.Add(pretakeoff.id, pretakeoff);
                cessna172rg.instructions.Add(takeoff.id, takeoff);
                cessna172rg.instructions.Add(descent.id, descent);
                cessna172rg.instructions.Add(beforeLanding.id, beforeLanding);
                cessna172rg.instructions.Add(afterLanding.id, afterLanding);


                checklists.Add("cessna172rg", cessna172rg);




            }
            {
                ChecklistGroup acar = new ChecklistGroup();
                InstructionDTO startingUp = new InstructionDTO("Start Engine", "Start Engine");
                startingUp.addTask("All doors", "CLOSED");
                startingUp.addTask("Seatbelt", "Fastened");
                startingUp.addTask("Brake Pedal", "PRESSED");
                startingUp.addTask("Transmission", "SET TO P");
                startingUp.addTask("Start/Stop central console button", "HOLD TILL RPM 1400");
                startingUp.addTask("Start/Stop central console button", "RELEASED");
                startingUp.id = 4124123;

                acar.instructions.Add(startingUp.id, startingUp);

                InstructionDTO shuttingDown = new InstructionDTO("Stop Engine", "Stop Engine");
                shuttingDown.addTask("Brake Pedal", "PRESSED");
                shuttingDown.addTask("Transmission", "SET TO P");
                shuttingDown.addTask("Start/Stop central console button", "HOLD TILL RPM 0");
                shuttingDown.addTask("Start/Stop central console button", "RELEASED");
                shuttingDown.id = 4124124;

                acar.instructions.Add(shuttingDown.id, shuttingDown);
                checklists.Add("Generic Car", acar);
            }
            reandomth();

            yield return null;
        }

        public void reandomth() {
            Thread tj = new Thread(() =>
            {
                Thread.Sleep(3000);
                ready = true;
            });

            tj.Start(); 
        }

        public bool isReady()  { 
        
            return ready;
        }

    }
}
