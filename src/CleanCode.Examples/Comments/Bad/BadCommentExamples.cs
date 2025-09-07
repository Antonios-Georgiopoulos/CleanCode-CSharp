namespace CleanCode.Examples.Comments.Bad;

public class BadCommentExamples
{
    // BAD: Obvious comment that adds no value
    public int Add(int a, int b)
    {
        // Add a and b together
        return a + b;
    }

    // BAD: Redundant comment
    public void IncrementCounter()
    {
        // Increment the counter by 1
        counter++;
    }

    // BAD: Misleading comment - doesn't match the code
    // BAD: Misleading comment - doesn't match the code
    public decimal CalculateDiscount(decimal price)
    {
        // Apply 10% discount
        return price - (price * 0.15m); // Actually applies 15%!
    }

    // BAD: Commented out code
    public void ProcessOrder()
    {
        ValidateOrder();
        // CalculateShipping(); // Old shipping calculation
        // SendEmail();          // Email sending removed
        // LogOrder();           // Logging disabled for now
        SaveOrder();

        // TODO: Fix this later
        // var result = SomeComplexCalculation();
        // if (result > 100) 
        // {
        //     DoSomething();
        // }
    }

    // BAD: Noise comments that state the obvious
    public class Customer
    {
        // Customer name
        public string Name { get; set; }

        // Customer email address
        public string Email { get; set; }

        // Customer age in years
        public int Age { get; set; }

        // Boolean flag indicating if customer is active
        public bool IsActive { get; set; }
    }

    // BAD: Comment trying to explain bad code instead of fixing it
    public void ProcessData(List<object> data)
    {
        // This is a complex algorithm that processes data
        // First we iterate through all items
        // Then we check if each item is valid
        // If valid, we process it differently based on type
        // This might be confusing but it works
        foreach (var item in data)
        {
            // Check if item is not null (important!)
            if (item != null)
            {
                // Determine type and process accordingly
                if (item.GetType() == typeof(string))
                {
                    // String processing logic
                    var str = (string)item;
                    // Make sure string is not empty
                    if (!string.IsNullOrEmpty(str))
                    {
                        // Process the string
                        ProcessString(str);
                    }
                }
                else if (item.GetType() == typeof(int))
                {
                    // Integer processing logic
                    var num = (int)item;
                    // Check if number is positive
                    if (num > 0)
                    {
                        // Process the number
                        ProcessNumber(num);
                    }
                }
                // Add more type checks as needed
            }
        }
    }

    // BAD: Outdated comment that's no longer accurate
    public void CalculateTotal()
    {
        // Uses the old pricing model from 2020
        // This method calculates total with tax included
        var subtotal = GetSubtotal();
        var tax = subtotal * 0.08m; // 8% tax rate (changed to 10% in 2023)
        var total = subtotal + tax;

        // Apply senior discount if applicable (removed this feature)
        // Save to database (now we save to cloud)
    }

    // BAD: Rambling comment that explains too much
    /// <summary>
    /// This method is responsible for validating user input data
    /// It takes a string parameter which represents the user's input
    /// The validation process involves several steps:
    /// 1. Check if the input is null or empty
    /// 2. Check if the input length is within acceptable bounds
    /// 3. Check if the input contains only valid characters
    /// 4. Check if the input matches expected patterns
    /// 5. Perform additional business rule validations
    /// The method returns true if all validations pass
    /// Otherwise it returns false
    /// Note: This method was created by John on 2023-01-15
    /// Modified by Sarah on 2023-02-20 to add length validation
    /// Modified by Mike on 2023-03-10 to add pattern matching
    /// </summary>
    /// <param name="input">The user input string to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public bool ValidateInput(string input)
    {
        return !string.IsNullOrEmpty(input) && input.Length <= 100;
    }

    // BAD: Comment expressing frustration or unprofessional remarks
    public void ProcessPayment()
    {
        // This payment API is terrible and always breaks
        // TODO: Replace this garbage with something that actually works
        // The documentation is useless and the support team is incompetent

        try
        {
            // Hack to make this work - don't ask me why
            System.Threading.Thread.Sleep(1000);

            // Another stupid workaround for their broken API
            var result = CallPaymentAPI();

            // This will probably fail but whatever
            if (result != null)
            {
                // Success! (surprisingly)
                ProcessSuccess();
            }
        }
        catch
        {
            // Of course it failed, it always does
            // Just ignore the error and hope for the best
        }
    }

    // BAD: HTML or formatting in regular comments
    public void GenerateReport()
    {
        /*
         * <h1>Report Generation Process</h1>
         * <p>This method generates a comprehensive report</p>
         * <ul>
         *   <li>Collects data from database</li>
         *   <li>Processes the data</li>
         *   <li>Formats the output</li>
         * </ul>
         */

        // <b>Important:</b> Make sure to call this method only after validation
        CollectData();
        ProcessData();
        FormatOutput();
    }

    // BAD: Legal disclaimers in code comments
    public void SaveUserData()
    {
        /*
         * LEGAL NOTICE: This software is proprietary and confidential.
         * Any unauthorized use, reproduction, or distribution is strictly prohibited.
         * Copyright (c) 2023 Company Name. All rights reserved.
         * Patent pending. Trademark of Company Name.
         * For licensing inquiries contact legal@company.com
         */

        // Implementation here
    }

    // BAD: Version control information in comments
    public void UpdateUser()
    {
        // Version: 1.2.3
        // Last modified: 2023-12-01
        // Author: John Doe
        // Revision: r12345
        // Branch: feature/user-updates
        // Commit: abc123def456

        // Implementation here
    }

    // BAD: Commented out alternative implementations
    public int Calculate(int x, int y)
    {
        return x * y;

        // Old implementation:
        // var result = 0;
        // for (int i = 0; i < y; i++)
        // {
        //     result += x;
        // }
        // return result;

        // Alternative approach:
        // return Math.Pow(x, Math.Log(y));

        // Recursive version:
        // return y == 0 ? 0 : x + Calculate(x, y - 1);
    }

    // Private fields and methods for compilation
    private int counter = 0;
    private void ValidateOrder() { }
    private void SaveOrder() { }
    private void ProcessString(string str) { }
    private void ProcessNumber(int num) { }
    private decimal GetSubtotal() => 100m;
    private object CallPaymentAPI() => new object();
    private void ProcessSuccess() { }
    private void CollectData() { }
    private void ProcessData() { }
    private void FormatOutput() { }
}