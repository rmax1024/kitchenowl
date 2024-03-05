namespace BackendWebApi.Common;

public interface ICommonRepository
{
    Task<bool> IsOnboarding();
}