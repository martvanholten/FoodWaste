namespace Domain.Models;
public class ErrorModel : Exception
{
    public string Error { get; } = null!;
    public int HttpError { get; }

    public ErrorModel(string error, int httpError)
    {
        this.Error = error;
        this.HttpError = httpError;
    }
}
