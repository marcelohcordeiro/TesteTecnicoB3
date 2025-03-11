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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }


        }


        [HttpGet("renda-fixa")]
        public async Task<IActionResult> GetTitulosRendaFixa()
        {
            try
            {
                var titulo = await _tituloService.GetTitulosRendaFixa();
                return Ok(titulo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("simulacao/{id}/{valorInicial}/{qtdeMesesInvestimento}")]
        public async Task<IActionResult> GetSimulacaoTitulo(Guid id, decimal valorInicial, int qtdeMesesInvestimento)
        {
            try
            {
                var titulo =  await _tituloService.GetSimularTitulo(id, valorInicial, qtdeMesesInvestimento);
                return Ok(titulo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
