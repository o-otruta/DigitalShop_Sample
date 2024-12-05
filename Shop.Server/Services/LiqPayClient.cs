using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class LiqPaySettings
{
    public string PublicKey { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD";
}

public class LiqPayClient
{
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly string _returnUrl;
    private readonly string _serverUrl;
    private readonly string _currency;
    private const string ApiUrl = "https://www.liqpay.ua/api/3/checkout";

    public LiqPayClient(IOptions<LiqPaySettings> liqPaySettings)
    {
        var settings = liqPaySettings.Value;
        _publicKey = settings.PublicKey;
        _privateKey = settings.PrivateKey;
        _returnUrl = settings.ReturnUrl;
        _currency = settings.Currency;
    }

    public string GeneratePaymentUrl(decimal amount, string currency, string description, string orderId)
    {
        var data = new
        {
            public_key = _publicKey,
            version = "3",
            action = "pay",
            amount = amount,
            currency = _currency,
            description = description,
            order_id = orderId,
            result_url = _returnUrl.Replace("{orderId}", orderId),
            server_url = _serverUrl,
            sandbox = "1"
        };

        string jsonData = JsonConvert.SerializeObject(data);
        string encodedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
        string signature = GenerateSignature(encodedData);

        return $"{ApiUrl}?data={encodedData}&signature={signature}";
    }

    private string GenerateSignature(string data)
    {
        string signatureString = $"{_privateKey}{data}{_privateKey}";
        using var sha1 = System.Security.Cryptography.SHA1.Create();
        var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(signatureString));
        return Convert.ToBase64String(hashBytes);
    }
}
