using FluentValidation;
using KRT.Services.Accounts.Application.Commands;
using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace KRT.Services.Accounts.API.Controllers;

/// <summary>
/// Controller que lida com requisições relacionadas a Contas.
/// </summary>
[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtém todas as Contas.
    /// </summary>
    /// <returns>Todas as Contas.</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAll()
    {
        var getAllAccountsQuery = new GetAllAccountsQuery();

        var result = await _mediator.Send(getAllAccountsQuery);

        if (!result.IsSuccess)
            return new ObjectResult(new { message = result.Message }) { StatusCode = result.StatusCode };

        return Ok(result.Value);
    }

    /// <summary>
    /// Obtém uma Conta por Id.
    /// </summary>
    /// <param name="id">Id da Conta.</param>
    /// <returns>A Conta correspondente.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetById(int id)
    {
        var getAccountByIdQuery = new GetAccountByIdQuery(id);

        var result = await _mediator.Send(getAccountByIdQuery);

        if (!result.IsSuccess)
            return new ObjectResult(new { message = result.Message }) { StatusCode = result.StatusCode };

        return Ok(result.Value);
    }

    /// <summary>
    /// Cria uma conta.
    /// </summary>
    /// <param name="command">Command contendo os detalhes da Conta.</param>
    /// <returns>Id da Conta.</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Post([FromBody] AddAccountCommand command, IValidator<AddAccountCommand> validator)
    {
        var validation = await validator.ValidateAsync(command);
        if (!validation.IsValid)
            return new BadRequestObjectResult(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return new ObjectResult(new { message = result.Message }) { StatusCode = result.StatusCode };

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, command);
    }

    /// <summary>
    /// Atualiza uma Conta a partir do Id.
    /// </summary>
    /// <param name="id">Id da Conta.</param>
    /// <param name="command">Command contendo os detalhes para atualizar uma Conta.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateAccountCommand command, IValidator<UpdateAccountCommand> validator)
    {
        var validation = await validator.ValidateAsync(command);
        if (!validation.IsValid)
            return new BadRequestObjectResult(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        command.Id = id;

        var result = await _mediator.Send(command);

        if(!result.IsSuccess)
            return new ObjectResult(new { message = result.Message }) { StatusCode = result.StatusCode };

        return NoContent();
    }

    /// <summary>
    /// Deleta uma Conta a partir do Id.
    /// </summary>
    /// <param name="id">Id da Conta.</param>
    /// <param name="command">Command contendo os detalhes para deletar uma Conta.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(int id, [FromBody] DeleteAccountCommand command)
    {
        command.Id = id;

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return new ObjectResult(new { message = result.Message }) { StatusCode = result.StatusCode };

        return NoContent();
    }
}
