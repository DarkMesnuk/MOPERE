namespace Base.API.Requests;

public class BaseAuthRequest
{
    private string _activeUserId;

    public void SetActiveUserId(string userId) => _activeUserId = userId;
    public string GetActiveUserId() => _activeUserId;
}