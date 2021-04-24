namespace Api.Entities
{
    public class ApplicationSettings
    {
        public string JWTSecret { get; set; }
        public string DefaultEmail { get; set; }
        public string DefaultPassword { get; set; }
    }
}