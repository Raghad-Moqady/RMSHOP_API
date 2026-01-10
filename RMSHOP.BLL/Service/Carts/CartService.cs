using Mapster;
using RMSHOP.DAL.DTO.Request.Cart;
using RMSHOP.DAL.DTO.Response;
using RMSHOP.DAL.Models.cart;
using RMSHOP.DAL.Repository.Carts;
using RMSHOP.DAL.Repository.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.BLL.Service.Carts
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository ,IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request)
        {
            try
            {
                var product = await _productRepository.FindProductById(request.ProductId);
                if (product is null)
                {
                    //404
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Product Not Found",
                    };
                }
                if(product.Quantity< request.Count)
                {
                    //400
                    //item==product
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = $"Only {product.Quantity} items are currently available in stock.",
                    };
                }
                // امامي خيارين وبعتمد حسب هل هذا اليوزر لاول مرة بضيف هذا المنتج عالسلة ؟وللا المنتج موجود من قبل 
                // الخيارين : Add Or Update Count
                //وهاد كله بسبب انه عملنا :composite key in Cart Table (يعني ممنوع التكرار لان لو تكرر رح يعطيني ايرور من الداتا بيس)
                var cartItem = await _cartRepository.GetCartItemAsync(userId,request.ProductId);
                if (cartItem is not null)
                {
                    //1. update exist item
                    cartItem.Count += request.Count;
                    if (product.Quantity < cartItem.Count)
                    {  //400
                        return new BaseResponse()
                        {
                            Success = false,
                            Message = "Adding this quantity would exceed the available stock.",
                        };
                    }
                    await _cartRepository.UpdateCartItemCountAsync();
                    //200
                    return new BaseResponse()
                    {
                        Success = true,
                        Message = "Count of this Product Updated Successfully"
                    };
                }
                else
                {
                    //2. add new item to cart
                    var newCart = request.Adapt<Cart>();
                    newCart.UserId = userId;
                    await _cartRepository.AddToCartAsync(newCart);

                    //200
                    return new BaseResponse()
                    {
                        Success = true,
                        Message = "Product Added Successfully to Cart"
                    };
                }
            }
            catch (Exception ex)
            {
                //500
                return new BaseResponse()
                {
                    Success = false,
                    UnexpectedErrorFlag = true,
                    Message = "Unexpected Error!",
                    Errors = new List<string> { ex.Message }
                };
            }
            
        }
    }
}
