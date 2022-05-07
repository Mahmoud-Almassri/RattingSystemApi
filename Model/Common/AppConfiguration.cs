namespace RattingSystem.Model.Common
{
    public class AppConfiguration
    {
        private readonly string _connectionString = string.Empty;
        private readonly string _JWTKey = string.Empty;
        private readonly string _Issuer = string.Empty;
        private readonly string _Audience = string.Empty;
        public AppConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            IConfigurationRoot root = configurationBuilder.Build();

            _connectionString = root.GetConnectionString("SqlConnection");
            _JWTKey = root.GetSection("Jwt").GetSection("Key").Value;
            _Issuer = root.GetSection("Jwt").GetSection("Issuer").Value;
            _Audience = root.GetSection("Jwt").GetSection("Audience").Value;
        }
        public string ConnectionString
        {
            get => _connectionString;
        }
        public string JWTKey
        {
            get => _JWTKey;
        }
        public string Issuer
        {
            get => _Issuer;
        }
        public string Audience
        {
            get => _Audience;
        }
    }
}
