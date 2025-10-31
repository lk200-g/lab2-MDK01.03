namespace lab2
{
    class Objects
    {
        public static Lamp[] objects = GetObjects();
        public static Lamp[] GetObjects()
        {
            User user = new User();
            user.Name = "lampowner";
            Lamp lamp1 = new Lamp(user, "lampcorp", "lamp000");
            lamp1.Power = 30;
            lamp1.Resistance = 10;
            lamp1.Configure(user, LightType.COLD, Brightness.BRIGHT);

            Lamp lamp2 = new Lamp(user, "lampcorp2", "lamp2222");
            lamp2.Power = 60;
            lamp2.Resistance = 5;
            lamp2.RefreshRate = 50;

            Lamp lamp3 = new Lamp(user, "lampcorp3", "notalamp");
            lamp3.Power = 30;
            lamp3.Resistance = 10;
            lamp3.Configure(user, LightType.WARM, Brightness.LOW);

            Lamp lamp4 = new Lamp(user, "lampcorpotaion", "LAMP");
            lamp4.Power = 30;
            lamp4.Resistance = 10;
            lamp4.RefreshRate = 50;
            lamp4.Configure(user, LightType.COLD, Brightness.BRIGHT);


            return new Lamp[] { lamp1, lamp2, lamp3, lamp4 };
        }

    }
}
