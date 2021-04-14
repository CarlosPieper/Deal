namespace Api.Services.Interfaces
{
    public interface ICryptographyService
    {
        string EncryptPassword(string password);
    }
}