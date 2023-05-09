using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using SaleAPI.Repository;

namespace SaleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        #region Dependency Injection
        private readonly SaleRepository _salesRepository;

        public SalesController(SaleRepository salesRepository)
        {
            _salesRepository = salesRepository;
        }
        #endregion

        #region Get
        //[HttpGet]
        //public ActionResult<List<Sale>> GetSales() => _salesRepository.GetSalesAsync().Result;

        //[HttpGet("{departure}")]
        //public ActionResult<List<Sale>> GetFlight() => _salesRepository.GetSalesAsync().Result;
        #endregion

        #region Post
        [HttpPost]
        public ActionResult<Sale> PostSale(SaleDTO sale)
        {
            if (_salesRepository.PostSalesAsync(sale).Result)
                return Ok(sale);
            else
                return BadRequest();
        }
        #endregion

        #region Put
        [HttpPut("{iata}/{rab}/{date}")]
        public ActionResult<Sale> PutSale(string cpf, string date) =>
        _salesRepository.PutSalesAsync(cpf, date).Result;
        #endregion

        #region Delete
        [HttpDelete("{cpf}/{Date}")]
        public ActionResult<Sale> DeleteSale(string cpf, string date) =>
            _salesRepository.DeleteSalesAsync(cpf, date).Result;
        #endregion
    }
}
