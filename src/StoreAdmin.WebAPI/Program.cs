namespace StoreAdmin.WebAPI
{
    ///<summary></summary>
    public class Program
    {
        ///<summary></summary>
        protected Program() { }

        ///<summary></summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        ///<summary></summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}