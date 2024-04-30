using ConsoleApp3;

public class Program
{
    private static void Main(string[] args)
    {
        SCode obj = new SCode("C:\\Users\\zezooo\\Desktop\\sql-injection-example-master\\sql-injection-example-master\\SqlInjectionExample\\SqlInjection.aspx.cs");  
        Console.WriteLine(obj.getlines());
        Console.WriteLine(obj.numberFunctions());
    }
}