using AutoMapper;
using E_commerce.Features.Order;
using E_commerce.Models;

namespace E_commerce.Features.Orders
{
    public class OrderProfile:Profile
    {
        public OrderProfile() 
        {
        CreateMap<Products,OrderProduct>();

            CreateMap<Models.Order, OrderResponse>()
                    .ForMember(des => des.Products, opt => opt.Ignore())
                    .ForMember(des=>des.CustomerName,opt=>opt.MapFrom(so=>so.Customer.Name));   
        }
       
    }
}
