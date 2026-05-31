using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Models;

namespace CatalogoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private static readonly List<Produto> Produtos = new()
    {
        new Produto { Id = 1, Nome = "Notebook", Preco = 4500.00m },
        new Produto { Id = 2, Nome = "Mouse", Preco = 150.00m }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetAll()
    {
        return Ok(Produtos);
    }

    [HttpGet("{id}")]
    public ActionResult<Produto> GetById(int id)
    {
        var produto = Produtos.FirstOrDefault(p => p.Id == id);
        
        if (produto == null)
        {
            return NotFound(new { mensagem = $"Produto com ID {id} não encontrado." });
        }

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult<Produto> Create([FromBody] Produto novoProduto)
    {

        if (string.IsNullOrEmpty(novoProduto.Nome) || novoProduto.Preco <= 0)
        {
            return BadRequest(new { mensagem = "Dados inválidos para o produto." });
        }

        novoProduto.Id = Produtos.Any() ? Produtos.Max(p => p.Id) + 1 : 1;
        Produtos.Add(novoProduto);

        return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Produto produtoAtualizado)
    {
        var produtoExistente = Produtos.FirstOrDefault(p => p.Id == id);
        
        if (produtoExistente == null)
        {
            return NotFound(new { mensagem = "Produto não encontrado." });
        }

        produtoExistente.Nome = produtoAtualizado.Nome;
        produtoExistente.Preco = produtoAtualizado.Preco;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var produto = Produtos.FirstOrDefault(p => p.Id == id);
        
        if (produto == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        Produtos.Remove(produto);
        return NoContent();
    }
}