namespace Kompozyt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MobilePhone mobilePhone1 = new(1000, 100, 6), mobilePhone2 = new(600, 150, 4), mobilePhone3 = new(2000, 200, 16);
            Speaker speaker1 = new(100, 300, 200), speaker2 = new(250, 350, 350), speaker3 = new(1500, 1000, 1500);
            Modem modem1 = new(100, 100, 4), modem2 = new(50, 70, 3), modem3 = new(450, 200, 8);
            Box box1 = new(), box2 = new(), box3 = new(), box4 = new(), box5 = new(), box6 = new(), box7 = new(), box8 = new(), box9 = new(), box10 = new();

            // Kod klienta współpracuje ze wszystkimi komponentami za
            // pośrednictwem ich interfejsu bazowego. Dzięki temu kod
            // kliencki posiada wsparcie zarówno prostych obiektów-liści,
            // jak i złożonych kompozytów.
            box2.Add(speaker1);
            box5.Add(mobilePhone1);
            box5.Add(modem1);
            box3.Add(mobilePhone2);
            box3.Add(box5);
            box1.Add(box4);
            box1.Add(box3);
            box1.Add(box2);

            Box order1 = new();
            order1.Add(box1);

            box10.Add(speaker3);
            box9.Add(mobilePhone3);
            box8.Add(box10);
            box8.Add(modem2);
            box8.Add(modem3);
            box6.Add(box7);
            box6.Add(speaker2);
            box6.Add(box8);
            box6.Add(box9);

            Box order2 = new();
            order2.Add(box6);
           
            Console.WriteLine($"{order1.CountPrice()} {order1.CountWeight()}"); //1800,0 650,0
            Console.WriteLine($"{order2.CountPrice()} {order2.CountWeight()}"); //
        }

        // Interfejs komponentu deklaruje działania wspólne zarówno dla
        // prostych jak i skomplikowanych obiektów struktury.
        public interface IComponent
        {
            public decimal CountPrice();
            public decimal CountWeight();
        }

        public class Box : IComponent
        {
            private List<IComponent> _children = new();
            public List<IComponent> Children { get { return _children; } }

            public void Add(IComponent element) => _children.Add(element);

            public void Remove(IComponent element) => _children.Remove(element);

            public decimal CountPrice()
            {
                decimal sum = 0.0m;
                foreach(var child in _children)
                {
                    sum += child.CountPrice();
                }
                return sum;
            }

            public decimal CountWeight()
            {
                decimal sum = 0.0m;
                foreach (var child in _children)
                {
                    sum += child.CountWeight();
                }
                return sum;
            }
        }

        // Klasa liść reprezentuje końcowe elementy struktury. Liść nie
        // posiada pod-obiektów. Na ogół to właśnie te obiekty wykonują
        // faktyczne działania, zaś obiekty złożone jedynie je delegują
        // swoim pod-komponentom.
        public class Product : IComponent
        {
            decimal Price { get; set; }
            decimal Weight { get; set; }

            public decimal CountPrice() => this.Price;
            public decimal CountWeight() => this.Weight;
        
            protected Product(decimal price, decimal weight)
            {
                Price = price;
                Weight = weight;
            }
        }

        // Wszystkie klasy komponentów mogą rozszerzać inne komponenty.
        public class MobilePhone : Product
        {
            int Memory { get; }

            public MobilePhone(decimal price, decimal weight, int memory) : base(price, weight) => this.Memory = memory;
        }

        // Wszystkie klasy komponentów mogą rozszerzać inne komponenty.
        public class Speaker : Product
        {
            int Power { get; }

            public Speaker(decimal price, decimal weight, int power) : base(price, weight) => this.Power = power;
        }

        // Wszystkie klasy komponentów mogą rozszerzać inne komponenty.
        public class Modem : Product
        {
            int inputSocketNumber { get; }

            public Modem(decimal price, decimal weight, int socketNumber) : base(price, weight) => inputSocketNumber = socketNumber;
        }

    }
}