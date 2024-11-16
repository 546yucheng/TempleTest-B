namespace Aprojectbackend.DTO.CartDTO
{
    public class CartDTO
    {
        public decimal CartTotal { get; set; }

        public List<CartItemDTO> CartItemDTOs { get; set; }
    }
}
