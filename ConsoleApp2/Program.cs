
Console.WriteLine(new MilesToKmConverter().MilesToKm(1));
Console.WriteLine(new NauticalMilesToKmConverter().MilesToKm(1));
Console.WriteLine(((MilesToKmConverter)new NauticalMilesToKmConverter()).MilesToKm(1));
Console.WriteLine(((NauticalMilesToKmConverter)new MilesToKmConverter()).MilesToKm(1));

//void Sort(out int a, out int b)
//void Sort(int a, int b)
//void Sort(ref int a, ref int b)
//void Sort(int* a, int* b)
void Sort(object a, object b)
{
    if (a > b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }

}



public class MilesToKmConverter
{
    public virtual double MilesToKmFactor { get { return 1.609; } }
    public double MilesToKm(double miles)
    {
        return this.MilesToKmFactor * miles;
    }
}

public class NauticalMilesToKmConverter : MilesToKmConverter
{
    public override double MilesToKmFactor { get { return 1.852; } }
}


