namespace Aprojectbackend.Service.orderInterface
{
    public interface IOrderEmailService
    {
        Task SendOrderCompletionEmailAsync(string orderId);
    }
}
