using B3.Domain.Interfaces;
using B3.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace B3.Application.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TituloController : Controller
    {
        private readonly ITituloService _tituloService;


        public TituloController(ITituloService tituloService)
        {
            _tituloService = tituloService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTitulos()
        {
            try
            {
                var titulos = await _tituloService.GetTitulos();
                return Ok(titulos);
            }
            catch (Exception ex)
            {
                return BadRequest("Aconteceu um erro ao tentar carregar os titulos: " + ex.Message);
            }


        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTituloById(Guid id)
        {
            try
            {
                var titulo = await _tituloService.GetTituloById(id);
                return Ok(titulo);
            }
            catch (Exception ex)
            {
                return BadRequest("Aconteceu um erro ao tentar carregar os titulos: " + ex.Message);
            }


        }

        [HttpGet("simulacao/{id}/{valorInicial}/{valorAporteMensal}/{qtdeMesesInvestimento}")]
        public async Task<IActionResult> GetSimulacaoTitulo(Guid id, float valorInicial, float valorAporteMensal, int qtdeMesesInvestimento)
        {
            try
            {
                var titulo =  await _tituloService.GetSimularTitulo(id, valorInicial, valorAporteMensal, qtdeMesesInvestimento);
                return Ok(titulo);
            }
            catch (Exception ex)
            {
                return BadRequest("Aconteceu um erro ao tentar carregar os titulos: " + ex.Message);
            }
        }
    }
}
