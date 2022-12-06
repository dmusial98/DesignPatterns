//oddzielenie abstrakcji od implementacji

namespace Bridge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TV tv = new TV();
            Radio radio = new Radio();

            RemoteControl remote = new(radio);
            AdvancedRemoteControl advancedRemote = new(tv);

            remote.togglePower();
            remote.channelUp();

            advancedRemote.togglePower();
            advancedRemote.volumeDown();
        }

        // "Abstrakcja" definiuje interfejs dla części "kontrolującej"
        // obu hierarchii klas. Posiada odniesienie do obiektu z
        // hierarchii "implementacyjnej" i deleguje temu obiektowi
        // faktyczną pracę.
        public class RemoteControl
        {
            protected IDevice device;

            public RemoteControl(IDevice device) => this.device = device;

            public void togglePower()
            {
                if (device.isEnabled())
                    device.disable();
                else
                    device.enable();
            }

            public void volumeDown() => device.setVolume(device.getVolume() - 1);

            public void volumeUp() => device.setVolume(device.getVolume() + 1);

            public void channelDown() => device.setChannel(device.getChannel() - 1);

            public void channelUp() => device.setChannel(device.getChannel() + 1);
        }

        // Można rozszerzać klasy należące do hierarchii abstrakcyjnej
        // niezależnie od klas urządzeń.
        public class AdvancedRemoteControl : RemoteControl
        {
            public AdvancedRemoteControl(IDevice device) : base(device)
            { }

            public void mute() => device.setVolume(0);
        }

        // Interfejs "implementacji" deklaruje metody wspólne dla
        // wszystkich konkretnych klas implementacji. Nie musi zgadzać
        // się z interfejsem abstrakcji. Co więcej, oba interfejsy mogą
        // być zupełnie różne. Zazwyczaj interfejs implementacji posiada
        // tylko proste działania, podczas gdy abstrakcja definiuje
        // działania wysokopoziomowe oparte na tych podstawowych.
        public interface IDevice
        {
            public abstract bool isEnabled();
            public void enable();
            public void disable();
            public int getVolume();
            public void setVolume(int percent);
            public int getChannel();
            public void setChannel(int channel);
        }

        public class TV : IDevice
        {
            private bool enabled = false;
            private int channel = 1;
            private int volume = 10;

            public bool isEnabled() => enabled;
            public void enable() => this.enabled = true;
            public void disable() => this.enabled = false;
            public int getVolume() => this.volume;
            public void setVolume(int percent) => this.volume = percent;
            public int getChannel() => this.channel;
            public void setChannel(int channel) => this.channel = channel;
        }

        public class Radio : IDevice
        {
            private bool enabled = false;
            private int channel = 1;
            private int volume = 5;

            public bool isEnabled() => enabled;
            public void enable() => this.enabled = true;
            public void disable() => this.enabled = false;
            public int getVolume() => this.volume;
            public void setVolume(int percent) => this.volume = percent;
            public int getChannel() => this.channel;
            public void setChannel(int channel) => this.channel = channel;
        }
    }
}