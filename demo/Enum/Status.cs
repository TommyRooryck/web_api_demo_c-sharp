namespace demo.Enum
{
    public enum Status
    {
        CART, // User is adding items to cart
        CLOSED, // Cart status is closed, create new cart
        NEW, // User has checked out
        FULFILLED // Admin has delivered the order
    }
}
