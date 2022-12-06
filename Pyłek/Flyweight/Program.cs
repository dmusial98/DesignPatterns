namespace Flyweight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Forest forest = new();
            forest.PlantTree(1, 1, "oak", "brown", "oak.png");
            forest.PlantTree(12, 8, "oak", "brown", "oak.png");
            forest.PlantTree(3, 19, "willow", "white", "willow.png");
            forest.PlantTree(40, 20, "spruce", "green", "spruce.png");
            forest.PlantTree(10, 50, "spruce", "brown", "spruce.png");
            forest.PlantTree(2, 2, "oak", "brown", "oak.png");

            forest.Draw();
        }

        // Fabryka pyłków podejmuje decyzję o ponownym użyciu
        // istniejącego obiektu-pyłka lub utworzeniu nowego.
        public class TreeFactory
        {
            private static List<TreeType> _treeTypes = new List<TreeType>();

            public static TreeType GetTreeType(string name, string color, string textureName)
            {
                var t = new TreeType(name, color, textureName);
                if(_treeTypes.Contains(t))
                    return _treeTypes[_treeTypes.IndexOf(t)];
                else
                {
                    _treeTypes.Add(t);
                    return t;
                }
            }
        }


        // Klasa pyłek zawiera część stanu drzewa. Pola te przechowują
        // wartości które są unikalne dla każdego drzewa. Przykładowo
        // nie znajdziemy tu współrzędnych drzewa, ale teksturę oraz
        // wspólne barwy — owszem. Ponieważ te dane są zazwyczaj
        // WIELKIE, zmarnowalibyśmy bardzo dużo pamięci operacyjnej,
        // przechowując ich kopie w obrębie każdego z obiektów-drzew.
        // Ekstrahujemy więc tekstury, barwy i inne powtarzające się
        // dane do odrębnego obiektu. Wszystkie drzewa będą posiadać
        // odniesienie do nowego obiektu.
        public class TreeType : IEquatable<TreeType>
        {
            static private int _index = 0;
            private int _id;
            private string _name;
            private string _color;
            private string _textureName;

            public TreeType(string name, string color, string textureName)
            {
                _id = _index++;
                _name = name;
                _color = color;
                _textureName = textureName;
            }

            public void Draw(int x, int y)
            {
                Console.WriteLine($"Tree {_name}, id: {_id} in {x},{y} coordinates with color {_color}, texture's name {_textureName} has been drawn");
            }

            public override bool Equals(object obj)
            {
                if (obj == null) 
                    return false;
                TreeType objAsTreeType = obj as TreeType;
                if (objAsTreeType == null) 
                    return false;
                else 
                    return Equals(objAsTreeType);
            }

            public bool Equals(TreeType other)
            {
                if (other is null)
                    return false;

                return this._name == other._name && this._color == other._color && this._textureName == other._textureName;
            }
        }

        // Obiekt-kontekst zawiera zewnętrzne elementy stanu drzewa.
        // Aplikacja może stworzyć miliardy drzew, bo są one bardzo
        // małe: opisują je dwie liczby całkowite oznaczające
        // współrzędne i jedno pole przechowujące odniesienie do obiektu
        // zawierającego opis stanu zewnętrznego.
        public class Tree
        {
            public int x, y;
            public TreeType Type { get; }
        
            public Tree(int x, int y, TreeType type)
            {
                this.x = x;
                this.y = y;
                this.Type = type;
            }

            public void Draw() => Type.Draw(x, y);
        }

        // Klasy Tree i Forest są klientami pyłku. Możesz je połączyć,
        // jeśli nie zamierzasz dalej rozwijać klasy Tree.
        public class Forest
        {
            private List<Tree> _trees = new List<Tree>();

            public void PlantTree(int x, int y, string name, string color, string textureName) => _trees.Add(new Tree(x, y, TreeFactory.GetTreeType(name, color, textureName)));

            public void Draw()
            {
                foreach(var tree in _trees)
                    tree.Draw();
            }
        }
    }
}