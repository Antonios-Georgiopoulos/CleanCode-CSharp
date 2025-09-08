namespace CleanCode.Examples.CodeFormatting.Bad;

// BAD: Inconsistent indentation and spacing
public class BadFormattingExamples
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }

    // BAD: No spacing around operators and inconsistent brace placement
    public void ProcessData(List<string> data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        for (int i = 0; i < data.Count; i++)
        {
            var item = data[i];
            if (item != null && item.Length > 0)
            {
                Console.WriteLine($"Processing: {item}");
            }
        }
    }

    // BAD: Long lines that are hard to read
    public void ProcessUserOrderWithComplexBusinessLogicAndMultipleValidationsAndCalculations(string userName, string userEmail, List<OrderItem> orderItems, PaymentMethod paymentMethod, ShippingAddress shippingAddress, BillingAddress billingAddress, DiscountCoupon discountCoupon, TaxCalculationRules taxRules)
    {
        if (userName != null && userEmail != null && orderItems != null && orderItems.Count > 0 && paymentMethod != null && shippingAddress != null && billingAddress != null)
        {
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity); var discountAmount = discountCoupon?.IsValid == true ? subtotal * discountCoupon.DiscountPercentage : 0; var taxAmount = taxRules.CalculateTax(subtotal - discountAmount, shippingAddress.State); var total = subtotal - discountAmount + taxAmount;
        }
    }

    // BAD: Inconsistent method spacing and random blank lines


    public void Method1()
    {


        Console.WriteLine("Method 1");


    }
    public void Method2()
    {
        Console.WriteLine("Method 2");
    }

    public void Method3()
    {
        Console.WriteLine("Method 3");
    }

    // BAD: Poor variable alignment and inconsistent formatting
    public void ConfigureSettings()
    {
        var setting1 = "Value1";
        var anotherSetting = "Value2";
        var setting3 = "Value3";
        var veryLongSettingName = "Value4";

        var x = 10;
        var y = 20;
        var result = x + y;
    }

    // BAD: Nested code without proper indentation
    public void ProcessNestedData()
    {
        var users = GetUsers();
        foreach (var user in users)
        {
            var orders = GetUserOrders(user.Id);
            foreach (var order in orders)
            {
                var items = GetOrderItems(order.Id);
                foreach (var item in items)
                {
                    if (item.IsValid)
                    {
                        var price = CalculatePrice(item);
                        if (price > 0)
                        {
                            var discount = CalculateDiscount(item, user);
                            if (discount > 0)
                            {
                                var finalPrice = price - discount;
                                SavePrice(item.Id, finalPrice);
                            }
                        }
                    }
                }
            }
        }
    }

    // BAD: Inconsistent commenting style
    public void BadCommentFormatting()
    {
        //This is a comment without space
        // This is a comment with space
        //This is a poorly aligned comment
        /*This is a block comment
        without proper formatting*/

        /* This is another
           block comment with
           inconsistent spacing */

        var x = 5; //inline comment without space
        var y = 10;  // inline comment with inconsistent spacing
    }

    // BAD: Mixed naming conventions and inconsistent casing
    public class mixed_naming_Conventions
    {
        public string firstName;
        public string LastName;
        public string email_address;
        public int AGE;
        public bool is_active;

        public void Process_Data() { }
        public void processUser() { }
        public void CALCULATE_TOTAL() { }
    }

    // BAD: No grouping of related members
    public class PoorMemberOrganization
    {
        public string Name { get; set; }

        public void ProcessOrder() { }

        public int Age { get; set; }

        public void CalculateTotal() { }

        public bool IsActive { get; set; }

        public void ValidateUser() { }

        private string secretKey;

        public string Email { get; set; }

        private void InternalMethod() { }
    }

    // BAD: Inconsistent string formatting
    public void BadStringFormatting()
    {
        var name = "John";
        var age = 30;
        var email = "john@example.com";

        // Mixed string concatenation and interpolation
        var message1 = "Hello " + name + ", you are " + age.ToString() + " years old";
        var message2 = $"Hello {name}";
        var message3 = string.Format("Your email is {0}", email);
        var message4 = "Welcome, " + name + "! Your email: " + email;
    }

    // BAD: Poor LINQ formatting
    public void BadLinqFormatting()
    {
        var users = GetUsers();

        var result1 = users.Where(u => u.Age > 18).Select(u => u.Name).OrderBy(n => n).ToList();

        var result2 = users
        .Where(u => u.IsActive)
        .Where(u => u.Age > 21)
        .Select(u => new { u.Name, u.Email }).OrderBy(u => u.Name)
        .ToList();

        var result3 = users.Where(u =>
        u.IsActive &&
        u.Age > 18 &&
        !string.IsNullOrEmpty(u.Email)).Select(u => u).ToList();
    }

    // BAD: Inconsistent exception handling formatting
    public void BadExceptionFormatting()
    {
        try
        {
            var data = GetData();
            ProcessData(data);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine("Null argument: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Invalid operation: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
        finally
        {
            Cleanup();
        }
    }

    // BAD: Poor switch statement formatting
    public string BadSwitchFormatting(int value)
    {
        switch (value)
        {
            case 1: return "One";
            case 2:
                return "Two";
            case 3: return "Three";
            default:
                return "Unknown";
        }
    }

    // Supporting methods and classes for compilation
    private List<User> GetUsers() => new List<User>();
    private List<Order> GetUserOrders(int userId) => new List<Order>();
    private List<OrderItem> GetOrderItems(int orderId) => new List<OrderItem>();
    private decimal CalculatePrice(OrderItem item) => 10.0m;
    private decimal CalculateDiscount(OrderItem item, User user) => 1.0m;
    private void SavePrice(int itemId, decimal price) { }
    private object GetData() => new object();
    private void ProcessData(object data) { }
    private void Cleanup() { }
}

// Supporting classes with bad formatting
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Email { get; set; } = "";
    public bool IsActive { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsValid { get; set; }
}

public class PaymentMethod
{
    public string Type { get; set; } = "";
}

public class ShippingAddress
{
    public string State { get; set; } = "";
}

public class BillingAddress
{
}

public class DiscountCoupon
{
    public bool IsValid { get; set; }
    public decimal DiscountPercentage { get; set; }
}

public class TaxCalculationRules
{
    public decimal CalculateTax(decimal amount, string state) => amount * 0.08m;
}