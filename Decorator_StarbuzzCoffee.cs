using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Beverage beverage = new Espresso();
        var des = string.Join(", ", beverage.getDescription());
        Console.WriteLine("Cafe: " + des + " ($" + beverage.cost() + ")");
        Beverage beverage2 = new DarkRoast();
        beverage2 = new Mocha(beverage2);
        beverage2 = new Mocha(beverage2);
        beverage2 = new Mocha(beverage2);
        des = string.Join(", ", beverage2.getDescription());
        Console.WriteLine("Cafe: " + des + " ($" + beverage2.cost() + ")");
    }
}
public enum Size { VENTI, GRANDE, TALL }
public abstract class Beverage
{
    protected String description = "Unknown Beverage";
    protected Size size = Size.GRANDE;
    public virtual List<String> getDescription() { return new List<String>() { description }; }
    public abstract double cost();
    public Size getSize() { return size; }
    public virtual void SetSize(Size s) { size = s; }
}
public class Decaf : Beverage
{
    public Decaf() { description = "Decaf"; }
    public override double cost() { return 1.05; }
}
public class Espresso : Beverage
{
    public Espresso() { description = "Espresso"; }
    public override double cost() { return 1.99; }
}
public class DarkRoast : Beverage
{
    public DarkRoast() { description = "DarkRoast"; }
    public override double cost() { return 0.99; }
}
public class HouseBlend : Beverage
{
    public HouseBlend() { description = "House Blend Coffee"; }
    public override double cost() { return .89; }
}
public abstract class CondimentDecorator : Beverage
{
    public Beverage beverage;
    public new Size getSize() { return beverage.getSize(); }
    protected List<String> CountDes(String des)
    {
        var lstDes = beverage.getDescription();
        for (int i = 0; i < lstDes.Count; i++)
        {
            var d = lstDes[i];
            if (d.Contains(des))
            {
                if (d.Contains(" "))
                {
                    string[] subs = d.Split(' ');
                    if (des.Contains("Steamed Milk") && subs.Length < 3)
                    {
                        lstDes[i] = "2 " + d;
                    }
                    else
                    {
                        try
                        {
                            int c = Int32.Parse(subs[0]);
                            c++;
                            lstDes[i] = c.ToString() + " " + subs[1];
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Wrong to parse " + subs[0]);
                        }
                    }
                }
                else lstDes[i] = "2 " + d;
                return lstDes;
            }
        }
        lstDes.Add(des);
        return lstDes;
    }
}
public class Soy : CondimentDecorator
{
    public Soy(Beverage b) { beverage = b; }
    public override List<String> getDescription() { return CountDes("Soy"); }
    public override double cost()
    {
        double cost = beverage.cost();
        var s = beverage.getSize();
        if (s == Size.TALL) cost += .10;
        else if (s == Size.GRANDE) cost += .15;
        else if (s == Size.VENTI) cost += .20;
        return cost;
    }
}
public class Mocha : CondimentDecorator
{
    public Mocha(Beverage b) { beverage = b; }
    public override List<String> getDescription() { return CountDes("Mocha"); }
    public override double cost()
    {
        double cost = beverage.cost();
        var s = beverage.getSize();
        if (s == Size.TALL) cost += .15;
        else if (s == Size.GRANDE) cost += .20;
        else if (s == Size.VENTI) cost += .25;
        return cost;
    }
}
public class Whip : CondimentDecorator
{
    public Whip(Beverage b) { beverage = b; }
    public override List<String> getDescription() { return CountDes("Whip"); }
    public override double cost()
    {
        double cost = beverage.cost();
        var s = beverage.getSize();
        if (s == Size.TALL) cost += .05;
        else if (s == Size.GRANDE) cost += .10;
        else if (s == Size.VENTI) cost += .15;
        return cost;
    }
}
public class SteamedMilk : CondimentDecorator
{
    public SteamedMilk(Beverage b) { beverage = b; }
    public override List<String> getDescription() { return CountDes("Steamed Milk"); }
    public override double cost()
    {
        double cost = beverage.cost();
        var s = beverage.getSize();
        if (s == Size.TALL) cost += .05;
        else if (s == Size.GRANDE) cost += .10;
        else if (s == Size.VENTI) cost += .15;
        return cost;
    }
}
