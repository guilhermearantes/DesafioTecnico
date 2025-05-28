using DesafioTecnicoObjective.DTOs;
using DesafioTecnicoObjective.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoObjective.Controllers
{
    /// <summary>
    /// Controller responsável pelos endpoints de criação e consulta de contas bancárias.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly IContaService _service;

        /// <summary>
        /// Construtor que injeta o serviço de contas.
        /// </summary>
        /// <param name="service">Serviço de contas utilizado para operações de conta.</param>
        public ContaController(IContaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria uma nova conta bancária.
        /// </summary>
        /// <param name="dto">DTO contendo os dados necessários para criação da conta.</param>
        /// <returns>
        /// Retorna 201 (Created) com a conta criada em caso de sucesso,
        /// ou 409 (Conflict) se a conta já existir.
        /// </returns>
        [HttpPost]
        public IActionResult CriarConta([FromBody] ContaCreateDto dto)
        {
            try
            {
                var conta = _service.CriarConta(dto);
                return Created("", conta);
            }
            catch
            {
                return Conflict("Conta já existe.");
            }
        }

        /// <summary>
        /// Obtém as informações de uma conta bancária pelo número da conta.
        /// </summary>
        /// <param name="numero_conta">Número identificador da conta.</param>
        /// <returns>
        /// Retorna 200 (OK) com os dados da conta, ou 404 (NotFound) se a conta não for encontrada.
        /// </returns>
        [HttpGet]
        public IActionResult ObterConta([FromQuery] int numero_conta)
        {
            var conta = _service.ObterConta(numero_conta);
            return conta == null ? NotFound() : Ok(conta);
        }
    }
}
