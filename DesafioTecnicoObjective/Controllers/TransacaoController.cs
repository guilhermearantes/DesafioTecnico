using DesafioTecnicoObjective.DTOs;
using DesafioTecnicoObjective.Exceptions;
using DesafioTecnicoObjective.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoObjective.Controllers
{
    /// <summary>
    /// Controller responsável pelo endpoint de transações em contas bancárias.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TransacaoController : ControllerBase
    {
        private readonly IContaService _service;

        /// <summary>
        /// Construtor que injeta o serviço de contas.
        /// </summary>
        /// <param name="service">Serviço de contas utilizado para operações de transação.</param>
        public TransacaoController(IContaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Realiza uma transação (débito, crédito ou pix) em uma conta bancária.
        /// </summary>
        /// <param name="dto">DTO contendo os dados da transação a ser realizada.</param>
        /// <returns>
        /// Retorna 201 (Created) com a conta atualizada em caso de sucesso,
        /// 404 (NotFound) se a transação for inválida,
        /// ou 400 (BadRequest) em caso de erro de negócio ou exceção.
        /// </returns>
        [HttpPost]
        public IActionResult RealizarTransacao([FromBody] TransacaoCreateDto dto)
        {
            var conta = _service.RealizarTransacao(dto);

            if (conta == null)
                return BadRequest(new { mensagem = "Saldo insuficiente." });

            return Created("", conta);
        }
    }
}
