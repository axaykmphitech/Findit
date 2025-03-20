using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    private static IStoreController storeController; // Unity IAP Controller
    private static IExtensionProvider storeExtensionProvider; // Unity IAP Extension

    // Product IDs (must match Play Console)
    public static string PRODUCT_CONSUMABLE25 = "com.finditapp.token_25";
    public static string PRODUCT_CONSUMABLE40 = "com.finditapp.token_40";

    void Start()
    {
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
        if (args.purchasedProduct.definition.id == PRODUCT_CONSUMABLE25)
        {
            Debug.Log("Purchased 25 Tokens!");
            // Grant the tokens here
        }
        else if (args.purchasedProduct.definition.id == PRODUCT_CONSUMABLE40)
        {
            Debug.Log("Purchased 40 Tokens!");
            // Grant the tokens here
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
        Debug.Log("IAP Initialized!");
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
}
