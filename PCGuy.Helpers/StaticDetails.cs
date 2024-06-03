namespace PCGuy.Helpers;

// ReSharper disable InconsistentNaming
public static class OrderStatus
{
    public const string PENDING = "Pending";
    public const string APPROVED = "Approved";
    public const string PROCESSING = "Processing";
    public const string SHIPPED = "Shipped";
    public const string CANCELLED = "Cancelled";
    public const string REFUNDED = "Refunded";
}

public static class PaymentStatus
{
    public const string PENDING = "Pending";
    public const string APPROVED = "Approved";
    public const string DELAYED_PAYMENT = "ApprovedForDelayedPayment";
    public const string REJECTED = "Rejected";
}

public static class Roles
{
    public const string CUSTOMER = "Customer";
    public const string COMPANY = "Company";
    public const string ADMIN = "Admin";
    public const string EMPLOYEE = "Employee";
}