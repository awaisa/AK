namespace BusinessCore.Data
{
    public static class ApplicationContextExtension
    {
        public static void EnsureSeeded(this ApplicationContext context)
        {
            DbInitializer dbInitializer = new DbInitializer(context);
            dbInitializer.DoIt();
        }
    }
}
