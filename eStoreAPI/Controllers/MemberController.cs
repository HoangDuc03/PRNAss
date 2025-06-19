using BusinessObject.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IRepository<Member> _memberRepository;

        public MemberController(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }
        [HttpGet]
        public List<Member> getAll()
        {
            var query = _memberRepository.GetAll();
            return query.ToList();
        }


    }
}
