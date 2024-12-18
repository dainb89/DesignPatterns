using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        CoffeFactory cFactory = new CoffeFactory();
		CoffeStore cStore = new CoffeStore(cFactory);
        Console.WriteLine(cStore.OrderCoffe("1 Espresso with whip and steamed Milk"));
        Console.WriteLine(cStore.OrderCoffe("decaf with 2 Steamed milk"));
		Console.WriteLine(cStore.OrderCoffe("venti Decaf with 2 Steamed milk"));
        Console.WriteLine(cStore.OrderCoffe("dark Roast with 2 mocha and 1 soy"));
        Console.WriteLine(cStore.OrderCoffe("1 tall Dark Roast with 2 mocha and 1 soy"));
    }
}
public class CoffeStore
{
    CoffeFactory factory;
    public CoffeStore(CoffeFactory f) { factory = f; }
    public string OrderCoffe(string orderTxt)
    {
        string sampleOrder = "Plz order like this: House Blend with Steamed Milk, 2 Mocha, 1 soy and Whip";
        if (String.IsNullOrEmpty(orderTxt) || String.IsNullOrWhiteSpace(orderTxt))
        {
            return sampleOrder;
        }
        // #region replace 2 or more space into one
        RegexOptions options = RegexOptions.None;
        Regex regex = new Regex("[ ]{2,}", options);
        var ordTxtLow = regex.Replace(orderTxt, " ");
        // #endregion replace
        ordTxtLow = ordTxtLow.ToLower();
        Beverage beverage;
        if (ordTxtLow.Contains("espresso")) beverage = factory.CreateCoffe("Espresso");
        else if (ordTxtLow.Contains("dark roast")) beverage = factory.CreateCoffe("DarkRoast");
        else if (ordTxtLow.Contains("decaf")) beverage = factory.CreateCoffe("Decaf");
        else if (ordTxtLow.Contains("house blend")) beverage = factory.CreateCoffe("House Blend");
        else return sampleOrder;
        string tSize = Size.GRANDE.ToString();
        if (ordTxtLow.Contains("venti"))
        {
            beverage.SetSize(Size.VENTI);
            tSize = Size.VENTI.ToString();
        }
        else if (ordTxtLow.Contains("tall"))
        {
            beverage.SetSize(Size.TALL);
            tSize = Size.TALL.ToString();
        }

        string[] arOrdTxt = ordTxtLow.Split(' ');

        for (int ii = 1; ii < arOrdTxt.Length; ii++)
        {
            var txt = arOrdTxt[ii];
            var prTxt = arOrdTxt[ii - 1];
            if (txt.Contains("mocha"))
            {
                beverage = CreateCondiment("Mocha", prTxt, beverage);
            }
            if (txt.Contains("soy"))
            {
                beverage = CreateCondiment("Soy", prTxt, beverage);
            }
            if (txt.Contains("whip"))
            {
                beverage = CreateCondiment("Whip", prTxt, beverage);
            }
            if (txt.Contains("steamed") && ii < arOrdTxt.Length - 1 && arOrdTxt[ii + 1].Contains("milk"))
            {
                beverage = CreateCondiment("Steamed Milk", prTxt, beverage);
            }
        }
        var des = string.Join(", ", beverage.getDescription());
        return "Ready: 1 " + tSize + " " + des + " ($" + beverage.cost() + ")";
    }
    private Beverage CreateCondiment(string name, string prTxt, Beverage b)
    {
        Beverage beverage = b;
        if (prTxt.Contains("with") || prTxt.Contains("and") || prTxt.Contains(","))
        {
            return factory.CreateCondiment(name, beverage);
        }
        try
        {
            int cc = Int32.Parse(prTxt);
            for (int iic = 0; iic < cc; iic++)
            {
                beverage = factory.CreateCondiment(name, beverage);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Unable to parse " + prTxt);
        }
        return beverage;
    }
}
public class CoffeFactory
{
    public Beverage CreateCoffe(string type)
    {
        switch (type)
        {
            case "Decaf": return new Decaf();
            case "Espresso": return new Espresso();
            case "DarkRoast": return new DarkRoast();
            case "House Blend": return new HouseBlend();
            default: return null;
        }
    }
    public Beverage CreateCondiment(string type, Beverage coffe)
    {
        switch (type)
        {
            case "Steamed Milk": return new SteamedMilk(coffe);
            case "Whip": return new Whip(coffe);
            case "Mocha": return new Mocha(coffe);
            case "Soy": return new Soy(coffe);
            default: return coffe;
        }
    }
}
//public abstract class Beverage
//public class Decaf : Beverage
//public class Espresso : Beverage
//public class DarkRoast : Beverage
//public class HouseBlend : Beverage
//public abstract class CondimentDecorator : Beverage
//public class Soy : CondimentDecorator
//public class Mocha : CondimentDecorator
//public class Whip : CondimentDecorator
//public class SteamedMilk : CondimentDecorator