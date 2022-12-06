namespace Decorator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileDataSource dataSource1 = new FileDataSource("file.txt");

            EncryptionDecorator encDecorator = new(dataSource1);
            CompressionDecorator compressionDecorator = new(encDecorator);

            compressionDecorator.WriteData("a");
            compressionDecorator.ReadData(32);
        }

        public interface IDataSource
        {
            public void WriteData(object data);
            public void ReadData(object data);
        }

        public class FileDataSource : IDataSource
        {
            private string _fileName;

            public FileDataSource(string filename) => _fileName = filename;

            public void WriteData(object data)
            {
                Console.WriteLine("FileDataSource write data");
            }

            public void ReadData(object data)
            {
                Console.WriteLine("FileDataSource read data");
            }
        }

        public class DataSourceDecorator : IDataSource
        {
            protected IDataSource wrapee;
            public DataSourceDecorator(IDataSource s) => wrapee = s;

            public void WriteData(object data)
            {
                Console.WriteLine("DataSourceDecorator write data");
                wrapee.WriteData(data);
            }
            
            public void ReadData(object data)
            {
                Console.WriteLine("DataSourceDecorator read data");
                wrapee.ReadData(data);
            }
        }

        public class EncryptionDecorator : DataSourceDecorator
        {
            public EncryptionDecorator(IDataSource s) : base(s)
            {

            }

            public void WriteData(object data)
            {
                Console.WriteLine("EncryptionDecorator write data");
                wrapee.WriteData(data);
            }

            public void ReadData(object data)
            {
                Console.WriteLine("EncryptionDecorator read data");
                wrapee.ReadData(data);
            }
        }

        public class CompressionDecorator : DataSourceDecorator
        {
           public CompressionDecorator(IDataSource s) : base(s)
            {

            }

            public void WriteData(object data)
            {
                Console.WriteLine("CompressionDecorator write data");
                wrapee.WriteData(data);
            }

            public void ReadData(object data)
            {
                Console.WriteLine("CompressionDecorator read data");
                wrapee.ReadData(data);
            }
        }

    }
}