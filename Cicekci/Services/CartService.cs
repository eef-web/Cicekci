using Cicekci.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Cicekci.Services
{
    // Session tabanlı sepet servisi — verileri session'da JSON olarak saklar
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        private List<CartItem> GetCart()
        {
            var json = Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json)) return new List<CartItem>();
            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            Session.SetString(SessionKey, json);
        }

        public List<CartItem> GetCartItems()
        {
            return GetCart();
        }

        public void AddItem(int productId, string productName, decimal price, int quantity, string? imageUrl)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    ImageUrl = imageUrl
                });
            }
            SaveCart(cart);
        }

        public void RemoveItem(int productId)
        {
            var cart = GetCart();
            cart.RemoveAll(i => i.ProductId == productId);
            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
            SaveCart(cart);
        }

        public void Clear()
        {
            Session.Remove(SessionKey);
        }

        public decimal GetTotal()
        {
            return GetCart().Sum(i => i.Price * i.Quantity);
        }
    }
}
