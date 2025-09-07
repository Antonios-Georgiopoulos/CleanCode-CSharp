namespace CleanCode.Examples.NamingConventions.Bad;

// BAD: Cryptic class name
public class Calc
{
    // BAD: Single letter variables
    public double a, b, r;

    // BAD: Abbreviations and unclear names
    public string usr, pwd, db;

    // BAD: Hungarian notation
    public int iCount;
    public string strName;
    public bool bIsActive;

    // BAD: Meaningless method names
    public void DoStuff()
    {
        // BAD: What does 'x' represent?
        int x = 10;

        // BAD: What is 'temp'?
        var temp = GetData();

        // BAD: Magic numbers without explanation
        if (x > 5)
        {
            ProcessData(temp, 42);
        }
    }

    // BAD: What does this method do?
    private void ProcessData(object data, int value)
    {
        // Some processing
    }

    // BAD: Unclear method name and parameters
    public void Process(object data, int flag)
    {
        // Implementation
    }

    // BAD: What does this method return?
    public object GetData()
    {
        return "some data";
    }

    // BAD: Noise words
    public void ProcessDataInfo()
    {
        // Implementation
    }

    // BAD: Inconsistent naming
    public string userName;
    public string user_email;
    public string UserAddress;
}