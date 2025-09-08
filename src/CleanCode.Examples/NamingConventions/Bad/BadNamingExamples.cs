namespace CleanCode.Examples.NamingConventions.Bad;

// BAD: Cryptic class name
public class Calc
{
    // BAD: Single-letter & abbreviations
    public double a, b;
    public string usr = string.Empty, pwd = string.Empty;

    // BAD: Hungarian notation
    public int iCount;
    public string strName = string.Empty;
    public bool bIsActive;

    public void DoStuff()
    {
        int x = 10;            // What is x?
        var temp = GetData();  // temp? data?

        // BAD: Magic numbers & vague method/params
        if (x > 5)
        {
            Process(temp, 1);
        }
    }

    private object GetData() => "some data"; // What data?
    private void Process(object data, int flag) { } // What does flag mean?

    // BAD: Inconsistent casing
    public string userName = string.Empty;
    public string user_email = string.Empty;
    public string UserAddress = string.Empty;
}