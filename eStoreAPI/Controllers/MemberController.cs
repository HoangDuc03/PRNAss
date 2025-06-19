using AutoMapper;
using BusinessObject.Models;
using DataAccess.Dto;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace eStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MemberController(IRepository<Member> memberRepository,IConfiguration configuration, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public async Task<List<MemberDto>> getAll()
        {
            var query =from i in _memberRepository.GetAll()
                       select new MemberDto()
                       {
                           MemberId = i.MemberId,
                           Email =i.Email,
                           Password =i.Password,
                           City = i.City,
                           CompanyName = i.CompanyName,
                           Country = i.Country,
                           Role = 1
                       };
            return query.ToList();
        }
        [HttpGet("Getid")]
        public async Task<MemberDto> GetId(int id)
        {
            var query = _memberRepository.GetId(id);
            return _mapper.Map<MemberDto>(query);
        }
        [HttpPut("Update")]
        public async Task<bool> Update(MemberDto member)
        {
            try
            {
                if (member == null || member.MemberId == null)
                {
                    return false; // Hoặc ném ngoại lệ tùy theo logic của bạn
                }
                var existingMember = _memberRepository.GetId((int)member.MemberId);
                if (existingMember == null)
                {
                    return false; // Hoặc ném ngoại lệ nếu không tìm thấy thành viên
                }

                _mapper.Map(member, existingMember);

                _memberRepository.Update(existingMember);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet("Delete")]
        public async Task<bool> Delete(int id)
        {
            _memberRepository.Remove(id);
            return true;
        }
        [HttpPost("Create")]
        public async Task<int> Create(MemberDto member)
        {
            try
            {
                if (member == null)
                {
                    return -1; // Hoặc ném ngoại lệ tùy theo logic của bạn
                }
                if (_memberRepository.GetAll().Any(x => x.Email == member.Email))
                {
                    return 0; // Email đã tồn tại
                }
                var newMember = _mapper.Map<Member>(member);
                _memberRepository.Add(newMember);
                return newMember.MemberId;
            }
            catch (Exception ex)
            {
                return -2;
            }
        }
        [HttpPost("Login")]
        public async Task<MemberDto> Login([FromBody] MemberDto memberDto)
        {
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            if (memberDto.Email == adminEmail && memberDto.Password == adminPassword)
            {
                return new MemberDto() { Email = adminEmail, Role = 2 };
            }
            var mem = _memberRepository.GetAll().Where(x => x.Email == memberDto.Email && x.Password == memberDto.Password).FirstOrDefault();

            if (mem == null)
            {
                return new MemberDto();
            }
            var result = _mapper.Map<MemberDto>(mem);
            result.Role = 1; // Giả sử vai trò 1 là thành viên thường
            return result;
        }

    }
}
