using Cicekci.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Cicekci.Services
{
    // Session tabanlı sepet yönetimi
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContext;
        private const string CartKey = "Cart";

        public CartService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public List<CartItem> GetCart()
        {
            var session = _httpContext.HttpContext?.Session;
            if (session == null) return new List<CartItem>();

            var json = session.GetString(CartKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> cart)
        {
            var session = _httpContext.HttpContext?.Session;
            if (session == null) return;
            session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }
            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveCart(cart);
            }
        }

        public void ClearCart()
        {
            var session = _httpContext.HttpContext?.Session;
            session?.Remove(CartKey);
        }

        public decimal GetTotal()
        {
            return GetCart().Sum(c => c.Price * c.Quantity);
        }

        public int GetCount()
        {
            return GetCart().Sum(c => c.Quantity);
        }
    }
}
