using UnityEngine;

public class PopulateTheList : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*var checklist =  CurrentInstructionSingleton.Instance.instructionDTO;
         var lpc = GetComponent<ListPanel>();
         bool isFirst = true;
         foreach (var task in checklist.tasks) {

             string current = "  " + task.shortTitle;
             if (isFirst) {
                  current = "> " + task.shortTitle;
                 isFirst = false;
             }

             string fixedPart;
             if (current.Length >= 25)
             {
                 fixedPart = current.Substring(0, 21) + "... "; // clamp to 24 chars, then add 1 space
             }
             else
             {
                 int paddingNeeded = 25 - current.Length;
                 string spaces = new string(' ', paddingNeeded);
                 fixedPart = current + spaces;
             }

             string result = fixedPart + task.description;

             lpc.addOption(result);
         }
         lpc.setTitle(checklist.shortTitle);*/
        //recreateListPanel();

        CurrentInstructionSingleton.Instance.setListPanel(recreateListPanel());
    }

    ListPanel recreateListPanel() {
        var checklist = CurrentInstructionSingleton.Instance.instructionDTO;
        var lpc = GetComponent<ListPanel>();
        //lpc.clear();

        bool isFirst = true;
        foreach (var task in checklist.tasks)
        {

            string current = "  " + task.shortTitle;
            if (isFirst)
            {
                current = "> " + task.shortTitle;
                isFirst = false;
            }

            string fixedPart;
            if (current.Length >= 25)
            {
                fixedPart = current.Substring(0, 21) + "... "; // clamp to 24 chars, then add 1 space
            }
            else
            {
                int paddingNeeded = 25 - current.Length;
                string spaces = new string(' ', paddingNeeded);
                fixedPart = current + spaces;
            }

            string result = fixedPart + task.description;

            lpc.addOption(result);
        }
        lpc.setTitle(checklist.shortTitle);
        return lpc;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
