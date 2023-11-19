namespace api.Domain;

public interface IChefRepository
{
    Task AddAsync(Chef chef);
    Task<bool> NameIsAlreadyTakenAsync(string chefname);
    Task<IEnumerable<Chef>> GetAllAsync();
    Task<Chef> GetAsync(string name);
}
