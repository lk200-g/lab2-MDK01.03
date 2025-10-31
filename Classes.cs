using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    interface IToggle
    {
        void TurnOn();
        void TurnOff();
    }
    interface IChargeable
    {
        void Charge();
        bool IsOnCharge();
    }
    interface INotification
    {
        void Notify(SmartHub sh);
    }
    class SmartHub // хаб для управления устройствами
    {
        public User Owner;
        public List<Device> Devices;
        public List<Notification> Notifications = new List<Notification>(); // уведомления от устройств

        public SmartHub(User u)
        {
            Devices = new List<Device>();
            Owner = u;
        }
        public void AddNotification(Notification n)
        {
            Notifications.Add(n);
        }
        public void GetNotifications()
        {
            foreach (Notification n in Notifications)
            {
                Console.WriteLine($"{n.Date} {n.Message}");
            }
        }
        public void AddDevice(Device d)
        {
            Devices.Add(d);
        }
        private bool _isDeviceAvailable(Device d)
        {
            if (!Devices.Contains(d))
            {
                throw new ExceptionDeviceNotFound("Устройство не найдено");
            }
            if (!this.Owner.access.GetPermissions().Contains(Permission.execute))
            {
                Console.WriteLine("Access denied");
                return false;
            }
            return true;
        }

        public void DeviceOn(Device d)
        {
            if (_isDeviceAvailable(d))
                d.TurnOn();
        }
        public void DeviceOff(Device d)
        {
            if (_isDeviceAvailable(d))
                d.TurnOn();
        }
    }

    enum Permission
    {
        write, // w - изменение конфигурации
        read, // r - чтение конфигурации
        execute // вкл/выкл устройство
    }
    struct Access
    {
        private List<Permission> _permissions;
        public void AddPermission(Permission p)
        {
            if (_permissions is null)
            {
                _permissions = new List<Permission>();
            }
            _permissions.Add(p);
        }
        public Permission[] GetPermissions()
        {
            if (_permissions is null)
            {
                Console.WriteLine("Права не выданы");
                return null;
            }
            return this._permissions.ToArray();
        }
    }
    struct Notification
    {
        public DateTime Date;
        public string Message;
        public Notification(string msg)
        {
            Date = DateTime.Now;
            Message = msg;
        }
    }
    class User
    {
        public override string ToString() { return base.ToString(); }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj) { return base.Equals(obj); }
        public string Name { get; set; }
        public Access access;
    }
    class Admin : User
    {
        public Admin()
        {
            this.access.AddPermission(Permission.write);
            this.access.AddPermission(Permission.read);
            this.access.AddPermission(Permission.execute);
        }
    }
    abstract class Device : IToggle
    {
        public string Brand;
        public string Model;
        public Device(string brand, string model)
        {
            Brand = brand;
            Model = model;
        }

        public override string ToString() { return base.ToString(); }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj) { return base.Equals(obj); }
        public abstract void Work(); // метод определяет что делает устройство
        public void Configure() { }
        public void TurnOn()
        {
            Console.WriteLine($"Устройство {this.Brand} {this.Model} включено");
            Work();
        }
        public void TurnOff()
        {
            Console.WriteLine($"Устройство {this.Brand} {this.Model} выключено");
        }
    }
    public enum LightType
    {
        WARM,
        WHITE,
        COLD
    }
    public enum Brightness
    {
        LOW,
        NORMAL,
        BRIGHT
    }
    public struct Configuration
    {
        public LightType LightType;
        public Brightness Brightness;
    }
    class Lamp : Device, IComparable<Lamp>
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public int CompareTo(Lamp other) // Сравнение по ID (для сортировки SortedSet)
        {
            if(this.Id > other.Id) 
            {
                return 1;
            } 
            else if (this.Id < other.Id)
            {
                return -1;
            } else
            {
                return 0;
            }
        }
        private static int _nextId = 0;
        public int Id { get; set; } // Генерируется автоматически
        public Lamp(User owner, string brand, string model) : base(brand, model)
        {
            Owner = owner;
            Id = _nextId++;
            CreationDate = DateTime.Now;
        }
        public DateTime CreationDate { get; set; } // Генерируется автоматически
        public float Power { get; set; }
        public float Resistance { get; set; }
        public int? RefreshRate { get; set; }
        public User Owner { get; set; } // class
        private Configuration _config; // struct
        public LightType LightType { get { return _config.LightType; } } // enum
        public Brightness Brightness { get { return _config.Brightness; } } // enum

        public void Configure(User u, LightType lt = LightType.WHITE, Brightness b = Brightness.NORMAL)
        {
            if (u == this.Owner)
            {
                _config.LightType = lt;
                _config.Brightness = b;
            }
        }
        public override void Work()
        {
            Console.WriteLine($"Устройство {this.Brand} {this.Model} работает, свет: {_config.LightType}");
        }
    }
    class Robocleaner : Device, IChargeable
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public Robocleaner(string brand, string model) : base(brand, model) { }
        private int _speed;
        private string _route;
        public int Battery;
        private bool _isCharging;
        public void Configure(User u, int speed, string route)
        {
            if (u is Admin)
            {
                _speed = speed;
                _route = route;
            }
            else
            {
                Console.WriteLine("Access denied");
            }
        }

        public override void Work()
        {
            _isCharging = false;
            Console.WriteLine($"Устройство {this.Brand} {this.Model} чистит пол");
        }
        public bool IsOnCharge()
        {
            if (_isCharging) return true;
            else return false;
        }
        public void Charge()
        {
            _isCharging = true;
            Battery = 100;
            Console.WriteLine($"Устройство {this.Brand} {this.Model} заряжено на 100%");
            return;
        }
    }
    class CoffeeMachine : Device
    {
        public CoffeeMachine(string brand, string model) : base(brand, model) { }
        private List<string> _coffees;

        public void Configure(User u, string coffee)
        {
            if (u is Admin)
            {
                _coffees.Add(coffee);
                Console.WriteLine("Добавлен новый вариант кофе " + coffee);
            }
            else
            {
                Console.WriteLine("Access denied");
            }
        }
        public override void Work()
        {
            Console.WriteLine($"Устройство {this.Brand} {this.Model} делает кофе");
        }
    }

    class MotionDetector : Device, INotification
    {
        public MotionDetector(string brand, string model) : base(brand, model) { }
        public override void Work()
        {
            Console.WriteLine($"Устройство {this.Brand} {this.Model} отслеживает движения");
        }
        public void Notify(SmartHub sh)
        {
            sh.AddNotification(new Notification("Движение замечено"));
        }

    }
    class ExceptionDeviceNotFound : Exception
    {
        public ExceptionDeviceNotFound() { }
        public ExceptionDeviceNotFound(string message) : base(message) { }
        public override string Message => "Устройство не найдено";
    }
}
