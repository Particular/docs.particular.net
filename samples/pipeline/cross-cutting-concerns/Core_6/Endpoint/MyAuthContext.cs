class MyAuthContext
{
    public MyAuthContext(string login, string token)
    {
        Login = login;
        Token = token;
    }

    public string Login { get; }
    public string Token { get; }

    public override string ToString()
    {
        return $"login: {Login}, token: {Token}";
    }
}