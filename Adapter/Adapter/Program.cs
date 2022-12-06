namespace Adapter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RoundHole roundHole = new(5);
            RoundPeg smallRoundPeg = new(5), largeRoundPeg = new(10);
            Console.WriteLine(roundHole.fits(smallRoundPeg).ToString() + " " + roundHole.fits(largeRoundPeg).ToString());

            SquarePeg smallSquarePeg = new(5), largeSquarePeg = new(10);
            //roundHole.fits(smallSquarePeg); //nie skompiluje się przez niezgodność typów
            //roundHole.fits(largeSquarePeg); //nie skompiluje się przez niezgodność typów

            SquarePegAdapter smallSquarePegAdapter = new(smallSquarePeg), largeSquarePegAdapter = new(largeSquarePeg);

            Console.WriteLine($"{roundHole.fits((RoundPeg)smallSquarePegAdapter)} {roundHole.fits((RoundPeg)largeSquarePegAdapter)}");
        }

        public class RoundHole
        {
            public double Radius { get; }

            public RoundHole(double radius) => this.Radius = radius;
            
            public bool fits(RoundPeg peg)
            {
                return this.Radius >= peg.Radius;
            }
        }

        public class RoundPeg
        {
            public double Radius { get; }
        
            public RoundPeg(double radius) => this.Radius = radius;
        }

        public class SquarePeg
        {
            public double Width { get; }
            public SquarePeg(double width) => this.Width = width;
        }

        public class SquarePegAdapter : RoundPeg
        {
            private SquarePeg peg { get; }
            private double Width { get; }

            public SquarePegAdapter(SquarePeg peg) : base(peg.Width * Math.Sqrt(2)/2) 
            { 
                this.peg = peg; 
            }
            
        }


    }
}