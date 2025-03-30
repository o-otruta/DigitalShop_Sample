/// \file LiqPayClient.cs
/// \brief Contains configuration and logic for generating LiqPay payment URLs.


using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

/// \brief Configuration settings for LiqPay integration.
public class LiqPaySettings
{
    /// \brief Public API key from LiqPay.
    public string PublicKey { get; set; } = string.Empty;

    /// \brief Private API key from LiqPay.
    public string PrivateKey { get; set; } = string.Empty;

    /// \brief URL to which users are redirected after payment.
    public string ReturnUrl { get; set; } = string.Empty;

    /// \brief Default currency for transactions.
    public string Currency { get; set; } = "USD";
}

/// \brief Client for creating LiqPay payment requests and generating checkout URLs.
public class LiqPayClient
{
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly string _returnUrl;
    private readonly string _serverUrl;
    private readonly string _currency;
    private const string ApiUrl = "https://www.liqpay.ua/api/3/checkout";

    /// \brief Constructor that initializes LiqPay client with settings.
    /// \param liqPaySettings Injected configuration options.
    public LiqPayClient(IOptions<LiqPaySettings> liqPaySettings)
    {
        var settings = liqPaySettings.Value;
        _publicKey = settings.PublicKey;
        _privateKey = settings.PrivateKey;
        _returnUrl = settings.ReturnUrl;
        _currency = settings.Currency;
    }

    /// \brief Generates a LiqPay checkout URL with encoded payment data.
    /// \param amount Amount to be paid.
    /// \param currency Currency for the transaction (e.g., "USD").
    /// \param description Description of the purchase.
    /// \param orderId Unique identifier for the order.
    /// \return A fully formed LiqPay payment URL.
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

    /// \brief Generates a Base64-encoded SHA1 signature for payment verification.
    /// \param data Encoded data string.
    /// \return Signature string for request authentication.
    private string GenerateSignature(string data)
    {
        string signatureString = $"{_privateKey}{data}{_privateKey}";
        using var sha1 = System.Security.Cryptography.SHA1.Create();
        var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(signatureString));
        return Convert.ToBase64String(hashBytes);
    }
}
