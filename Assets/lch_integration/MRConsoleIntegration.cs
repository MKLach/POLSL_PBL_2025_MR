

using lch.com.mkl.lch.variable;
using TMPro;

public class MRConsoleIntegration {
    public TextMeshProUGUI console;


    public MRConsoleIntegration(TextMeshProUGUI c)
    {

        console = c;
    }

    public void println(Variable msg) {

        console.text += msg.getAsString() + "\n";
    }
    public void printspace()
    {

        console.text += " ";
    }

    public void print(Variable msg)
    {

        console.text += msg.getAsString();
    }


}