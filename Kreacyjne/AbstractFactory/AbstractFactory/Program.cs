using System.Reflection.Metadata;

namespace AbstractFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TypesOfFurnitures[] typesOfFurnitures = { TypesOfFurnitures.Victorian, TypesOfFurnitures.Modern, TypesOfFurnitures.Modern, TypesOfFurnitures.Victorian, TypesOfFurnitures.Modern };
            IFurnitureFactory furnitureFactory = new ModernFurnitureFactory();
            IChair[] chairs = new IChair[typesOfFurnitures.Length];
            ISofa[] sofas = new ISofa[typesOfFurnitures.Length];
            ICoffeTable[] coffeTables = new ICoffeTable[typesOfFurnitures.Length];

            for(int i =0; i < typesOfFurnitures.Length; i++)
            {
                if (typesOfFurnitures[i] == TypesOfFurnitures.Victorian && furnitureFactory.GetType() != typeof(VictorianFurnitureFactory))
                    furnitureFactory = new VictorianFurnitureFactory();
                    
                else if(typesOfFurnitures[i] == TypesOfFurnitures.Modern && furnitureFactory.GetType() != typeof(ModernFurnitureFactory))
                    furnitureFactory= new ModernFurnitureFactory();

                chairs[i] = furnitureFactory.CreateChair();
                sofas[i] = furnitureFactory.CreateSofa();
                coffeTables[i] = furnitureFactory.CreateCoffeTable();
            }

            for(int i =0; i < typesOfFurnitures.Length; i++)
            {
                Console.WriteLine(chairs[i].SitDown());
                Console.WriteLine(sofas[i].LieDown());
                Console.WriteLine(coffeTables[i].TakeCupOfCoffe());
            }
        }
    }

    public enum TypesOfFurnitures
    {
        Victorian = 0,
        Modern = 1
    }

    public interface IFurnitureFactory
    {
        public IChair CreateChair();
        public ISofa CreateSofa();
        public ICoffeTable CreateCoffeTable();
    }

    public interface IChair
    {
        public string SitDown();
    }

    public interface ISofa
    {
        public string LieDown();
    }

    public interface ICoffeTable
    {
        public string TakeCupOfCoffe();
    }

    public class VictorianFurnitureFactory : IFurnitureFactory
    {
        public IChair CreateChair() => new VictorianChair();

        public ISofa CreateSofa() => new VictorianSofa();

        public ICoffeTable CreateCoffeTable() => new VictorianCoffeTable(); 
    }

    public class ModernFurnitureFactory : IFurnitureFactory
    {
        public IChair CreateChair() => new ModernChair();

        public ISofa CreateSofa() => new ModernSofa();

        public ICoffeTable CreateCoffeTable() => new ModernCoffeTable();
    }

    public class VictorianChair : IChair
    {
        public string SitDown()
        {
            return "I sit down on victorian chair";
        }
    }

    public class VictorianSofa : ISofa
    {
        public string LieDown()
        {
            return "I lie down on victorian sofa";
        }
    }

    public class VictorianCoffeTable : ICoffeTable
    {
        public string TakeCupOfCoffe() => "I take cup of coffe from victorian coffe table";
    }

    public class ModernChair : IChair
    {
        public string SitDown() => "I sit down on modern chair";
    }

    public class ModernSofa : ISofa
    {
        public string LieDown() => "I lie down on modern sofa";
    }

    public class ModernCoffeTable : ICoffeTable
    {
        public string TakeCupOfCoffe() => "I take cup of coffe from modern coffe table";
    }

}