using GSU.Museum.Web.Interfaces;
using GSU.Museum.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.Web.Services
{
    public class HomeService : IHomeService
    {
        private IHallsRepository _hallsRepository;

        public HomeService(IHallsRepository hallsRepository)
        {
            _hallsRepository = hallsRepository;
        }

        public async Task<List<HallViewModel>> GetAllAsync()
        {
            return await _hallsRepository.GetAllAsync();
        }
    }
}
