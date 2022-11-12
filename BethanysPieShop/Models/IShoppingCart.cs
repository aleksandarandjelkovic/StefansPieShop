namespace BethanysPieShop.Models
{
	public interface IShoppingCart
	{
		void AddToCart(Pie pie, int amount);
		int RemoveFromCart(Pie pie);
		List<ShoppingCartItem> GetShoppingCartItems();
		void ClearCart();
		decimal GetShoppingCartTotal();
		List<ShoppingCartItem> ShoppingCartItems { get; set; }
	}
}
