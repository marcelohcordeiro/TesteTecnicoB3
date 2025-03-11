using B3.Application.Inputs;
using B3.Domain.Interfaces;
using B3.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace B3.Application.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TituloController : ControllerBase
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
        

        [HttpPost("simulacao")]
        public async Task<IActionResult> GetSimulacaoTitulo([FromBody] SimulacaoTituloInputModel input)
        {
            try
            {
                var titulo = await _tituloService.GetSimularTitulo(input.IdTitulo, input.ValorInicial, input.QuantidadeMesesInvestimento);
                return Ok(titulo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
