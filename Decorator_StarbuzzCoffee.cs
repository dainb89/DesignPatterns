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
    public virtual void setSize(Size s) { size = s; }
}
public abstract class CondimentDecorator : Beverage
{
    public Beverage beverage;
    public new Size getSize() { return beverage.getSize(); }
    protected List<String> countDes(String des)
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
                    int c = Int32.Parse(subs[0]);
                    c++;
                    lstDes[i] = c.ToString() + " " + subs[1];
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
    public override List<String> getDescription() { return countDes("Soy"); }
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
public class Espresso : Beverage
{
    public Espresso() { description = "Espresso"; }
    public override double cost() { return 1.99; }
}
public class DarkRoast : Beverage
{
    public DarkRoast() { description = "DarkRoast"; }
    public override double cost() { return 0.69; }
}
public class HouseBlend : Beverage
{
    public HouseBlend() { description = "House Blend Coffee"; }
    public override double cost() { return .89; }
}
public class Mocha : CondimentDecorator
{
    public Mocha(Beverage b) { beverage = b; }
    public override List<String> getDescription() { return countDes("Mocha"); }
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