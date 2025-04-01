using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    private static IStoreController storeController; // Unity IAP Controller
    private static IExtensionProvider storeExtensionProvider; // Unity IAP Extension

    // Product IDs (must match Play Console)
    public static string PRODUCT_CONSUMABLE25 = "com.finditapp.token_25";
    public static string PRODUCT_CONSUMABLE40 = "com.finditapp.token_40";

    public string androidPurchaseUrl = "";
    public string iosPurchaseUrl = "";

    private void Awake()
    {
    }

    void Start()
    {
        androidPurchaseUrl = ApiDataCall.Instance.baseUrl + "inapppurchase/androidPlanPurchase";
        iosPurchaseUrl =       ApiDataCall.Instance.baseUrl + "inapppurchase/applePlanPurchase";

        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add consumable products
        builder.AddProduct(PRODUCT_CONSUMABLE25, ProductType.Consumable);
        builder.AddProduct(PRODUCT_CONSUMABLE40, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    // Purchase function for 25 tokens
    public void BuyConsumable25()
    {
        BuyProductID(PRODUCT_CONSUMABLE25);
    }

    // Purchase function for 40 tokens
    public void BuyConsumable40()
    {
        BuyProductID(PRODUCT_CONSUMABLE40);
    }

    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"Purchasing product: {productId}");
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.LogError("Purchase failed: Product not available for purchase");
            }
        }
        else
        {
            Debug.LogError("Purchase failed: IAP not initialized");
        }
    }

    // Called when a purchase is completed
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string receipt = args.purchasedProduct.receipt;

        if (Application.platform == RuntimePlatform.Android)
        {
            string productId = args.purchasedProduct.definition.id;
            string transactionID = args.purchasedProduct.transactionID;
            string purchaseToken = ExtractGooglePurchaseToken(receipt);

            Debug.Log($"Purchase Successful: {productId}");
            Debug.Log($"Order ID: {transactionID}");
            Debug.Log($"Raw Receipt: {receipt}");
            Debug.Log("purchase token " + purchaseToken);
        }
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string appleTransactionId = "";
            string originalTransactionId = "";

            (appleTransactionId, originalTransactionId) = ExtractAppleTransactionIds(receipt);

            Debug.Log($"Apple Transaction ID:       {appleTransactionId}");
            Debug.Log($"Original Transaction ID: {originalTransactionId}");
        }

        if (args.purchasedProduct.definition.id == PRODUCT_CONSUMABLE25)
        {
            Debug.Log("Purchased 25 Tokens!");
        }
        else if (args.purchasedProduct.definition.id == PRODUCT_CONSUMABLE40)
        {
            Debug.Log("Purchased 40 Tokens!");
        }
        else
        {
            Debug.LogWarning("Purchase error: Unrecognized product");
        }

        return PurchaseProcessingResult.Complete;
    }

    // Called when a purchase fails
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"Purchase failed: {product.definition.id}, Reason: {failureDescription.reason}");
    }

    // Called when initialization is successful
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    // Called when initialization fails (Updated for IDetailedStoreListener)
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"IAP Initialization Failed: {error}, Message: {message}");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new NotImplementedException();
    }


    private string ExtractGooglePurchaseToken(string receipt)
    {
        try
        {
            var receiptWrapper = JsonUtility.FromJson<GooglePurchaseWrapper>(receipt);
            return receiptWrapper.Payload.json.purchaseToken;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to extract Google purchaseToken: {e.Message}");
            return "";
        }
    }

    private (string, string) ExtractAppleTransactionIds(string receipt)
    {
        try
        {
            var receiptWrapper = JsonUtility.FromJson<ApplePurchaseWrapper>(receipt);
            var decodedReceipt = JsonUtility.FromJson<AppleReceiptPayload>(receiptWrapper.Payload);

            string appleTransactionId = decodedReceipt.in_app[0].transaction_id;
            string originalTransactionId = decodedReceipt.in_app[0].original_transaction_id;

            return (appleTransactionId, originalTransactionId);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to extract Apple Transaction IDs: {e.Message}");
            return ("", "");
        }
    }


    public IEnumerator AndroidPurchase(string productId, string purchaseToken, string orderId, string type, string amount)
    {
        WWWForm form = new WWWForm();
        form.AddField("productId", productId);
        form.AddField("productId", purchaseToken);
        form.AddField("productId", orderId);
        form.AddField("productId", type);
        form.AddField("productId", amount);

        Debug.Log(productId + " userName");
        Debug.Log(purchaseToken + " profile");
        Debug.Log(orderId + " profile");
        Debug.Log(type + " profile");
        Debug.Log(amount + " profile");

        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(androidPurchaseUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("POST request successful: " + request.downloadHandler.text);
                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json) ;
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json) ;
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
        }
    }


    public IEnumerator IOSPurchase(string appleTransactionId, string originalTransactionId, string productId, string amount)
    {
        WWWForm form = new WWWForm();
        form.AddField("appleTransactionId", appleTransactionId);
        form.AddField("originalTransactionId", originalTransactionId);
        form.AddField("productId", productId);
        form.AddField("amount", amount);

        Debug.Log("appleTransactionId " + appleTransactionId);
        Debug.Log("originalTransactionId " + originalTransactionId);
        Debug.Log("productId " + productId);
        Debug.Log("amount " + amount);
    
        // Create request
        using (UnityWebRequest request = UnityWebRequest.Post(iosPurchaseUrl, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + ApiDataCall.Instance.token);
            yield return request.SendWebRequest();

            // Check response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("POST request successful: " + request.downloadHandler.text);
                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json) ;
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
            else
            {
                Debug.LogError("POST request failed!");
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);

                string Json = request.downloadHandler.text;
                SimpleJSON.JSONNode status = SimpleJSON.JSON.Parse(Json);
                DialogCanvas.Instance.ShowFailedDialog(status["message"]);
            }
        }
    }


    // JSON Structure for Google Play Receipt
    [Serializable]
    public class GooglePurchaseWrapper
    {
        public GoogleReceiptPayload Payload;
    }

    [Serializable]
    public class GoogleReceiptPayload
    {
        public GooglePurchaseData json;
    }

    [Serializable]
    public class GooglePurchaseData
    {
        public string purchaseToken;
    }


    [Serializable]
    public class ApplePurchaseWrapper
    {
        public string Payload; // This is the base64-encoded Apple receipt
    }

    [Serializable]
    public class AppleReceiptPayload
    {
        public AppleInAppPurchase[] in_app;
    }

    [Serializable]
    public class AppleInAppPurchase
    {
        public string transaction_id;
        public string original_transaction_id;
    }
}
