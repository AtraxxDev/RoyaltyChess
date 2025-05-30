[System.Serializable]
public class LoginResponse
{
    public string status;
    public int id;
    public string name;
    public string email;
    public string password;
}

[System.Serializable]
public class ProductResponse
{
    public string user;
    public string game;
    public string product;
    public float price;
    public string sale_date;
    public int discount_percentage;
}
