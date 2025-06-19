using AutoMapper;
using BusinessObject.Models;
using DataAccess.Dto;

namespace eStoreAPI.AutoMapping
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            // Tạo mapping 2 chiều giữa Member và MemberDto
            CreateMap<Member, MemberDto>().ReverseMap();
        }
    }
}
