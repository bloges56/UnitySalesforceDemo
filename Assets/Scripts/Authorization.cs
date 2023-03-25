using UnityEngine.Networking;


public class Authorization
{
    public const string LoginEndpoint = "https://test.salesforce.com/services/oauth2/token";
    public const string ApiEndpoint = "/services/data/v57.0/"; //Use your org's version number

    private string Username { get; set; }
    private string Password { get; set; }
    private string Token { get; set; }
    private string ClientId { get; set; }
    private string ClientSecret { get; set; }
    public string AuthToken { get; set; }
    public string ServiceUrl { get; set; }

    static readonly UnityWebRequest Client;

    static  Authorization()
    {
        Client = UnityWebRequest.Get(LoginEndpoint);
    }
}
