namespace FactoryMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ModeOfTransport[] modeOfTransports = { ModeOfTransport.Sea, ModeOfTransport.Road, ModeOfTransport.Road, ModeOfTransport.Sea };
            ILogistics logistic = new RoadLogistics();
            ITransport[] transports = new ITransport[modeOfTransports.Length];

            for (int i = 0; i < modeOfTransports.Length; i++)
            {
                if (modeOfTransports[i] == ModeOfTransport.Sea && logistic is not SeaLogistics)
                    logistic = new SeaLogistics();
                else if (modeOfTransports[i] == ModeOfTransport.Road && logistic is not RoadLogistics)
                    logistic = new RoadLogistics();

                transports[i] = logistic.CreateTransport();
            }


            //deliver goods....
            for (int i = 0; i < modeOfTransports.Length; i++)
            {
                Console.WriteLine(transports[i].Deliver());
            }
        }
    }

    public enum ModeOfTransport
    {
        Road = 0,
        Sea = 1
    }

    public interface ILogistics
    {
        public ITransport CreateTransport();
    }

    public interface ITransport
    {
        public string Deliver();
    }

    public class Truck : ITransport
    {
        private static int idCounter;
        int id;

        public Truck() => id = idCounter++;

        public string Deliver()
        {
            return $"Truck {id} deliver products to destination";
        }
    }

    public class Ship : ITransport
    {
        private static int idCounter;
        int id;

        public Ship() => id = idCounter++;

        public string Deliver()
        {
            return $"Ship {id} deliver products to destination";
        }
    }

    public class RoadLogistics : ILogistics
    {
        public ITransport CreateTransport() => new Truck();
    }

    public class SeaLogistics : ILogistics
    {
        public ITransport CreateTransport() => new Ship();
    }
}