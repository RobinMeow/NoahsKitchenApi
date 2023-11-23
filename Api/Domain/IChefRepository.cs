namespace api.Domain;

public interface IChefRepository
{
    Task AddAsync(Chef chef);
    Task<Chef?> GetByNameAsync(string chefname);
    Task<Chef?> GetByEmailAsync(string email);
    Task<IEnumerable<Chef>> GetAllAsync();
}
